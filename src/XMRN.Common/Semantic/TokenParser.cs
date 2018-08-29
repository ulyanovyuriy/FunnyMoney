using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.Semantic
{
    public interface ITokenParser
    {
        IEnumerable<IToken> Parse(string text);
    }

    public abstract class TokenParser : ITokenParser
    {
        public IEnumerable<Token> Parse(string text)
        {
            return ParseCore(text);
        }

        protected abstract IEnumerable<Token> ParseCore(string text);

        IEnumerable<IToken> ITokenParser.Parse(string text)
        {
            return ParseCore(text);
        }
    }
}
