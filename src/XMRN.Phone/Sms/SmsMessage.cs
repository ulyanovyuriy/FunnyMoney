using System;
using XMRN.Common.System;

namespace XMRN.Phone.Sms
{
    public class SmsMessage
    {
        public SmsMessage()
        {
        }

        public SmsMessage(SmsMessage from)
        {
            from = Guard.ArgumentNotNull(from, nameof(from));

            Id = from.Id;
            Address = from.Address;
            Date = from.Date;
            SentDate = from.SentDate;
            Body = from.Body;
        }

        public int Id { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public DateTime SentDate { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            return $"id: {Id}, address: {Address}, date_sent: {SentDate}, body: {Body}";
        }
    }
}
