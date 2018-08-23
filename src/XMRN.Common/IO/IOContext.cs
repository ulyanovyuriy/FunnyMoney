using System.IO;
using XMRN.Common.Threading;

namespace XMRN.Common.IO
{
    public partial class IOContext : BaseContextScope<IOContext>
    {
        private MemoryStream Memory = new MemoryStream(new byte[Defaults.FileBufferSize]);

        private byte[] Buffer = new byte[Defaults.FileBufferSize];

        protected override void Dispose(bool disposing)
        {
            Memory.Dispose();
            Memory = null;
            Buffer = null;
            base.Dispose(disposing);
        }
    }
}
