using System;
using System.Collections.Generic;

namespace XMRN.Common.Semantic
{
    public interface IToken
    {
        string Name { get; }

        string Value { get; }

        IToken Parent { get; }

        IEnumerable<IToken> GetChilds();

        void Add(IToken child);

        void AddRange(IEnumerable<IToken> childs);
    }

    public abstract class Token : IToken
    {
        public Token(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #region IToken Support

        string IToken.Name => Name;

        string IToken.Value => Value;

        IToken IToken.Parent => Parent;

        IEnumerable<IToken> IToken.GetChilds()
        {
            return GetChildsCore();
        }

        void IToken.Add(IToken child)
        {
            AddCore(child);
        }

        void IToken.AddRange(IEnumerable<IToken> childs)
        {
            foreach (var child in childs)
                AddCore(child);
        }

        #endregion

        public string Name { get; set; }

        public string Value { get; set; }

        public Token Parent { get => GetParentCore(); }

        public IEnumerable<Token> GetChilds() => GetChildsCore();

        public void Add(Token token) => AddCore(token);

        public void AddRange(IEnumerable<Token> childs) => AddRange(childs);

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        protected abstract void AddCore(IToken child);

        protected abstract IEnumerable<Token> GetChildsCore();

        protected abstract Token GetParentCore();
    }
}
