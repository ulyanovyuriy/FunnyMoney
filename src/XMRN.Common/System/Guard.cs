using System;
using System.Runtime.CompilerServices;

namespace XMRN.Common.System
{
    public static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNotNull(object arg, string name)
        {
            if (arg == null) throw new ArgumentNullException(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ArgumentNotNull<T>(T arg, string name)
        {
            if (arg == null) throw new ArgumentNullException(name);
            return arg;
        }
    }
}
