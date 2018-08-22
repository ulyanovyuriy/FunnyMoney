using System;

namespace XMRN.Common.System
{
    public static class DoubleExtension
    {
        public static TimeSpan AsMilliseconds(this double ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }
    }
}
