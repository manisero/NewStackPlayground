using System;
using System.Collections.Generic;
using System.Linq;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.ScriptProviders;
using Manisero.Logger.Configuration;

namespace Manisero.Logger.ToPostgreSql
{
    public static class SchemaUpdater
    {
        private class ScriptsProvider : IScriptProvider
        {
            private readonly EmbeddedScriptProvider _embeddedScriptProvider = new EmbeddedScriptProvider(
                typeof(PostgreSqlLoggerConfigurator).Assembly,
                x => x.EndsWith(".sql"));

            public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
            {
                return _embeddedScriptProvider
                       .GetScripts(connectionManager)
                       .Select(x => new SqlScript(
                                   x.Name.Split('.').Reverse().ElementAt(1),
                                   x.Contents));
            }
        }

        public static void Update(
            LoggerConfig.DbConfig config)
        {
            var upgrader = DeployChanges
                           .To.PostgresqlDatabase(config.ConnectionString, "logger")
                           .WithScripts(new ScriptsProvider())
                           .WithTransaction()
                           .LogToConsole()
                           .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new InvalidOperationException(
                    "Error while updating Logger database schema. See inner exception for details.",
                    result.Error);
            }
        }
    }
}
