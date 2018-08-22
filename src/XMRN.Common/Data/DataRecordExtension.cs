using System;
using System.Data;

namespace XMRN.Common.Data
{
    public static class DataRecordExtension
    {
        public static string GetString(this IDataRecord record, string name)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            return record.GetString(record.GetOrdinal(name));
        }
    }
}
