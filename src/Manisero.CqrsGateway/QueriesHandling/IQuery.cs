namespace Manisero.CqrsGateway.QueriesHandling
{
    /// <summary>This is only marker interface.</summary>
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }
}
