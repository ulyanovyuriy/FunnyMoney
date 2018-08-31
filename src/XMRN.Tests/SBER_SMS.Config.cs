using FunnyMoney.SBER.Config;
using FunnyMoney.SBER.Sms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.Semantic.Regexp;

namespace XMRN.Tests
{
    public partial class SBER_SMS
    {
        [TestMethod]
        public void SBER_SMS_Config()
        {
            var cfg = SberSettings.Global;
        }
    }
}
