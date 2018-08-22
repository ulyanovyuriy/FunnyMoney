
using System.Collections.Generic;
using System.Data;
using XMRN.Common.Linq;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        public static IDataReader AsDataReader<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            return new ItemDataReader<T>(items);
        }
    }
}
