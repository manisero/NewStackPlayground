using Microsoft.AspNetCore.Http;

namespace NewStackPlayground.Web.Utils
{
    public static class HttpContextUtils
    {
        public static string GetCorrelationId(
            this HttpContext context)
            => context.TraceIdentifier;
    }
}
