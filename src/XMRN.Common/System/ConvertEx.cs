using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.System
{
    public static class ConvertEx
    {
        static DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnix(TimeSpan ts)
        {
            var dt = (UnixStart + ts).ToLocalTime();
            return dt;
        }

        public static DateTime FromUnix(long ms)
        {
            var ts = TimeSpan.FromMilliseconds(ms);
            return FromUnix(ts);
        }
    }
}
