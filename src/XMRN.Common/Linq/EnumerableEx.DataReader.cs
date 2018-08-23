
using System.Collections.Generic;
using System.Data;
using XMRN.Common.Linq;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        public static IDataReader AsDataReader<T>(this IEnumerable<T> items, Action<ItemMapBuilder<T>> init)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var mb = new ItemMapBuilder<T>();
            init?.Invoke(mb);
            var maps = mb.Build();

            return new ItemDataReader<T>(items, maps);
        }

        public static IDataReader AsDataReader<T>(this IEnumerable<T> items)
        {
            return items.AsDataReader(mb => mb.AddAllMembers());
        }
    }
}
