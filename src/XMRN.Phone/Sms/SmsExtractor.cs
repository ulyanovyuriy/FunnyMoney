using System.Collections.Generic;

namespace XMRN.Phone.Sms
{
    public abstract class SmsExtractor
    {
        public abstract IEnumerable<SmsMessage> Extract();
    }
}
