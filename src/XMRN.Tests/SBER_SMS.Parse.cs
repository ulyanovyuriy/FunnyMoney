﻿using FunnyMoney.SBER.Sms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.Semantic.Regexp;

namespace XMRN.Tests
{
    public partial class SBER_SMS
    {
        [TestMethod]
        public void SBER_SMS_PARSE_WRITE_OFF()
        {
            var rp = SberSmsMessage.Parser;

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

        [TestMethod]
        public void SBER_SMS_PARSE_BUY()
        {
            var rp = SberSmsMessage.Parser;

            string operation = @"VISA8842 20.08.18 10:40 покупка 121.65р YM*AliExpress Баланс: 217461.29р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "20.08.18 10:40");
            Assert.AreEqual(token["V"], "121.65р");
            Assert.AreEqual(token["T"], "YM*AliExpress");
            Assert.AreEqual(token["B"], "217461.29р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_ATM()
        {
            var rp = SberSmsMessage.Parser;

            string operation = @"VISA8842 14.08.18 19:38 выдача 9000р ATM 60003570 Баланс: 227928.97р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "14.08.18 19:38");
            Assert.AreEqual(token["V"], "9000р");
            Assert.AreEqual(token["T"], "ATM 60003570");
            Assert.AreEqual(token["B"], "227928.97р");

            operation = @"VISA8842 20.12.17 12:11 выдача 4000р с комиссией 100р Malaya Semenovskaya Баланс: 290212.14р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "20.12.17 12:11");
            Assert.AreEqual(token["V"], "4000р");
            Assert.AreEqual(token["C"], "100р");
            Assert.AreEqual(token["T"], "Malaya Semenovskaya");
            Assert.AreEqual(token["B"], "290212.14р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_SALARY()
        {
            var rp = SberSmsMessage.Parser;

            string operation = @"VISA8842 20.07.18 12:13 зачисление зарплаты 12000р Баланс: 420791.38р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "20.07.18 12:13");
            Assert.AreEqual(token["V"], "12000р");
            Assert.AreEqual(token["B"], "420791.38р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_PAY_IN()
        {
            var rp = SberSmsMessage.Parser;

            string operation = @"VISA8842 24.02.18 11:55 зачисление 5000р Баланс: 173573.89р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "24.02.18 11:55");
            Assert.AreEqual(token["V"], "5000р");
            Assert.AreEqual(token["B"], "173573.89р");

            operation = @"VISA8842 11.02.18 16:13 зачисление 50000р ATM 014708 Баланс: 179373.89р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "11.02.18 16:13");
            Assert.AreEqual(token["V"], "50000р");
            Assert.AreEqual(token["T"], "ATM 014708");
            Assert.AreEqual(token["B"], "179373.89р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_PAY_OUT()
        {
            var rp = SberSmsMessage.Parser;

            string operation = @"VISA8842 08.08.18 оплата Мобильного банка за 08/08/2018-07/09/2018 60р Баланс: 246609.56р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "08.08.18");
            Assert.AreEqual(token["V"], "60р");
            Assert.AreEqual(token["T"], "Мобильного банка за 08/08/2018-07/09/2018");
            Assert.AreEqual(token["B"], "246609.56р");

            //VISA8842 02.08.18 15:03 оплата 540р Yota TOPUP 6983 Баланс: 236402.35р
        }
    }
}
