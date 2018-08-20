using System;
using System.Collections.Generic;
using System.Text;

namespace XMRN.Common.Semantic
{
    public interface ITokenParser
    {
        IToken Parse(string text);
    }

    public abstract class TokenParser : ITokenParser
    {
        public Token Parse(string text)
        {
            return ParseToken(text);
        }

        protected abstract Token ParseToken(string text);

        IToken ITokenParser.Parse(string text)
        {
            return ParseToken(text);
        }
    }
}
