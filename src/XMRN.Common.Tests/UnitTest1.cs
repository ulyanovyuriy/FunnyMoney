using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XMRN.Common.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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
