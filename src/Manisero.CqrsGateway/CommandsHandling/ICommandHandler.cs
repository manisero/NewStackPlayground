namespace Manisero.CqrsGateway.CommandsHandling
{
    public interface ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        TResult Handle(
            TCommand command,
            CommandContext context);
    }
}
