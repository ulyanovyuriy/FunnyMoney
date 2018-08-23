using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XMRN.Common.IO;

namespace XMRN.Common.Data
{
    public static partial class DataReaderExtension
    {
        public static void ExportToCsv(this IDataReader reader
            , StringBuilder sb, bool includeHeaders = true)
        {
            IOContext.ExportToCsv(reader, sb, includeHeaders);
        }
    }
}
