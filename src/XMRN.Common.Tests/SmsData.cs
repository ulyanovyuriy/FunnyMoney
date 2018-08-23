using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using XMRN.Common.Compression;
using XMRN.Common.Data;
using XMRN.Common.IO;
using XMRN.Common.Security.Cryptography;
using XMRN.Common.Collections;

namespace XMRN.Common.Tests
{
    [TestClass]
    public class SmsData
    {
        [TestMethod]
        public void SMS_Encrypt()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            var data = File.ReadAllBytes(@"C:\Users\ulyanov\Desktop\my sms.txt");
            data = CompressionContext.CompressByGZ(data);
            data = CryptoContext.EncryptByTripleDES(data, key, vector);

            File.WriteAllBytes(@"C:\Users\ulyanov\Desktop\my sms1.txt", data);
        }

        [TestMethod]
        [DeploymentItem("my sms1.txt")]
        public void SMS_Decrypt()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            var data = File.ReadAllBytes(@"my sms1.txt");
            data = CryptoContext.DecryptByTripleDES(data, key, vector);
            data = CompressionContext.DecompressByGZ(data);

            var ms = new MemoryStream(data);
            var dt = new DataTable();
            dt.ReadXml(ms);

            var r = dt.CreateDataReader();
            var sb = new StringBuilder();
            IOContext.ExportToCsv(r, sb, true);
            var text = sb.ToString();

            var e = new XMRN.Android.Common.Sms.DSmsExtractor(() => dt.CreateDataReader());
            var msgs = e.Extract()
                .Where(m => m.Address == "900")
                .ToArray();

            var ssb = new StringBuilder();
            msgs.AsDataReader().ExportToCsv(ssb);
            var nt = ssb.ToString();

            msgs
                .AsDataReader()
                .AsDataTable()
                .ToArray()
                .WriteTo(@"C:\Users\ulyan\Documents\Graphics\900.ed");
        }


    }
}
