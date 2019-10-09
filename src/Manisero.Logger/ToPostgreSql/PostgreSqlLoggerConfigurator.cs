using System;
using System.Collections.Generic;
using Manisero.Logger.Configuration;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace Manisero.Logger.ToPostgreSql
{
    public static class PostgreSqlLoggerConfigurator
    {
        public static void Configure(
            LoggerConfiguration serilogConfiguration,
            LoggerConfig.DbConfig config)
        {
            SchemaUpdater.Update(config);
            LogTypesSynchronizer.Synchronize(config);
            
            serilogConfiguration
                .WriteTo.PostgreSQL(
                    config.ConnectionString,
                    "Log",
                    new Dictionary<string, ColumnWriterBase>
                    {
                        ["Timestamp"] = new TimestampUtcColumnWriter(),
                        ["TypeCode"] = new SinglePropertyColumnWriter("LogCode", PropertyWriteMethod.Raw, NpgsqlDbType.Uuid),
                        ["Properties"] = new PropertiesColumnWriter(),
                        ["Exception"] = new ExceptionColumnWriter()
                    },
                    LogEventLevel.Information,
                    schemaName: "Logger",
                    period: TimeSpan.FromSeconds(2));
        }

        private class TimestampUtcColumnWriter : ColumnWriterBase
        {
            public TimestampUtcColumnWriter()
                : base(NpgsqlDbType.Timestamp)
            {
            }

            public override object GetValue(
                LogEvent logEvent,
                IFormatProvider formatProvider = null)
                => logEvent.Timestamp.UtcDateTime;
        }
    }
}
