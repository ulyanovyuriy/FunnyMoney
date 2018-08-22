using System;
using System.Collections.Generic;
using System.Text;
using XMRN.Common.Threading;

namespace XMRN.Common.Security.Cryptography
{
    public partial class CryptoContext : BaseContextScope<CryptoContext>
    {
        private byte[] _buffer = new byte[Defaults.FileBufferSize];

        protected override void Dispose(bool disposing)
        {
            _buffer = null;
            base.Dispose(disposing);
        }
    }
}
