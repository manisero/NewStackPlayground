using System;
using Serilog.Events;

namespace Manisero.Logger.BuiltInLogs
{
    [Log(
        "00000000-0000-0000-0000-000000000001",
        "Unknown fatal error occured.",
        LogEventLevel.Fatal)]
    internal struct UnknownFatalErrorLog : ILog
    {
        private readonly Exception _error;

        public UnknownFatalErrorLog(
            Exception error)
        {
            _error = error;
        }

        public void Log(
            Serilog.ILogger logger)
        {
            logger.LogException(this, _error);
        }
    }

    [Log(
        "00000000-0000-0000-0000-000000000002",
        "Known fatal error occured.",
        LogEventLevel.Fatal)]
    internal struct KnownFatalErrorLog : ILog
    {
        private readonly KnownException _error;

        public KnownFatalErrorLog(
            KnownException error)
        {
            _error = error;
        }

        public void Log(
            Serilog.ILogger logger)
        {
            using (LoggerFacade.PushToContext("ErrorCode", _error.ErrorCode))
            using (LoggerFacade.PushToContext("ErrorData", _error.GetData(), true))
            {
                logger.LogException(this, _error);
            }
        }
    }

    [Log(
        "00000000-0000-0000-0000-000000000003",
        "Unknown error occured.",
        LogEventLevel.Error)]
    internal struct UnknownErrorLog : ILog
    {
        private readonly Exception _error;

        public UnknownErrorLog(
            Exception error)
        {
            _error = error;
        }

        public void Log(
            Serilog.ILogger logger)
        {
            logger.LogException(this, _error);
        }
    }

    [Log(
        "00000000-0000-0000-0000-000000000004",
        "Known error occured.",
        LogEventLevel.Error)]
    internal struct KnownErrorLog : ILog
    {
        private readonly KnownException _error;

        public KnownErrorLog(
            KnownException error)
        {
            _error = error;
        }

        public void Log(
            Serilog.ILogger logger)
        {
            using (LoggerFacade.PushToContext("ErrorCode", _error.ErrorCode))
            using (LoggerFacade.PushToContext("ErrorData", _error.GetData(), true))
            {
                logger.LogException(this, _error);
            }
        }
    }
}
