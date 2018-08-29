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

        void AddRange(IEnumerable<IToken> childs);
    }

    public abstract class Token : IToken
    {
        private readonly List<Token> _childs;

        public Token(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }

        public Token Parent { get => GetParentCore(); internal set => SetParentCore(value); }

        string IToken.Name => Name;

        string IToken.Value => Value;

        IToken IToken.Parent => Parent;

        public IEnumerable<Token> GetChilds() => GetChildsCore();

        IEnumerable<IToken> IToken.GetChilds()
        {
            return GetChildsCore();
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        protected virtual void AddChildsCore(IEnumerable<Token> childs)
        {
            if (childs == null) return;
            foreach (var child in childs)
            {
                if (child.Parent != null) throw new ArgumentException(nameof(child.Parent));
                child.Parent = this;

                _childs.Add(child);
            }
        }

        protected virtual IEnumerable<Token> GetChildsCore() => _childs;

        protected abstract Token GetParentCore();

        protected abstract void SetParentCore(Token value);

        void IToken.AddRange(IEnumerable<IToken> childs)
        {
            throw new NotImplementedException();
        }
    }
}
