using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.Collections
{
    public static class ByteArrayExtensions
    {
        public static string ToHex(this byte[] data)
        {
            return ToHex(data, 0);
        }

        public static string ToHex(this byte[] data, int offset)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return ToHex(data, offset, data.Length);
        }

        public static string ToHex(this byte[] data, int offset, int count)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (count <= 0) return string.Empty;

            if (offset >= data.Length) return string.Empty;

            var hex = new StringBuilder(count * 2);
            var max = Math.Min(data.Length, offset + count);
            for (int i = offset; i < max; i++)
            {
                var b = data[i];
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
    }
}
