using System;
using System.Runtime.CompilerServices;
using Manisero.Logger.BuiltInLogs;

namespace Manisero.Logger
{
    public interface ILogger
    {
        void Log(
            ILog log,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        void LogFatalError(
            Exception error,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        void LogFatalError(
            KnownException knownError,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        void LogError(
            Exception error,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);

        void LogError(
            KnownException knownError,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0);
    }

    internal class Logger : ILogger
    {
        private readonly Serilog.ILogger _serilogLogger;

        public Logger(
            Serilog.ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger;
        }

        public void Log(
            ILog log,
            string callerMember = null,
            int callerLine = 0)
        {
            using (LoggerFacade.PushToContext("LogCode", log.GetLogAttribute().Code))
            using (LoggerFacade.PushToContext("SourceMember", callerMember))
            using (LoggerFacade.PushToContext("SourceLine", callerLine))
            {
                log.Log(_serilogLogger);
            }
        }

        public void LogFatalError(
            Exception error,
            string callerMember = null,
            int callerLine = 0)
        {
            if (error is KnownException knownError)
            {
                LogFatalError(knownError, callerMember, callerLine);
            }
            else
            {
                Log(new UnknownFatalErrorLog(error), callerMember, callerLine);
            }
        }

        public void LogFatalError(
            KnownException knownError,
            string callerMember = null,
            int callerLine = 0)
        {
            Log(new KnownFatalErrorLog(knownError), callerMember, callerLine);
        }

        public void LogError(
            Exception error,
            string callerMember = null,
            int callerLine = 0)
        {
            if (error is KnownException knownError)
            {
                LogError(knownError, callerMember, callerLine);
            }
            else
            {
                Log(new UnknownErrorLog(error), callerMember, callerLine);
            }
        }

        public void LogError(
            KnownException knownError,
            string callerMember = null,
            int callerLine = 0)
        {
            Log(new KnownErrorLog(knownError), callerMember, callerLine);
        }
    }
}
