using System;
using System.Text.RegularExpressions;

namespace XMRN.Common.Semantic.Regexp
{
    public class RegexTokenParser : TokenParser
    {
        public RegexTokenParser(string name, Regex regex)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Regex = regex ?? throw new ArgumentNullException(nameof(regex));
        }

        public string Name { get; }

        public Regex Regex { get; }

        public new RegexToken Parse(string text)
        {
            var match = Regex.Match(text);
            if (match == null) return null;

            return new RegexToken(Name, match);
        }

        protected override Token ParseToken(string text)
        {
            return Parse(text);
        }
    }
}
