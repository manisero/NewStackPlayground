using Manisero.CqrsGateway.CommandsHandling;
using Manisero.CqrsGateway.QueriesHandling;
using Manisero.Logger;

namespace Manisero.CqrsGateway.LogsAndErrors
{
    [Log(
        "17DD2BDD-9342-4D78-9F21-66A8FF9660D0",
        "Command received: {@Command}. Context: {@Context}.")]
    public struct CommandReceivedLog : ILog
    {
        private readonly ICommand _command;
        private readonly CommandContext _context;

        public CommandReceivedLog(
            ICommand command,
            CommandContext context)
        {
            _command = command;
            _context = context;
        }

        public void Log(Serilog.ILogger logger)
        {
            logger.Log(this, _command, _context);
        }
    }

    [Log(
        "4BFAAE03-3229-4334-A314-4A23B4D6ED36",
        "Query received: {@Query}. Context: {@Context}.")]
    public struct QueryReceivedLog : ILog
    {
        private readonly IQuery _query;
        private readonly QueryContext _context;

        public QueryReceivedLog(
            IQuery query,
            QueryContext context)
        {
            _query = query;
            _context = context;
        }

        public void Log(Serilog.ILogger logger)
        {
            logger.Log(this, _query, _context);
        }
    }
}
