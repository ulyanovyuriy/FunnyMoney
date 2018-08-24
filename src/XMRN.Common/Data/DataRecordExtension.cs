using System;
using System.Data;
using XMRN.Common.System;

namespace XMRN.Common.Data
{
    public static class DataRecordExtension
    {
        public static string GetString(this IDataRecord record, string name)
        {
            Guard.ArgumentNotNull(record, nameof(record));
            return record.GetString(record.GetOrdinal(name));
        }

        public static DateTime GetDateTime(this IDataRecord record, string name)
        {
            Guard.ArgumentNotNull(record, nameof(record));
            return record.GetDateTime(record.GetOrdinal(name));
        }

        public static int GetInt32(this IDataRecord record, string name)
        {
            Guard.ArgumentNotNull(record, nameof(record));
            return record.GetInt32(record.GetOrdinal(name));
        }
    }
}
