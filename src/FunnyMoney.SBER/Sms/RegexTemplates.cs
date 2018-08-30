using System;
using System.Collections.Generic;
using System.Text;

namespace FunnyMoney.SBER.Sms
{
    public static class RegexTemplates
    {
        public static string WriteOff =
            @"(?<CN>VISA\d{4}) (?<DT>\d{2}\.\d{2}\.\d{2} \d{2}:\d{2}) списание (?<V>\d+(\.\d{2})?р)( с комиссией (?<C>\d+(\.\d{2})?р))?(\s+(?<T>.*))? Баланс: (?<B>\d+(\.\d{2})?р)";
    }
}
