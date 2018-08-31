using System;

namespace XMRN.Common.System
{
    public static class StringExtension
    {
        public static T ParseTo<T>(this string value, bool ignoreCase = true)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static long ParseToInt64(this string value)
        {
            return long.Parse(value);
        }

        public static int ParseToInt32(this string value)
        {
            return int.Parse(value);
        }

        public static double ParseToDouble(this string value)
        {
            return double.Parse(value);
        }

        public static string IfNull(this string value, string next)
        {
            if (value == null) return next;
            return value;
        }

        public static string IfNullOrEmpty(this string value, string next)
        {
            if (string.IsNullOrEmpty(value)) return next;
            return value;
        }
    }
}
