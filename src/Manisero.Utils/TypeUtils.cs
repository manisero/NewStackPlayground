using System;
using System.Reflection;

namespace Manisero.Utils
{
    public static class TypeUtils
    {
        public static TAttribute GetAttributeOrThrow<TAttribute>(
            this Type type,
            bool inherit = true)
            where TAttribute : Attribute
            => type.GetCustomAttribute<TAttribute>(inherit) ??
               throw new InvalidOperationException($"{type} is not decorated with {typeof(TAttribute).FullName}.");
    }
}
