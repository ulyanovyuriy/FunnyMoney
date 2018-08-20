using System;
using System.Text.RegularExpressions;

namespace XMRN.Common.Semantic.Regexp
{
    public class RegexToken : Token
    {
        public RegexToken(string name, Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Name = name;
        }

        public Match Match { get; }

        public override string Value
        {
            get
            {
                var group = Match.Groups[Name];
                if (group != null && group.Success)
                {
                    return group.Value;
                }
                return Match.Value;
            }
        }
    }
}
