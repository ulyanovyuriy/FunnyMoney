using FunnyMoney.SBER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.Config;
using XMRN.Common.Semantic;
using XMRN.Common.Semantic.Regexp;
using XMRN.Common.System;
using XMRN.Phone.Sms;

namespace FunnyMoney.SBER.Sms
{
    public class SberSms : SmsMessage
    {
        public const string WRITE_OFF_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) списание (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string BUY_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) покупка (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string ATM_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) выдача (наличных )?(?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string SALARY_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) зачисление зарплаты (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_IN_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) зачисление (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_OUT1_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2}) оплата(\s+ )?(\s+(?<T>.*)\s+)?(?<V>\d+(\.\d{2})?р) Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_OUT_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2}) оплата?(\s+(?<T>.*)\s+)?(?<V>\d+(\.\d{2})?р) Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string PAY_OUT2_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) оплата (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
        public const string BUY_CANCEL_RX = @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) отмена покупки (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";

        public static Parser Parser;

        public static Dictionary<string, SberSmsPropertyMap> Maps;

        public static string[] AllTokens;

        static SberSms()
        {
            Maps = SberSettings.Global.SmsPropertyMaps.ToDictionary();
            AllTokens = Maps.Select(x => x.Value.Token).Distinct().ToArray();

            Parser = new Parser(new ParserOptions()
            {
                BreakOnFirstMatch = true
            });

            Parser.Register(new RegexTokenParser(SberSmsType.WriteOff.ToString(), new Regex(WRITE_OFF_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.Buy.ToString(), new Regex(BUY_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.Atm.ToString(), new Regex(ATM_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.Salary.ToString(), new Regex(SALARY_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.PayIn.ToString(), new Regex(PAY_IN_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.PayOut.ToString(), new Regex(PAY_OUT_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.PayOut.ToString(), new Regex(PAY_OUT2_RX)
                , "CN", "DT", "V", "C", "T", "B"));
            Parser.Register(new RegexTokenParser(SberSmsType.BuyCancel.ToString(), new Regex(BUY_CANCEL_RX)
                , "CN", "DT", "V", "C", "T", "B"));

            SberSettings.Global.SmsTemplates
                .OrderBy(x => x.Id)
                .ForEach(t =>
                {
                    Parser.Register(new RegexTokenParser(t.Type.ToString(), new Regex(t.Regex), AllTokens));
                });
        }

        private IToken _token;

        public SberSms(SmsMessage msg) : base(msg)
        {
            var tokens = Parser.Parse(Body).ToList();
            _token = tokens.FirstOrDefault();
        }

        private IToken GetToken()
        {
            return _token;
        }

        private string GetValue(string propertyName)
        {
            var _token = GetToken();
            if (_token == null)
                return null;

            var map = Maps[propertyName];
            var value = _token[map.Token];
            return value;
        }

        public SberSmsType Type
        {
            get
            {
                var token = GetToken();
                if (token == null)
                    return SberSmsType.None;

                return token.Name.ParseTo<SberSmsType>();
            }
        }

        public string CardNumber => GetValue(nameof(CardNumber));

        public string OperationTime => GetValue(nameof(OperationTime));

        public string Value => GetValue(nameof(OperationTime));

        public string Commission => GetValue(nameof(Commission));

        public string Category => GetValue(nameof(Category));

        public string Target => GetValue(nameof(Target));

        public string Balance => GetValue(nameof(Balance));
    }
}
