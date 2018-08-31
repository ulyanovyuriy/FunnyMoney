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
        public void SBER_SMS_PARSE_WRITE_OFF()
        {
            var rp = SberSms.Parser;

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
            var rp = SberSms.Parser;

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
            var rp = SberSms.Parser;

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

            operation = @"VISA8842 17.10.17 20:48 выдача наличных 4000р ATM 890426 Баланс: 226100.73р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "17.10.17 20:48");
            Assert.AreEqual(token["V"], "4000р");
            Assert.AreEqual(token["T"], "ATM 890426");
            Assert.AreEqual(token["B"], "226100.73р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_SALARY()
        {
            var rp = SberSms.Parser;

            string operation = @"VISA8842 20.07.18 12:13 зачисление зарплаты 12000р Баланс: 420791.38р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "20.07.18 12:13");
            Assert.AreEqual(token["V"], "12000р");
            Assert.AreEqual(token["B"], "420791.38р");

            operation = @"VISA8842 27.04.18 15:45 зачисление отпускных 92667.38р Баланс: 288067.53р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "27.04.18 15:45");
            Assert.AreEqual(token["V"], "92667.38р");
            Assert.AreEqual(token["B"], "288067.53р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_PAY_IN()
        {
            var rp = SberSms.Parser;

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
            var rp = SberSms.Parser;

            string operation = @"VISA8842 08.08.18 оплата Мобильного банка за 08/08/2018-07/09/2018 60р Баланс: 246609.56р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "08.08.18");
            Assert.AreEqual(token["CT"], "Мобильного банка");
            Assert.AreEqual(token["P"], "08/08/2018-07/09/2018");
            Assert.AreEqual(token["V"], "60р");
            Assert.AreEqual(token["B"], "246609.56р");

            operation = @"VISA8842 02.08.18 15:03 оплата 540р Yota TOPUP 6983 Баланс: 236402.35р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "02.08.18 15:03");
            Assert.AreEqual(token["V"], "540р");
            Assert.AreEqual(token["T"], "Yota TOPUP 6983");
            Assert.AreEqual(token["B"], "236402.35р");

            operation = @"VISA8842 23.10.17 21:04 оплата услуг 200р MTS OAO Баланс: 223512.88р";
            token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "23.10.17 21:04");
            Assert.AreEqual(token["CT"], "услуг");
            Assert.AreEqual(token["V"], "200р");
            Assert.AreEqual(token["T"], "MTS OAO");
            Assert.AreEqual(token["B"], "223512.88р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_BUY_CANCEL()
        {
            var rp = SberSms.Parser;

            string operation = @"VISA8842 16.10.17 13:17 отмена покупки 184р Баланс: 231222.12р";
            var token = rp.Parse(operation).Single();

            Assert.AreEqual(token["CN"], "VISA8842");
            Assert.AreEqual(token["DT"], "16.10.17 13:17");
            Assert.AreEqual(token["V"], "184р");
            Assert.AreEqual(token["B"], "231222.12р");
        }

        [TestMethod]
        public void SBER_SMS_PARSE_INFO()
        {
            var rp = SberSms.Parser;


            string operation = @"ЮРИЙ АЛЕКСАНДРОВИЧ, успешно подключен автоплатеж ""ЗАЩИЩЕННЫЙ ЗАЕМЩИК"" с карты VISA8842. Дата первого платежа - 13.03.18. Подробная информация об услуге sberbank.ru/autoinfo. Сбербанк";
            var token = rp.Parse(operation).Single();
        }
    }
}
