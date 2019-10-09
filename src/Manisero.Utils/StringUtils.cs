namespace Manisero.Utils
{
    public static class StringUtils
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static string NullIfEmpty(this string value) => value.IsNullOrEmpty() ? null : value;
    }
}
