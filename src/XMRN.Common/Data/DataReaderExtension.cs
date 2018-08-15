using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XMRN.Common.Data
{
    public static class DataReaderExtension
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            while (reader.Read())
                yield return reader;
        }
    }
}
