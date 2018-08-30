
using System.Collections.Generic;
using XMRN.Common.System;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        public static bool None<T>(this IEnumerable<T> items)
        {
            if (items == null) return true;

            return items.Any() == false;
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            items = Guard.ArgumentNotNull(items, nameof(items));
            action = Guard.ArgumentNotNull(action, nameof(action));

            foreach (var item in items)
                action(item);
        }
    }
}
