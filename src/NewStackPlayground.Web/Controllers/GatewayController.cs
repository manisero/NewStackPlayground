using Manisero.CqrsGateway.CommandsHandling;
using Manisero.CqrsGateway.QueriesHandling;
using Manisero.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewStackPlayground.Web.ErrorHandling;
using NewStackPlayground.Web.Utils;

namespace NewStackPlayground.Web.Controllers
{
    public abstract class GatewayController : ControllerBase
    {
        private static readonly ILogger Logger = LoggerFacade.GetLogger();

        protected ActionResult<TResult> HandleCommand<TCommand, TResult>(
            TCommand command)
            where TCommand : ICommand<TResult>
        {
            var context = new CommandContext { CorrelationId = HttpContext.GetCorrelationId() };

            using (LoggerFacade.PushToContext("CorrelationId", context.CorrelationId))
            {
                var commandResult = Startup.AppGateway.CommandsGateway.Handle<TCommand, TResult>(command, context);

                if (commandResult.UnknownError != null)
                {
                    Logger.LogWebError(commandResult.UnknownError, Request);

                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ErrorContentFactory.Create(commandResult.UnknownError, context.CorrelationId));
                }
                else if (commandResult.KnownError != null)
                {
                    Logger.LogWebError(commandResult.KnownError, Request);

                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ErrorContentFactory.Create(commandResult.KnownError, context.CorrelationId));
                }
                else if (commandResult.ValidationErrors != null)
                {
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        commandResult.ValidationErrors);
                }
                else
                {
                    return commandResult.Result;
                }
            }
        }

        protected ActionResult<TResult> HandleQuery<TQuery, TResult>(
            TQuery query)
            where TQuery : IQuery<TResult>
        {
            var context = new QueryContext { CorrelationId = HttpContext.GetCorrelationId() };

            using (LoggerFacade.PushToContext("CorrelationId", context.CorrelationId))
            {
                var queryResult = Startup.AppGateway.QueriesGateway.Handle<TQuery, TResult>(query, context);

                if (queryResult.UnknownError != null)
                {
                    Logger.LogWebError(queryResult.UnknownError, Request);

                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ErrorContentFactory.Create(queryResult.UnknownError, context.CorrelationId));
                }
                else if (queryResult.KnownError != null)
                {
                    Logger.LogWebError(queryResult.KnownError, Request);

                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        ErrorContentFactory.Create(queryResult.KnownError, context.CorrelationId));
                }
                else if (queryResult.Output.Result == null)
                {
                    return NotFound();
                }
                else
                {
                    if (queryResult.Output.TotalCount.HasValue)
                    {
                        Response.Headers.Add("x-total-count", queryResult.Output.TotalCount.Value.ToString());
                    }

                    return queryResult.Output.Result;
                }
            }
        }
    }
}
