using System;
using System.Collections.Generic;
using System.Data;
using XMRN.Common.System;

namespace XMRN.Common.Data
{
    public static partial class DataReaderExtension
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            Guard.ArgumentNotNull(reader, nameof(reader));

            while (reader.Read())
                yield return reader;
        }

        public static IEnumerable<T> AsEnumerable<T>(this IDataReader reader
            , Func<IDataRecord, T> map)
        {
            Guard.ArgumentNotNull(map, nameof(map));

            foreach (var record in reader.AsEnumerable())
                yield return map.Invoke(record);
        }
    }
}
