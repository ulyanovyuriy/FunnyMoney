using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using XMRN.Common.Collections;

namespace XMRN.Common.IO
{
    public class CsvWriter : IDisposable
    {
        public const string DOUBLE_QUOTE = "\"\"";

        public const char QUOTE_CHAR = '"';

        public const string COLUMN_SEPARATOR = ",";

        public const int MAX_TEXT_LEN = 30000;

        public const int MAX_BINARY_LEN = 64;

        private TextWriter _writer;

        private bool _leaveOpen;

        public CsvWriter(TextWriter writer, bool leaveOpen = true)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _leaveOpen = leaveOpen;
        }

        public TextWriter Writer
        {
            get
            {
                if (disposedValue) throw new ObjectDisposedException(nameof(Writer));
                return _writer;
            }
        }

        private string QuoteText(string value)
        {
            if (value == null) return DOUBLE_QUOTE;

            if (value.Length > MAX_TEXT_LEN)
                value = value.Substring(0, MAX_TEXT_LEN);

            return QUOTE_CHAR + value.Replace(QUOTE_CHAR.ToString(), DOUBLE_QUOTE) + QUOTE_CHAR;
        }

        private string ToText(object value)
        {
            string text = null;
            if (value == null || value == DBNull.Value)
                text = null;
            else if (value is byte[])
                text = ((byte[])value).ToHex(0, MAX_BINARY_LEN);
            else
                text = value.ToString();

            return text;
        }

        public void Write(IEnumerable<object> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var textValues = values.Select(v => ToText(v)).ToArray();
            Write(textValues);
        }

        public void Write(IEnumerable<string> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var csvValues = values.Select(v => QuoteText(v)).ToArray();
            var csvText = string.Join(COLUMN_SEPARATOR, csvValues);

            Writer.WriteLine(csvText);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if (_leaveOpen == false)
                    _writer.Close();

                _writer = null;

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
