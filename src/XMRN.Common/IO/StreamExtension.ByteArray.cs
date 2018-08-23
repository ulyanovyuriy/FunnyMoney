using System.IO;

namespace XMRN.Common.IO
{
    public static partial class StreamExtension
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            return IOContext.ReadAllBytes(stream);
        }
    }
}
