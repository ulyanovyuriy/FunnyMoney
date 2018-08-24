using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using System.Linq;
using XMRN.Common.Collections;
using XMRN.Common.Data;
using XMRN.Phone.Sms;

namespace XMRN.Tests
{
    [TestClass]
    public class SBER_ANALYZE
    {
        [TestMethod]
        [DeploymentItem("900.ed")]
        public void SBER_READ_SMS()
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
        }
    }
}
