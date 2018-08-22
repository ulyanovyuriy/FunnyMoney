using System;

namespace XMRN.Phone.Sms
{
    public class SmsMessage
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public DateTime SentDate { get; set; }

        public string Body { get; set; }
    }
}
