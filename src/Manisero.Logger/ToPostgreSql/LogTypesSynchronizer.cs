using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Dapper;
using Manisero.Logger.Configuration;
using Manisero.Utils;
using Npgsql;

namespace Manisero.Logger.ToPostgreSql
{
    public static class LogTypesSynchronizer
    {
        public static void Synchronize(
            LoggerConfig.DbConfig config)
        {
            var logAttributes = GetLogAttributes(config);
            CreateOrUpdateLogTypes(logAttributes, config);
        }

        private static ICollection<LogAttribute> GetLogAttributes(
            LoggerConfig.DbConfig config)
        {
            var allTypes = config
                           .AssembliesWithLogs
                           .SelectMany(x => x.DefinedTypes);

            var logTypes = new Dictionary<Guid, Type>();
            var logAttributes = new Dictionary<Guid, LogAttribute>();

            foreach (var type in allTypes)
            {
                if (type.IsAbstract || !type.ImplementedInterfaces.Contains(typeof(ILog)))
                {
                    continue;
                }

                var logAttribute = type.GetAttributeOrThrow<LogAttribute>(false);
                var existingLogType = logTypes.GetValueOrDefault(logAttribute.Code);

                if (existingLogType != null)
                {
                    throw new InvalidOperationException($"Found two Logs ('{existingLogType}', '{type}') with the same code ('{logAttribute.Code}')");
                }

                logTypes.Add(logAttribute.Code, type);
                logAttributes.Add(logAttribute.Code, logAttribute);
            }

            return logAttributes.Values;
        }

        private static void CreateOrUpdateLogTypes(
            ICollection<LogAttribute> logAttributes,
            LoggerConfig.DbConfig config)
        {
            const string sql = @"
INSERT INTO Logger.LogType
(Code,  Level,  MessageTemplate) VALUES
(@Code, @Level, @MessageTemplate)
ON CONFLICT (Code) DO UPDATE
SET
  Level = excluded.Level,
  MessageTemplate = excluded.MessageTemplate;";

            using (var transaction = new TransactionScope())
            using (var connection = new NpgsqlConnection(config.ConnectionString))
            {
                foreach (var logAttribute in logAttributes)
                {
                    connection.Execute(
                        sql,
                        new
                        {
                            logAttribute.Code,
                            logAttribute.Level,
                            logAttribute.MessageTemplate
                        });
                }

                transaction.Complete();
            }
        }
    }
}
