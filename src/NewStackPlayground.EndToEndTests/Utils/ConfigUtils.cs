using Microsoft.Extensions.Configuration;

namespace NewStackPlayground.EndToEndTests.Utils
{
    public static class ConfigUtils
    {
        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
                                                            .AddJsonFile("appsettings.json")
                                                            .Build();

        public static IConfigurationRoot GetConfig()
        {
            return Config;
        }
    }
}
