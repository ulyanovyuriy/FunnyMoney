using System;
using System.Collections.Generic;
using System.Data;
using XMRN.Common.Data;
using XMRN.Phone.Sms;
using XMRN.Common.System;

namespace XMRN.Android.Common.Sms
{
    public class DSmsExtractor : SmsExtractor
    {
        public DSmsExtractor(Func<IDataReader> readerFactory)
        {
            ReaderFactory = readerFactory ?? throw new ArgumentNullException(nameof(readerFactory));
        }

        public Func<IDataReader> ReaderFactory { get; }

        public override IEnumerable<SmsMessage> Extract()
        {
            var reader = ReaderFactory();
            try
            {
                while (reader.Read())
                {
                    var msg = new SmsMessage();
                    msg.Id = reader.GetString("_id").ParseToInt32();
                    msg.Address = reader.GetString("address");
                    msg.Body = reader.GetString("body");
                    msg.Date = reader.GetString("date").ParseToDouble().AsMilliseconds().FromUnix();
                    msg.SentDate = reader.GetString("date_sent").ParseToDouble().AsMilliseconds().FromUnix();

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