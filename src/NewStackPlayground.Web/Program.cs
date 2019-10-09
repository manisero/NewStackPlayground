using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

namespace NewStackPlayground.Web
{
    public class Program
    {
        public static void Main(
            string[] args)
        {
            var builder = CreateWebHostBuilder(args);

#if DEBUG
            RunNormally(builder);
#else
            RunAsService(builder);
#endif
        }

        public static IWebHostBuilder CreateWebHostBuilder(
            string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            return WebHost
                   .CreateDefaultBuilder(args)
                   .UseContentRoot(pathToContentRoot)
                   .ConfigureAppConfiguration(
                       x => x.AddJsonFile("connection_strings.json")
                             .AddJsonFile("logger_settings.json"))
                   .UseStartup<Startup>();
        }

        private static void RunNormally(
            IWebHostBuilder builder)
        {
            var host = builder.Build();

            host.Run();
        }

        private static void RunAsService(
            IWebHostBuilder builder)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            var host = builder
                       .UseContentRoot(pathToContentRoot)
                       .Build();

            host.RunAsService();
        }
    }
}
