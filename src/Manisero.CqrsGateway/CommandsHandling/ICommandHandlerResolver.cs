namespace Manisero.CqrsGateway.CommandsHandling
{
    public interface ICommandHandlerResolver
    {
        ICommandHandler<TCommand, TResult> Resolve<TCommand, TResult>()
            where TCommand : ICommand<TResult>;
    }
}
