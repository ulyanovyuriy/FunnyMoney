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

            SberSettings.Global.SmsTemplates
                .OrderBy(x => x.Id)
                .ForEach(t =>
                {
                    Parser.Register(new RegexTokenParser(
                        t.Type, new Regex(t.Regex), AllTokens));
                });
        }

        private IToken _token;

        public SberSms(SmsMessage msg) : base(msg)
        {
            var tokens = Parser.Parse(Body).ToList();
            _token = tokens.FirstOrDefault();
        }

        private string GetValue(string propertyName)
        {
            if (_token == null)
                return null;

            var map = Maps[propertyName];
            var value = _token[map.Token];
            return value;
        }

        public string Type => _token?.Name;

        public string CardNumber => GetValue(nameof(CardNumber));

        public string OperationTime => GetValue(nameof(OperationTime));

        public string Value => GetValue(nameof(OperationTime));

        public string Commission => GetValue(nameof(Commission));

        public string Category => GetValue(nameof(Category));

        public string Target => GetValue(nameof(Target));

        public string Balance => GetValue(nameof(Balance));
    }
}
