using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.System
{
    public static class TimeSpanExtension
    {
        public static DateTime FromUnix(this TimeSpan ts)
        {
            return ConvertEx.FromUnix(ts);
        }
    }
}
