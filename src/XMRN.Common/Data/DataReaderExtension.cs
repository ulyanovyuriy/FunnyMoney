using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XMRN.Common.Data
{
    public static partial class DataReaderExtension
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (reader.Read())
                yield return reader;
        }

        public static DataTable AsDataTable(this IDataReader reader, string tableName = "t")
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var dt = new DataTable(tableName);
            dt.Load(reader);
            return dt;
        }
    }
}
