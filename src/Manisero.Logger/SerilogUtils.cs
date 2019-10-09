using System;

namespace Manisero.Logger
{
    public static class SerilogUtils
    {
        public static void Log(
            this Serilog.ILogger logger, ILog log)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, logAttribute.MessageTemplate);
        }

        public static void Log<TProp1>(
            this Serilog.ILogger logger, ILog log, TProp1 val1)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, logAttribute.MessageTemplate, val1);
        }

        public static void Log<TProp1, TProp2>(
            this Serilog.ILogger logger, ILog log, TProp1 val1, TProp2 val2)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, logAttribute.MessageTemplate, val1, val2);
        }

        public static void Log<TProp1, TProp2, TProp3>(
            this Serilog.ILogger logger, ILog log, TProp1 val1, TProp2 val2, TProp3 val3)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, logAttribute.MessageTemplate, val1, val2, val3);
        }

        public static void Log(
            this Serilog.ILogger logger, ILog log, params object[] vals)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, logAttribute.MessageTemplate, vals);
        }

        public static void LogException(
            this Serilog.ILogger logger, ILog log, Exception ex)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, ex, logAttribute.MessageTemplate);
        }

        public static void LogException<TProp1>(
            this Serilog.ILogger logger, ILog log, Exception ex, TProp1 val1)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, ex, logAttribute.MessageTemplate, val1);
        }

        public static void LogException<TProp1, TProp2>(
            this Serilog.ILogger logger, ILog log, Exception ex, TProp1 val1, TProp2 val2)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, ex, logAttribute.MessageTemplate, val1, val2);
        }

        public static void LogException<TProp1, TProp2, TProp3>(
            this Serilog.ILogger logger, ILog log, Exception ex, TProp1 val1, TProp2 val2, TProp3 val3)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, ex, logAttribute.MessageTemplate, val1, val2, val3);
        }

        public static void LogException(
            this Serilog.ILogger logger, ILog log, Exception ex, params object[] vals)
        {
            var logAttribute = log.GetLogAttribute();
            logger.Write(logAttribute.Level, ex, logAttribute.MessageTemplate, vals);
        }
    }
}
