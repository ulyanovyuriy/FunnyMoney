using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.Semantic
{
    public class Parser
    {
        private readonly List<TokenParser> _parsers = new List<TokenParser>();

        public void Register(TokenParser parser)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            _parsers.Add(parser);
        }

        public IEnumerable<Token> Render(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            foreach (var parser in _parsers)
            {
                var token = parser.Parse(text);
                if (token != null)
                    yield return token;
            }
        }
    }
}
