namespace XMRN.Common.Semantic
{
    public interface IToken
    {
        string Name { get; }

        string Value { get; }
    }

    public class Token : IToken
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
