using System;
using System.IO;

namespace XMRN.Common.IO
{
    public partial class IOContext
    {
        public byte[] ToByteArray(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            CheckDisposed();

            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }

            Memory.Position = 0L;
            int len = 0;
            int rc;
            while ((rc = stream.Read(Buffer, 0, 0)) > 0)
            {
                checked
                {
                    len += rc;
                }
                Memory.Write(Buffer, 0, rc);
            }

            var data = new byte[len];
            if (len == 0) return data;

            Memory.Position = 0L;
            Memory.Read(data, 0, len);

            return data;
        }

        public static byte[] ReadAllBytes(Stream stream)
            => UseContextIfNotExists(ctx => ctx.ToByteArray(stream));
    }
}
