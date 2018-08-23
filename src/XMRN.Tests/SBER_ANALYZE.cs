using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XMRN.Common.Collections;

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
        }
    }
}
