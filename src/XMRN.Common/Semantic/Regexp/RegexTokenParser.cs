using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XMRN.Common.System;

namespace XMRN.Common.Semantic.Regexp
{
    public class RegexTokenParser : TokenParser
    {
        public RegexTokenParser(string name, Regex regex, params string[] groups)
        {
            Name = Guard.ArgumentNotNull(name, nameof(name));
            Regex = Guard.ArgumentNotNull(regex, nameof(regex));
            Groups = groups;
        }

        public string Name { get; }

        public Regex Regex { get; }

        public string[] Groups { get; }

        public new IEnumerable<RegexToken> Parse(string text)
        {
            var match = Regex.Match(text);
            if (match == null || match.Success == false)
                yield break;

            var parent = new RegexToken(Name, match.Value);
            yield return parent;

            if (Groups != null)
            {
                foreach (var groupName in Groups)
                {
                    var group = match.Groups[groupName];
                    if (group != null && group.Success)
                        yield return new RegexToken(groupName, group.Value, parent);
                }
            }
        }

        protected override IEnumerable<Token> ParseCore(string text)
        {
            return Parse(text);
        }
    }
}
