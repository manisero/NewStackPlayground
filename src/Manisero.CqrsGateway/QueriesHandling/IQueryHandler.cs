namespace Manisero.CqrsGateway.QueriesHandling
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        QueryOutput<TResult> Handle(
            TQuery query,
            QueryContext context);
    }

    public class QueryOutput<TResult>
    {
        public TResult Result { get; set; }

        public int? TotalCount { get; set; }
    }
}
