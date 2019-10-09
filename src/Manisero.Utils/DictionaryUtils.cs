using System;
using System.Collections.Generic;

namespace Manisero.Utils
{
    public static class DictionaryUtils
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue value;

            return dictionary.TryGetValue(key, out value)
                ? value
                : defaultValue;
        }

        public static TValue? GetValueOrNull<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
            where TValue : struct
        {
            TValue value;

            return dictionary.TryGetValue(key, out value)
                ? value
                : (TValue?)null;
        }

        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> valueGetter)
        {
            TValue value;

            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueGetter();
                dictionary.Add(key, value);
            }

            return value;
        }

        public static TValue GetOrThrow<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<Exception> exceptionGetter)
        {
            TValue value;

            if (!dictionary.TryGetValue(key, out value))
            {
                throw exceptionGetter();
            }

            return value;
        }
    }
}
