namespace XMRN.Common.Semantic.Regexp
{
    public class RegexToken : Token
    {
        public RegexToken(string name, string value
            , RegexToken parent = null) : base(name, value)
        {
            Parent = parent;
        }

        public new RegexToken Parent { get; private set; }

        protected override Token GetParentCore()
        {
            return Parent;
        }

        protected override void SetParentCore(Token value)
        {
            Parent = (RegexToken)value;
        }
    }
}
