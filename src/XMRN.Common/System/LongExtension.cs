using System;

namespace XMRN.Common.System
{
    public static class LongExtension
    {
        public static TimeSpan AsMilliseconds(this long ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }
    }
}
