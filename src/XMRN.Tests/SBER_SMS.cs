using FunnyMoney.SBER.Sms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XMRN.Common.Collections;
using XMRN.Common.Data;
using XMRN.Common.Semantic;
using XMRN.Common.Semantic.Regexp;
using XMRN.Phone.Sms;

namespace XMRN.Tests
{
    [TestClass]
    public partial class SBER_SMS
    {
        private SmsMessage[] ReadMessages()
        {
            var f = "900.ed";
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            var data = File.ReadAllBytes(f);
            data = data.Decrypt(key, vector);
            data = data.Decompress();
            var dt = new DataTable().Load(data);

            var msgs = dt.CreateDataReader().AsEnumerable(r => new SmsMessage()
            {
                Address = r.GetString("Address"),
                Body = r.GetString("Body"),
                Date = r.GetDateTime("Date"),
                Id = r.GetInt32("Id"),
                SentDate = r.GetDateTime("SentDate"),
            }).ToArray();

            return msgs;
        }

        [TestMethod]
        [DeploymentItem("900.ed")]
        public void SBER_SMS_READ()
        {
            var msgs = ReadMessages();
        }

        [TestMethod]
        [DeploymentItem("900.ed")]
        public void SBER_SMS_PARSE()
        {
            var msgs = ReadMessages()
                .Select(m => new SberSms(m))
                .ToList();

            var b = new StringBuilder();
            msgs.Where(m => m.Type == SberSmsType.None)
                .AsDataReader(mb =>
                mb.AddMember(m => m.Id)
                .AddMember(m => m.Type)
                .AddMember(m => m.CardNumber)
                .AddMember(m => m.OperationTime)
                .AddMember(m => m.Value)
                .AddMember(m => m.Commission)
                .AddMember(m => m.Balance)
                .AddMember(m => m.Target)
                .AddMember(m => m.Body)
            ).ExportToCsv(b);


            var t = b.ToString();
        }
    }
}
