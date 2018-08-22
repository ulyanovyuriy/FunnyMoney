using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using XMRN.Common.Threading;

namespace XMRN.Common.Security.Cryptography
{
    public partial class CryptoContext : BaseContextScope<CryptoContext>
    {
        private Lazy<TripleDESCryptoServiceProvider> _tripleDES
            = new Lazy<TripleDESCryptoServiceProvider>(() => new TripleDESCryptoServiceProvider());

        public SymmetricAlgorithm TripleDES => _tripleDES.Value;

        public static byte[] EncryptByTripleDES(byte[] data, byte[] key, byte[] vector)
            => UseContextIfNotExists(ctx => ctx.EncryptByTripleDESProvider(data, key, vector));

        public byte[] EncryptByTripleDESProvider(byte[] data, byte[] key, byte[] vector)
        {
            using (var transform = TripleDES.CreateEncryptor(key, vector))
            {
                var result = transform.Transform(data, _buffer);
                return result;
            }
        }

        public static string EncryptByTripleDES(string data, byte[] key, byte[] vector, Encoding encoding = null)
            => UseContextIfNotExists(ctx => ctx.EncryptByTripleDESProvider(data, key, vector, encoding));

        public string EncryptByTripleDESProvider(string data, byte[] key, byte[] vector, Encoding encoding = null)
        {
            using (var transform = TripleDES.CreateEncryptor(key, vector))
            {
                var result = transform.Transform(data, encoding, _buffer);
                return result;
            }
        }

        public static byte[] DecryptByTripleDES(byte[] data, byte[] key, byte[] vector)
            => UseContextIfNotExists(ctx => ctx.DecryptByTripleDESProvider(data, key, vector));

        public byte[] DecryptByTripleDESProvider(byte[] data, byte[] key, byte[] vector)
        {
            using (var transform = TripleDES.CreateDecryptor(key, vector))
            {
                var result = transform.Transform(data, _buffer);
                return result;
            }
        }

        public static string DecryptByTripleDES(string data, byte[] key, byte[] vector, Encoding encoding = null)
            => UseContextIfNotExists(ctx => ctx.DecryptByTripleDESProvider(data, key, vector, encoding));

        public string DecryptByTripleDESProvider(string data, byte[] key, byte[] vector, Encoding encoding = null)
        {
            using (var transform = TripleDES.CreateDecryptor(key, vector))
            {
                var result = transform.Transform(data, encoding, _buffer);
                return result;
            }
        }

    }
}
