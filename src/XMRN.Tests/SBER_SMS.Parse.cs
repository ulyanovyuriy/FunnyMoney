using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.Semantic.Regexp;

namespace XMRN.Tests
{
    public partial class SBER_SMS
    {
        [TestMethod]
        public void SBER_SMS_PARSE_WRITEOFF()
        {
            var rp = new RegexTokenParser("WRITEOFF"
                , new Regex(@"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) списание (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)")
                , "CN", "DT", "V", "C", "T", "B");


            string operation = @"VISA8842 08.08.18 07:59 списание 100000р SBERBANK ONL@IN KARTA-VKLAD Баланс: 246671.36р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "08.08.18 07:59");
            Assert.AreEqual(token["V"], "100000р");
            Assert.AreEqual(token["C"], null);
            Assert.AreEqual(token["T"], "SBERBANK ONL@IN KARTA-VKLAD");
            Assert.AreEqual(token["B"], "246671.36р");

            operation = @"VISA8842 14.08.18 11:44 списание 400р Баланс: 236930.77р";
            token = rp.Parse(operation).Single();
            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "14.08.18 11:44");
            Assert.AreEqual(token["V"], "400р");
            Assert.AreEqual(token["C"], null);
            Assert.AreEqual(token["T"], null);
            Assert.AreEqual(token["B"], "236930.77р");

            operation = @"VISA8842 03.06.18 21:00 списание 2000р с комиссией 20р Баланс: 292685.16р";
            token = rp.Parse(operation).Single();
            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "03.06.18 21:00");
            Assert.AreEqual(token["V"], "2000р");
            Assert.AreEqual(token["C"], "20р");
            Assert.AreEqual(token["T"], null);
            Assert.AreEqual(token["B"], "292685.16р");

            operation = @"VISA8842 08.03.18 06:27 списание 40613.37р в счет погашения кредита Баланс: 93204.52р";
            token = rp.Parse(operation).Single();
            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "08.03.18 06:27");
            Assert.AreEqual(token["V"], "40613.37р");
            Assert.AreEqual(token["C"], null);
            Assert.AreEqual(token["T"], "в счет погашения кредита");
            Assert.AreEqual(token["B"], "93204.52р");
        }
    }
}
