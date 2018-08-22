using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XMRN.Common.Security.Cryptography
{
    public static class CryptoTransformExtension
    {
        public static void Transform(this ICryptoTransform crypto
            , Stream input
            , Stream output
            , byte[] buffer = null)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (var cs = new CryptoStream(output, crypto, CryptoStreamMode.Write))
            {
                buffer = buffer ?? new byte[Defaults.FileBufferSize];
                int rc;
                while ((rc = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, rc);
                }
                cs.Flush();
            }
        }

        public static byte[] Transform(this ICryptoTransform crypto
            , byte[] data
            , byte[] buffer = null)
        {
            using (var input = new MemoryStream(data))
            using (var output = new MemoryStream())
            {
                crypto.Transform(input, output, buffer);
                var result = output.ToArray();
                return result;
            }
        }

        public static string Transform(this ICryptoTransform crypto
            , string data
            , Encoding encoding = null
            , byte[] buffer = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            encoding = encoding ?? Defaults.Encoding;
            var resultData = crypto.Transform(encoding.GetBytes(data), buffer);
            var result = encoding.GetString(resultData);

            return result;
        }
    }
}
