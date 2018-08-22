using System;
using System.Collections.Generic;
using System.Data;
using XMRN.Phone.Sms;

namespace XMRN.Android.Common.Sms
{
    public class DSmsExtractor : SmsExtractor
    {
        private Func<IDataReader> _readerFactory;

        public DSmsExtractor(Func<IDataReader> readerFactory)
        {
            _readerFactory = readerFactory ?? throw new ArgumentNullException(nameof(readerFactory));
        }

        public Func<IDataReader> ReaderFactory => _readerFactory;

        public override IEnumerable<SmsMessage> Extract()
        {
            var reader = ReaderFactory();
            try
            {
                while (reader.Read())
                {
                    var msg = new SmsMessage();
                    //msg.Id = int.Parse(reader["_id"]);

                    yield return msg;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }
    }
}