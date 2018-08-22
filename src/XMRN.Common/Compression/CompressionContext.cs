using System;
using System.Collections.Generic;
using System.Text;
using XMRN.Common.Threading;

namespace XMRN.Common.Compression
{
    public partial class CompressionContext : BaseContextScope<CompressionContext>
    {
        private byte[] _buffer = new byte[Defaults.FileBufferSize];

        protected override void Dispose(bool disposing)
        {
            _buffer = null;
            base.Dispose(disposing);
        }
    }
}
