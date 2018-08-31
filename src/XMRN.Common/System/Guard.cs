using System;
using System.Runtime.CompilerServices;
using XMRN.Common.Semantic.Regexp;

namespace XMRN.Common.System
{
    public static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ArgumentNotNull<T>(this T arg, string name)
        {
            if (arg == null) throw new ArgumentNullException($"{typeof(T).Name}: {name}");
            return arg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotSupported(this Enum @enum)
        {
            if (@enum == null) throw new ArgumentNullException(nameof(@enum));
            throw new NotSupportedException($"{@enum.GetType().Name}: {@enum}");
        }
    }
}
