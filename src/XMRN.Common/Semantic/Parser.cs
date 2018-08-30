using System;
using System.Collections.Generic;
using XMRN.Common.System;

namespace XMRN.Common.Semantic
{
    public sealed class Parser : ITokenParser
    {
        private readonly List<ITokenParser> _parsers = new List<ITokenParser>();

        public Parser(ParserOptions options)
        {
            Options = Guard.ArgumentNotNull(options, nameof(options));
        }

        public ParserOptions Options { get; }

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
                    bool matched = false;
                    foreach (var token in tokens)
                    {
                        matched = true;
                        yield return token;
                    }

                    if (matched && Options.BreakOnFirstMatch)
                        yield break;
                }
            }
        }
    }
}
