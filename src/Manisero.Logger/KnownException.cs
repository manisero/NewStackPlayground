using System;
using System.Collections.Concurrent;
using Manisero.Utils;

namespace Manisero.Logger
{
    public abstract class KnownException : Exception
    {
        public override string Message => GetType().Name;

        public Guid ErrorCode => this.GetKnownExceptionAttribute().ErrorCode;

        public abstract object GetData();
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class KnownExceptionAttribute : Attribute
    {
        public Guid ErrorCode { get; }

        public KnownExceptionAttribute(
            string errorCode)
        {
            ErrorCode = Guid.Parse(errorCode);
        }
    }

    internal static class KnownExceptionUtils
    {
        private static readonly ConcurrentDictionary<Type, KnownExceptionAttribute> KnownExceptionAttributes =
            new ConcurrentDictionary<Type, KnownExceptionAttribute>();

        public static KnownExceptionAttribute GetKnownExceptionAttribute(
            this KnownException exception)
            => KnownExceptionAttributes.GetOrAdd(
                exception.GetType(),
                exceptionType => exceptionType.GetAttributeOrThrow<KnownExceptionAttribute>());
    }
}
