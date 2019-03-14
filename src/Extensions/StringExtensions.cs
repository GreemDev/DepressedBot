using System;

namespace DepressedBot.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string str, string otherString)
        {
            if (str is null) return false;
            return str.Equals(otherString, StringComparison.OrdinalIgnoreCase);
        }


        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (str is null) return false;
            return str.Contains(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNullOrWhitespace(this string str) => string.IsNullOrWhiteSpace(str);

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}
