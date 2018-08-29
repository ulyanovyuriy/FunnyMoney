
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        public static bool None<T>(this IEnumerable<T> items)
        {
            if (items == null) return true;

            return items.Any() == false;
        }
    }
}
