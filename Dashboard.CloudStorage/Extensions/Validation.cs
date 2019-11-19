using System;

namespace Dashboard.CloudStorage.Extensions
{
    public static class Validation
    {
        public static void ThrowIfNull<T>(this T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(this string source, string paramName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}