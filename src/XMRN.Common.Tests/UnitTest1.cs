using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XMRN.Common.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static SymmetricAlgorithm _cryptoService = new TripleDESCryptoServiceProvider();
        // maybe use AesCryptoServiceProvider instead?

        // vector and key have to match between encryption and decryption
        public static string Encrypt(string text, byte[] key, byte[] vector)
        {
            return Transform(text, _cryptoService.CreateEncryptor(key, vector));
        }

        // vector and key have to match between encryption and decryption
        public static string Decrypt(string text, byte[] key, byte[] vector)
        {
            return Transform(text, _cryptoService.CreateDecryptor(key, vector));
        }

        private static string Transform(string text, ICryptoTransform cryptoTransform)
        {
            MemoryStream stream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);

            byte[] input = Encoding.Default.GetBytes(text);

            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.Default.GetString(stream.ToArray());
        }


        [TestMethod]
        public void DataReader_Crypto()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            var enc = Encrypt(File.ReadAllText(@"C:\Users\ulyanov\Desktop\my sms.txt"), key, vector);
            File.WriteAllText(@"C:\Users\ulyanov\Desktop\my sms1.txt", enc);

            //crypto.CreateEncryptor(Encoding.UTF8.GetBytes("12345"), )
        }

        [TestMethod]
        [DeploymentItem("my sms1.txt")]
        public void DataReader_Decrypt()
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] vector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            var enc = File.ReadAllText(@"my sms1.txt");
            var d = Decrypt(enc, key, vector);

            var xmlData = Encoding.UTF8.GetBytes(d);

            var ms = new MemoryStream(xmlData);

            var dt = new DataTable();
            dt.ReadXml(ms);
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
