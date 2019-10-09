using Manisero.CqrsGateway.CommandsHandling;
using NewStackPlayground.Gateway.LogsAndErrors;

namespace NewStackPlayground.Gateway.Commands
{
    public class ThrowSampleKnownErrorCommand : ICommand<object>
    {
        public string Message { get; set; }
    }

    public class ThrowSampleKnownErrorCommandHandler : ICommandHandler<ThrowSampleKnownErrorCommand, object>
    {
        public object Handle(
            ThrowSampleKnownErrorCommand command,
            CommandContext context)
            => throw new SampleKnownError(command.Message);
    }
}
