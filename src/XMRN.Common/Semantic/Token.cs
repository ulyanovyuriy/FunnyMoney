using System;
using System.Collections.Generic;
using System.Linq;

namespace XMRN.Common.Semantic
{
    public interface IToken
    {
        string Name { get; set; }

        string Value { get; set; }

        IToken Parent { get; set; }

        IEnumerable<IToken> GetChilds();

        void Add(IToken child);

        void AddRange(IEnumerable<IToken> childs);
    }

    public abstract class Token : IToken
    {
        protected Token() { }

        protected Token(string name, string value) : this()
        {
            Name = name;
            Value = value;
        }

        #region IToken Support

        string IToken.Name { get { return Name; } set { Name = value; } }

        string IToken.Value { get { return Value; } set { Value = value; } }

        IToken IToken.Parent { get { return ParentToken; } set { ParentToken = (Token)value; } }

        IEnumerable<IToken> IToken.GetChilds()
        {
            return GetChildsCore();
        }

        void IToken.Add(IToken child)
        {
            AddCore((Token)child);
        }

        void IToken.AddRange(IEnumerable<IToken> childs)
        {
            foreach (var child in childs)
                AddCore((Token)child);
        }

        #endregion

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual string this[string name]
        {
            get
            {
                var token = GetChilds().FirstOrDefault(c => c.Name == name);

                if (token == null)
                    return null;

                return token.Value;
            }
        }

        public Token Parent { get { return ParentToken; } set { ParentToken = value; } }

        public IEnumerable<Token> GetChilds() => GetChildsCore();

        public void Add(Token token) => AddCore(token);

        public void AddRange(IEnumerable<Token> childs) => AddRange(childs);

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region Abstracts

        protected abstract Token ParentToken { get; set; }

        protected abstract IEnumerable<Token> GetChildsCore();

        protected abstract void AddCore(Token child);

        #endregion
    }
}
