using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NewStackPlayground.Application.Domain;
using NewStackPlayground.Gateway.Commands;
using NewStackPlayground.Gateway.Queries;

namespace NewStackPlayground.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : GatewayController
    {
        [HttpGet]
        public ActionResult<ICollection<Item>> Get()
        {
            return HandleQuery<ItemsQuery, ICollection<Item>>(new ItemsQuery());
        }
        
        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            return HandleQuery<ItemQuery, Item>(new ItemQuery { ItemId = id });
        }
        
        [HttpPost]
        public ActionResult<Item> Post([FromBody] CreateItemCommand command)
        {
            return HandleCommand<CreateItemCommand, Item>(command);
        }
        
        [HttpPut("{id}")]
        public ActionResult<Item> Put(int id, [FromBody] UpdateItemCommand command)
        {
            command.ItemId = id;
            return HandleCommand<UpdateItemCommand, Item>(command);
        }
        
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            return HandleCommand<DeleteItemCommand, bool>(new DeleteItemCommand { ItemId = id });
        }
    }
}
