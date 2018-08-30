using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.Semantic;
using XMRN.Common.Semantic.Regexp;
using XMRN.Phone.Sms;

namespace FunnyMoney.SBER.Sms
{
    public class SberSmsMessage : SmsMessage
    {
        public const string WRITE_OFF_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) списание (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string BUY_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) покупка (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string ATM_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) выдача (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string SALARY_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) зачисление зарплаты (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_IN_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) зачисление (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_OUT_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2}) оплата?(\s+(?<T>.*)\s+)?(?<V>\d+(\.\d{2})?р) Баланс: (?<B>\d+(\.\d{2})?р)";

        public const string WRITE_OFF_TN = "WRITE_OFF";
        public const string BUY_TN = "BUY";
        public const string ATM_TN = "ATM";
        public const string SALARY_TN = "SALARY";
        public const string PAY_IN_TN = "PAY_IN";
        public const string PAY_OUT_TN = "PAY_OUT";

        public static Parser Parser;

        static SberSmsMessage()
        {
            Parser = new Parser();
            Parser.Register(new RegexTokenParser(WRITE_OFF_TN, new Regex(WRITE_OFF_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(BUY_TN, new Regex(BUY_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(ATM_TN, new Regex(ATM_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SALARY_TN, new Regex(SALARY_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(PAY_IN_TN, new Regex(PAY_IN_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(PAY_OUT_TN, new Regex(PAY_OUT_RX)
                , "CN", "DT", "V", "C", "T", "B"));
        }

        private List<IToken> _tokens;

        private IToken _writeOff;

        private IToken _buy;

        private IToken _atm;

        private IToken _salary;

        private IToken _payIn;

        private IToken _payOut;

        public SberSmsMessage(SmsMessage msg) : base(msg)
        {
            _tokens = Parser.Parse(Body).ToList();

            _writeOff = _tokens.FirstOrDefault(t => t.Name == WRITE_OFF_TN);
            _buy = _tokens.FirstOrDefault(t => t.Name == BUY_TN);
            _atm = _tokens.FirstOrDefault(t => t.Name == ATM_TN);
            _salary = _tokens.FirstOrDefault(t => t.Name == SALARY_TN);
            _payIn = _tokens.FirstOrDefault(t => t.Name == PAY_IN_TN);
            _payOut = _tokens.FirstOrDefault(t => t.Name == PAY_OUT_TN);
        }

        private IToken GetToken()
        {
            if (Type == SberSmsMessageType.WriteOff)
                return _writeOff;
            if (Type == SberSmsMessageType.Atm)
                return _atm;
            if (Type == SberSmsMessageType.Buy)
                return _buy;
            if (Type == SberSmsMessageType.Salary)
                return _salary;
            if (Type == SberSmsMessageType.PayIn)
                return _payIn;
            if (Type == SberSmsMessageType.PayOut)
                return _payOut;

            return null;
        }

        public SberSmsMessageType Type
        {
            get
            {
                if (_writeOff != null)
                    return SberSmsMessageType.WriteOff;
                if (_atm != null)
                    return SberSmsMessageType.Atm;
                if (_buy != null)
                    return SberSmsMessageType.Buy;
                if (_payOut != null)
                    return SberSmsMessageType.PayOut;
                if (_salary != null)
                    return SberSmsMessageType.Salary;
                if (_payIn != null)
                    return SberSmsMessageType.PayIn;
                else
                    return SberSmsMessageType.None;
            }
        }

        public string CardNumber
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["CN"];

                return null;
            }
        }

        public string OperationTime
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["DT"];

                return null;
            }
        }

        public string Value
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["V"];

                return null;
            }
        }

        public string Commission
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["C"];

                return null;
            }
        }

        public string Target
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["T"];

                return null;
            }
        }

        public string Balance
        {
            get
            {
                var token = GetToken();
                if (token != null)
                    return token["B"];

                return null;
            }
        }
    }
}
