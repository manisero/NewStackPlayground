using System;
using Manisero.Logger.Configuration;
using Serilog;
using Serilog.Context;

namespace Manisero.Logger
{
    public static class LoggerFacade
    {
        private static readonly object Lock = new object();

        private static Serilog.ILogger _serilogLogger;

        public static void Configure(
            LoggerConfig config)
        {
            lock (Lock)
            {
                if (_serilogLogger != null)
                {
                    throw new InvalidOperationException("Logger already configured.");
                }

                var serilogConfiguration = new LoggerConfiguration();
                LoggerConfigurator.Configure(serilogConfiguration, config);

                _serilogLogger = serilogConfiguration.CreateLogger();
            }
        }

        public static ILogger GetLogger()
        {
            if (_serilogLogger == null)
            {
                throw new InvalidOperationException("Logger not configured.");
            }

            var callingType = LoggerUtils.Utils.GetCallingType();
            var serilogLogger = _serilogLogger.ForContext(callingType);

            return new Logger(serilogLogger);
        }

        public static IDisposable PushToContext<TValue>(
            string key,
            TValue value,
            bool destructureObjects = false)
        {
            return LogContext.PushProperty(key, value, destructureObjects);
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
