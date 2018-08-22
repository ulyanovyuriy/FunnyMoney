using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace XMRN.Common.IO
{
    public partial class IOContext
    {
        public static void ExportToCsv(IDataReader reader
             , TextWriter writer
             , bool includeHeaders = true)
        => UseContextIfNotExists(ctx => ctx.WriteToCsv(reader, writer, includeHeaders));

        public void WriteToCsv(IDataReader reader
            , TextWriter writer
            , bool includeHeaders = true)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            using (var csv = new CsvWriter(writer, true))
            {
                if (includeHeaders)
                {
                    var fields = Enumerable.Range(0, reader.FieldCount)
                        .Select(f => reader.GetName(f))
                        .ToArray();

                    csv.Write(fields);
                }

                object[] values = new object[reader.FieldCount];
                while (reader.Read())
                {
                    reader.GetValues(values);
                    csv.Write(values);
                }
            }
        }

        public static void ExportToCsv(IDataReader reader
            , Stream stream
            , bool includeHeaders = true
            , Encoding encoding = null
            , int bufferSize = Defaults.FileBufferSize)
            => UseContextIfNotExists(ctx => ctx.WriteToCsv(reader, stream, includeHeaders, encoding, bufferSize));

        public void WriteToCsv(IDataReader reader
            , Stream stream
            , bool includeHeaders = true
            , Encoding encoding = null
            , int bufferSize = Defaults.FileBufferSize)
        {
            encoding = encoding ?? Defaults.Encoding;

            using (var writer = new StreamWriter(stream, encoding, bufferSize, true))
            {
                WriteToCsv(reader, writer, includeHeaders);
            }
        }

        public static void ExportToCsv(IDataReader reader
            , StringBuilder sb
            , bool includeHeaders = true)
            => UseContextIfNotExists(ctx => ctx.WriteToCsv(reader, sb, includeHeaders));

        public void WriteToCsv(IDataReader reader
            , StringBuilder sb
            , bool includeHeaders = true)
        {
            using (var writer = new StringWriter(sb))
            {
                WriteToCsv(reader, writer, includeHeaders);
            }
        }
    }
}
