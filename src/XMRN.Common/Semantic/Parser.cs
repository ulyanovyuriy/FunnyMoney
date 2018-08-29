using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.Semantic
{
    public class Parser : ITokenParser
    {
        private readonly List<ITokenParser> _parsers = new List<ITokenParser>();

        public void Register(ITokenParser parser)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            _parsers.Add(parser);
        }

        public IEnumerable<IToken> Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            foreach (var parser in _parsers)
            {
                var tokens = parser.Parse(text);
                if (tokens != null)
                {
                    foreach (var token in tokens)
                        yield return token;
                }
            }
        }
    }
}
