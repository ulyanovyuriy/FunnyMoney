using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMRN.Common.Security.Cryptography;

namespace XMRN.Common.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SMS_Encrypt()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            using (var ctx = new CryptoContext())
            {
                var result = CryptoContext.EncryptByTripleDES(
                    File.ReadAllBytes(@"C:\Users\ulyanov\Desktop\my sms.txt")
                    , key, vector);

                File.WriteAllBytes(@"C:\Users\ulyanov\Desktop\my sms1.txt", result);
            }
        }

        [TestMethod]
        [DeploymentItem("my sms1.txt")]
        public void SMS_Decrypt()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            using (var ctx = new CryptoContext())
            {
                var data = File.ReadAllBytes(@"my sms1.txt");
                var result = CryptoContext.DecryptByTripleDES(data, key, vector);

                var ms = new MemoryStream(result);
                var dt = new DataTable();
                dt.ReadXml(ms);
            }
        }

        [TestMethod]
        public void DataReader_GetSchemaTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("name", typeof(string));

            var r = dt.CreateDataReader();

            var st = r.GetSchemaTable();
            st.WriteXml(@"C:\Users\ulyanov\Desktop\cdc схемы\dtschema.xml", XmlWriteMode.WriteSchema);

            var sql = new SqlConnection(@"Data Source=zsql;Initial Catalog=CensorDb_ulyanov;Persist Security Info=True;User ID=sa;Password=Qwerty11;Pooling=True;");
            sql.Open();
            var cmd = sql.CreateCommand();

            cmd.CommandText = "select 1";

            var sr = cmd.ExecuteReader();

            var sst = sr.GetSchemaTable();

            sst.WriteXml(@"C:\Users\ulyanov\Desktop\cdc схемы\sqlschema.xml", XmlWriteMode.WriteSchema);
        }
    }
}
