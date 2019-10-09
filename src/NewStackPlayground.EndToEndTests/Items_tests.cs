using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NewStackPlayground.Application;
using NewStackPlayground.Application.Domain;
using NewStackPlayground.EndToEndTests.Utils;
using Newtonsoft.Json;
using Xunit;

namespace NewStackPlayground.EndToEndTests
{
    public class Items_tests
    {
        [Fact]
        public async Task get_Item_by_id___Item_returned()
        {
            using (var httpClient = new HttpClient())
            {
                // Arrange
                httpClient.BaseAddress = new Uri(ConfigUtils.GetConfig()["WebUrl"]);

                var itemToCreate = new Item
                {
                    Name = Guid.NewGuid().ToString()
                };

                // Act & Assert
                var postResponse = await httpClient.PostAsync(
                    "api/items/",
                    new StringContent(
                        JsonConvert.SerializeObject(itemToCreate),
                        Encoding.UTF8,
                        "application/json"));

                postResponse.IsSuccessStatusCode.Should().BeTrue();

                var getResponse = await httpClient.GetStringAsync("api/items");
                var allItems = JsonConvert.DeserializeObject<Item[]>(getResponse);
                
                allItems.Should().Contain(x => x.Name == itemToCreate.Name);

                var createdItem = allItems.Single(x => x.Name == itemToCreate.Name);

                var deleteResponse = await httpClient.DeleteAsync($"api/items/{createdItem.ItemId}");
                deleteResponse.IsSuccessStatusCode.Should().BeTrue();

                var get2Response = await httpClient.GetStringAsync("api/items");
                var allItems2 = JsonConvert.DeserializeObject<Item[]>(get2Response);

                allItems2.Should().NotContain(x => x.Name == itemToCreate.Name);
            }
        }
    }
}
