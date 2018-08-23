using System;
using System.Data;
using System.IO;

namespace XMRN.Common.Data
{
    public static class DataTableExtension
    {
        public static byte[] ToArray(this DataTable dt)
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));

            byte[] data;
            using (var ms = new MemoryStream())
            {
                dt.WriteXml(ms, XmlWriteMode.WriteSchema);
                data = ms.ToArray();
            }

            return data;
        }
    }
}
