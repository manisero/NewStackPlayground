using System;
using System.Diagnostics;
using System.IO;
using Manisero.Logger.ToPostgreSql;
using Serilog;
using Serilog.Debugging;

namespace Manisero.Logger.Configuration
{
    internal static class LoggerConfigurator
    {
        private static readonly Lazy<string> LogsFolderPath = new Lazy<string>(
            () => Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")).FullName);

        public static void Configure(
            LoggerConfiguration serilogConfiguration,
            LoggerConfig config)
        {
            ConfigureSelfLog();

            serilogConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithProcessName()
                .Enrich.WithDemystifiedStackTraces();

            WriteToFile(serilogConfiguration);

            if (config.Db != null)
            {
                PostgreSqlLoggerConfigurator.Configure(serilogConfiguration, config.Db);
            }

            if (config.Seq != null)
            {
                WriteToSeq(serilogConfiguration, config.Seq);
            }

            if (LoggerUtils.Utils.IsConsoleAvailable() || Debugger.IsAttached)
            {
                WriteToConsole(serilogConfiguration);
            }
        }

        private static void ConfigureSelfLog()
        {
            var selfLogPath = Path.Combine(LogsFolderPath.Value, "logger_errors.txt");
            var file = File.AppendText(selfLogPath);

            SelfLog.Enable(TextWriter.Synchronized(file));
        }

        private static void WriteToFile(
            LoggerConfiguration serilogConfiguration)
        {
            var logFilePath = Path.Combine(LogsFolderPath.Value, "log.txt");

            serilogConfiguration
                .WriteTo.Async(
                    c => c.File(
                        logFilePath,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}{NewLine}",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true));
        }

        private static void WriteToSeq(
            LoggerConfiguration serilogConfiguration,
            LoggerConfig.SeqConfig config)
        {
            var serverUrl = $"http://localhost:{config.Port}";

            serilogConfiguration
                .WriteTo.Seq(
                    serverUrl,
                    eventBodyLimitBytes: null);
        }

        private static void WriteToConsole(
            LoggerConfiguration serilogConfiguration)
        {
            serilogConfiguration.MinimumLevel.Verbose(); // TODO: Needed?

            serilogConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}{NewLine}");
        }
    }
}
