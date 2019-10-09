using System;
using System.Diagnostics;
using DbUp;
using Microsoft.Extensions.Configuration;
using NewStackPlayground.DbMigrations;
using Npgsql;

namespace NewStackPlayground.DbMigrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = GetConnectionString(args);

            var upgrader = DeployChanges
                           .To.PostgresqlDatabase(connectionString)
                           .WithScripts(new ScriptsProvider(typeof(DbMigrationsAssemblyMarker).Assembly))
                           .WithTransaction()
                           .LogToConsole()
                           .LogScriptOutput()
                           .Build();

            var result = upgrader.PerformUpgrade();

            if (result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

            return result.Successful ? 0 : -1;
        }

        private static string GetConnectionString(
            string[] args)
        {
            var connectionString = GetConfig().GetConnectionString("Default");

            if (args.Length > 0)
            {
                var dbName = args[0];

                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
                {
                    Database = dbName
                };

                connectionString = connectionStringBuilder.ConnectionString;
            }

            return connectionString;
        }

        private static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
                   .AddJsonFile("connection_strings.json")
                   .Build();
        }
    }
}
