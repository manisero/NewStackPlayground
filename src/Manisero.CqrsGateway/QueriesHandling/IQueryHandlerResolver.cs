namespace Manisero.CqrsGateway.QueriesHandling
{
    public interface IQueryHandlerResolver
    {
        IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>()
            where TQuery : IQuery<TResult>;
    }
}
