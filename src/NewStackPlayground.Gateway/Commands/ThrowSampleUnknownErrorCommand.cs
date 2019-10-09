using System;
using Manisero.CqrsGateway.CommandsHandling;

namespace NewStackPlayground.Gateway.Commands
{
    public class ThrowSampleUnknownErrorCommand : ICommand<object>
    {
        public string Message { get; set; }
    }

    public class ThrowSampleUnknownErrorCommandHandler : ICommandHandler<ThrowSampleUnknownErrorCommand, object>
    {
        public object Handle(
            ThrowSampleUnknownErrorCommand command,
            CommandContext context)
            => throw new Exception(command.Message);
    }
}
