namespace XMRN.Common.System
{
    public static class StringExtension
    {
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
    }
}
