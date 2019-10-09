using System;
using Manisero.Logger;

namespace NewStackPlayground.Web.ErrorHandling
{
    public static class ErrorContentFactory
    {
        public static object Create(
            Exception unknownError,
            string correlationId)
        {
            return new
            {
                CorrelationId = correlationId,
                Error = unknownError.ToString()
            };
        }

        public static object Create(
            KnownException knownError,
            string correlationId)
        {
            return new
            {
                CorrelationId = correlationId,
                knownError.ErrorCode,
                Data = knownError.GetData(),
                Error = knownError.ToString()
            };
        }
    }
}
