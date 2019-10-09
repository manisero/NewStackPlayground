using System;
using System.Collections.Concurrent;
using Manisero.Utils;
using Serilog.Events;

namespace Manisero.Logger
{
    public interface ILog
    {
        void Log(
            Serilog.ILogger logger);
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public class LogAttribute : Attribute
    {
        public Guid Code { get; }
        public LogEventLevel Level { get; }
        public string MessageTemplate { get; }

        public LogAttribute(
            string code,
            string messageTemplate,
            LogEventLevel level = LogEventLevel.Information)
        {
            Code = Guid.Parse(code);
            Level = level;
            MessageTemplate = messageTemplate;
        }
    }

    internal static class LogUtils
    {
        private static readonly ConcurrentDictionary<Type, LogAttribute> LogAttributes =
            new ConcurrentDictionary<Type, LogAttribute>();

        public static LogAttribute GetLogAttribute(
            this ILog log)
            => LogAttributes.GetOrAdd(
                log.GetType(),
                logType => logType.GetAttributeOrThrow<LogAttribute>());
    }
}
