using System;
using System.Data;
using System.IO;
using XMRN.Common.Collections;
using XMRN.Common.System;

namespace XMRN.Common.Data
{
    public static class DataTableExtension
    {
        public static MemoryStream ToStream(this DataTable dt)
        {
            Guard.ArgumentNotNull(dt, nameof(dt));

            var ms = new MemoryStream();
            dt.WriteXml(ms, XmlWriteMode.WriteSchema);
            ms.Position = 0L;
            return ms;
        }

        public static byte[] ToArray(this DataTable dt)
        {
            using (var ms = dt.ToStream())
                return ms.ToArray();
        }

        public static DataTable Load(this DataTable dt, Stream stream)
        {
            Guard.ArgumentNotNull(dt, nameof(dt));

            dt.ReadXml(stream);
            return dt;
        }

        public static DataTable Load(this DataTable dt, byte[] data)
        {
            Guard.ArgumentNotNull(data, nameof(data));

            using (var ms = data.ToStream())
                return dt.Load(ms);
        }
    }
}
