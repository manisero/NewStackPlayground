using System;
using System.IO;
using System.Runtime.CompilerServices;
using Manisero.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;

namespace NewStackPlayground.Web.Utils
{
    public static class LoggerUtils
    {
        public static void LogWebError(
            this ILogger logger,
            Exception error,
            HttpRequest request,
            [CallerMemberName] string callerMember = null,
            [CallerLineNumber] int callerLine = 0)
        {
            var requestData = new
            {
                request.Method,
                Uri = request.GetEncodedPathAndQuery(),
                Body = GetRequestBody(request)
            };

            using (LoggerFacade.PushToContext("Request", requestData))
            {
                logger.LogError(error, callerMember, callerLine);
            }
        }

        private static string GetRequestBody(
            HttpRequest request)
        {
            request.EnableRewind(); // TODO: This will probably not work once body was read

            using (var reader = new StreamReader(request.Body))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                return reader.ReadToEnd();
            }
        }
    }
}
