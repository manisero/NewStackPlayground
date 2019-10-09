using System;
using System.Threading.Tasks;
using Manisero.Logger;
using Microsoft.AspNetCore.Http;
using NewStackPlayground.Web.Utils;
using Newtonsoft.Json;

namespace NewStackPlayground.Web.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private static readonly ILogger Logger = LoggerFacade.GetLogger();

        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                using (LoggerFacade.PushToContext("CorrelationId", context.GetCorrelationId()))
                {
                    Logger.LogWebError(ex, context.Request);
                }

                var content = ErrorContentFactory.Create(ex, context.GetCorrelationId());
                var contentJson = JsonConvert.SerializeObject(content);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(contentJson);
            }
        }
    }
}
