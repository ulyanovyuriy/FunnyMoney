using System.Collections.Generic;

namespace XMRN.Phone.Sms
{
    public abstract class SmsExtractor
    {
        public static SmsExtractor Current;

        public abstract IEnumerable<SmsMessage> Extract();
    }
}
