using Manisero.CqrsGateway.QueriesHandling;
using NewStackPlayground.Gateway.LogsAndErrors;

namespace NewStackPlayground.Gateway.Queries
{
    public class ThrowSampleKnownErrorQuery : IQuery<object>
    {
    }

    public class ThrowSampleKnownErrorQueryHandler : IQueryHandler<ThrowSampleKnownErrorQuery, object>
    {
        public QueryOutput<object> Handle(
            ThrowSampleKnownErrorQuery query,
            QueryContext context)
            => throw new SampleKnownError();
    }
}
