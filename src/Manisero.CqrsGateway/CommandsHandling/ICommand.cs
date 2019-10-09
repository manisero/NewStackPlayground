namespace Manisero.CqrsGateway.CommandsHandling
{
    /// <summary>This is only marker interface.</summary>
    public interface ICommand
    {
    }

    public interface ICommand<TResult> : ICommand
    {
    }
}
