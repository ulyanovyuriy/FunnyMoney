using System;
using XMRN.Common.Security.Cryptography;

namespace XMRN.Common.Collections
{
    public static partial class ByteArrayExtension
    {
        public static byte[] Encrypt(this byte[] data, byte[] key, byte[] vector)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return CryptoContext.EncryptByTripleDES(data, key, vector);
        }

        public static byte[] Decrypt(this byte[] data, byte[] key, byte[] vector)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return CryptoContext.DecryptByTripleDES(data, key, vector);
        }
    }
}
