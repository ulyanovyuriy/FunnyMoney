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
            //var msgs = ReadMessages();

            ////VISA8842 08.08.18 07:59 списание 100000р SBERBANK ONL@IN KARTA-VKLAD Баланс: 246671.36р
            ////VISA8842 14.08.18 11:44 списание 400р Баланс: 236930.77р
            ////VISA8842 03.06.18 21:00 списание 2000р с комиссией 20р Баланс: 292685.16р
            ////VISA8842 08.03.18 06:27 списание 40613.37р в счет погашения кредита Баланс: 93204.52р


            //var p = new Parser();
            //p.Register(new RegexTokenParser("CARDNUMBER", new Regex(@"VISA\d{4}")));
            //p.Register(new RegexTokenParser("BALANCE", new Regex(@"Баланс: (?<BALANCE>\d+(\.\d{1,2})?)р")));
            //p.Register(new RegexTokenParser("BUY", new Regex(@"покупка (?<BUY>\d+(\.\d{1,2})?)р")));
            //p.Register(new RegexTokenParser("WRITEOFF", new Regex(@"списание (?<WRITEOFF>\d+(\.\d{1,2})?)р")));
            //p.Register(new RegexTokenParser("WRITEOFF_COMMISION", new Regex(@"списание \d+(\.\d{1,2})?р с комиссией (?<WRITEOFF_COMMISION>\d+(\.\d{1,2})?)р")));
            //p.Register(new RegexTokenParser("WRITEOFF_TARGET", new Regex(@"списание \d+(\.\d{1,2})?р( с комиссией \d+(\.\d{1,2})?р)? (?<WRITEOFF_TARGET>.*) Баланс:\s")));
            //p.Register(new RegexTokenParser("ATM", new Regex(@"выдача (?<ATM>\d+(\.\d{1,2})?)р")));
            ////p.Register(new RegexTokenParser("TARGET", new Regex(@"(\d+(\.\d{1,2})?)р\s(?<TARGET>.*) Баланс:\s")));
            //p.Register(new RegexTokenParser("DATETIME", new Regex(@"\d{2}\.\d{2}\.\d{2}\s\d{2}\:\d{2}")));


            //var rs = new List<ParsedMessage>();

            //foreach (var msg in msgs)
            //{
            //    var pm = new ParsedMessage(msg);

            //    var tokens = p.Render(msg.Body).ToArray();
            //    foreach (var token in tokens)
            //    {
            //        if (token.Name == "CARDNUMBER")
            //            pm.CARNUMBER = token.Value;
            //        else if (token.Name == "BALANCE")
            //            pm.BALANCE = token.Value;
            //        else if (token.Name == "BUY")
            //            pm.BUY = token.Value;
            //        else if (token.Name == "WRITEOFF")
            //            pm.WRITEOFF = token.Value;
            //        else if (token.Name == "WRITEOFF_COMMISION")
            //            pm.WRITEOFF_COMMISION = token.Value;
            //        else if (token.Name == "WRITEOFF_TARGET")
            //            pm.WRITEOFF_TARGET = token.Value;
            //        else if (token.Name == "ATM")
            //            pm.ATM = token.Value;
            //        else if (token.Name == "DATETIME")
            //            pm.DATETIME = token.Value;
            //    }

            //    rs.Add(pm);
            //}

            //var b = new StringBuilder();
            //rs.AsDataReader().ExportToCsv(b);

            //var t = b.ToString();
        }

       

        private class ParsedMessage
        {
            private SmsMessage _msg;

            public ParsedMessage(SmsMessage msg)
            {
                _msg = msg;
                Body = msg.Body.Replace("\r\n", "|").Replace("\r", "|").Replace("\n", "|");
            }

            public int Id => _msg.Id;

            public string CARNUMBER;
            public string BALANCE;
            public string BUY;
            public string WRITEOFF;
            public string WRITEOFF_COMMISION;
            public string WRITEOFF_TARGET;
            public string ATM;
            public string DATETIME;
            public string Body;

        }
    }
}
