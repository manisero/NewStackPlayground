using System;
using Manisero.CqrsGateway.LogsAndErrors;
using Manisero.CqrsGateway.QueriesHandling;
using Manisero.Logger;

namespace Manisero.CqrsGateway
{
    public interface IQueriesGateway
    {
        QueryResult<TResult> Handle<TQuery, TResult>(
            TQuery query,
            QueryContext context)
            where TQuery : IQuery<TResult>;
    }

    public class QueriesGateway : IQueriesGateway
    {
        private static readonly ILogger Logger = LoggerFacade.GetLogger();

        private readonly IQueryHandlerResolver _queryHandlerResolver;
        private readonly Func<IDisposable> _scopeFactory;

        public QueriesGateway(
            IQueryHandlerResolver queryHandlerResolver,
            Func<IDisposable> scopeFactory)
        {
            _queryHandlerResolver = queryHandlerResolver;
            _scopeFactory = scopeFactory;
        }

        public QueryResult<TResult> Handle<TQuery, TResult>(
            TQuery query,
            QueryContext context)
            where TQuery : IQuery<TResult>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Logger.Log(new QueryReceivedLog(query, context));

            using (_scopeFactory())
            {
                try
                {
                    var queryHandler = _queryHandlerResolver.Resolve<TQuery, TResult>();
                    var output = queryHandler.Handle(query, context);

                    return new QueryResult<TResult> { Output = output };
                }
                catch (KnownException e)
                {
                    return new QueryResult<TResult> { KnownError = e };
                }
                catch (Exception ex)
                {
                    return new QueryResult<TResult> { UnknownError = ex };
                }
            }
        }
    }
}
