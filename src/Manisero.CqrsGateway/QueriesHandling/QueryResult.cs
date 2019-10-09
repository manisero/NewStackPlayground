using System;
using Manisero.Logger;

namespace Manisero.CqrsGateway.QueriesHandling
{
    public class QueryResult<TResult>
    {
        public QueryOutput<TResult> Output { get; set; }

        public KnownException KnownError { get; set; }

        public Exception UnknownError { get; set; }
    }
}
