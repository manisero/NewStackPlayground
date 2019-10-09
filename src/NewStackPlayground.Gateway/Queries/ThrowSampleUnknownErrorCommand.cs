using System;
using Manisero.CqrsGateway.QueriesHandling;

namespace NewStackPlayground.Gateway.Queries
{
    public class ThrowSampleUnknownErrorQuery : IQuery<object>
    {
    }

    public class ThrowSampleUnknownErrorQueryHandler : IQueryHandler<ThrowSampleUnknownErrorQuery, object>
    {
        public QueryOutput<object> Handle(
            ThrowSampleUnknownErrorQuery query,
            QueryContext context)
            => throw new Exception("Sample unknown error.");
    }
}
