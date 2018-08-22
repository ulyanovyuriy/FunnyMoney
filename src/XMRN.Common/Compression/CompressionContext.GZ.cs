using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace XMRN.Common.Compression
{
    public partial class CompressionContext
    {
        public static void CompressByGZ(Stream input, Stream output)
            => UseContextIfNotExists(ctx => ctx.CompressByGZProvider(input, output));

        public void CompressByGZProvider(Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (var gzs = new GZipStream(output, CompressionMode.Compress, true))
            {
                int rc;
                while ((rc = input.Read(_buffer, 0, _buffer.Length)) > 0)
                    gzs.Write(_buffer, 0, rc);
            }
        }

        public static byte[] CompressByGZ(byte[] data)
            => UseContextIfNotExists(ctx => ctx.CompressByGZProvider(data));

        public byte[] CompressByGZProvider(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                CompressByGZProvider(input, output);

                var result = output.ToArray();
                return result;
            }
        }

        public static void DecompressByGZ(Stream input, Stream output)
            => UseContextIfNotExists(ctx => ctx.DecompressByGZProvider(input, output));

        public void DecompressByGZProvider(Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(output));

            using (var gzs = new GZipStream(input, CompressionMode.Decompress, true))
            {
                int rc;
                while ((rc = gzs.Read(_buffer, 0, _buffer.Length)) > 0)
                    output.Write(_buffer, 0, rc);
            }
        }

        public static byte[] DecompressByGZ(byte[] data)
            => UseContextIfNotExists(ctx => ctx.DecompressByGZProvider(data));

        public byte[] DecompressByGZProvider(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                DecompressByGZProvider(input, output);

                var result = output.ToArray();
                return result;
            }
        }
    }
}
