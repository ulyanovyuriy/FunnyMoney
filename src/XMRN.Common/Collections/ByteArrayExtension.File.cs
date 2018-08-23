using System;
using System.IO;

namespace XMRN.Common.Collections
{
    public static partial class ByteArrayExtension
    {
        public static void WriteTo(this byte[] data, string fileName)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            File.WriteAllBytes(fileName, data);
        }
    }
}
