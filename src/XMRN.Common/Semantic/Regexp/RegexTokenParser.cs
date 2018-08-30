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
            var matches = Regex.Matches(text);
            if (matches == null || matches.Count < 1)
                yield break;

            foreach (Match match in matches)
            {
                if (match.Success == false) continue;

                var root = new RegexToken(Name, match.Value);

                if (Groups != null)
                {
                    foreach (var groupName in Groups)
                    {
                        var group = match.Groups[groupName];
                        if (group != null && group.Success)
                        {
                            var child = new RegexToken(groupName, group.Value);
                            root.Add(child);
                        }
                    }
                }
                yield return root;
            }
        }

        protected override IEnumerable<Token> ParseCore(string text)
        {
            return Parse(text);
        }
    }
}
