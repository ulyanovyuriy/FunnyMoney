using System;
using XMRN.Common.Compression;

namespace XMRN.Common.Collections
{
    public static partial class ByteArrayExtension
    {
        public static byte[] Compress(this byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return CompressionContext.CompressByGZ(data);
        }

        public static byte[] Decompress(this byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return CompressionContext.DecompressByGZ(data);
        }
    }
}
