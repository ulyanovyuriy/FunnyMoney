using System;
using System.Collections.Generic;
using System.Linq;
using XMRN.Common.System;

namespace XMRN.Common.Semantic.Regexp
{
    public class RegexToken : Token
    {
        private RegexToken _parent;

        private readonly HashSet<RegexToken> _childs = new HashSet<RegexToken>();

        public RegexToken() : base()
        {
        }

        public RegexToken(string name, string value) : base(name, value)
        {
        }

        #region Token Support

        protected override Token ParentToken { get => Parent; set => Parent = (RegexToken)value; }

        protected override void AddCore(Token child) => Add((RegexToken)child);

        protected override IEnumerable<Token> GetChildsCore() => GetChilds();

        #endregion

        public new RegexToken Parent
        {
            get => _parent;
            set
            {
                value = Guard.ArgumentNotNull(value, nameof(value));

                if (_parent != null) throw new Exception("Parent assigned");
                _parent = value;
                _parent._childs.Add(this);
            }
        }

        public new IEnumerable<RegexToken> GetChilds()
        {
            return _childs.AsEnumerable();
        }

        public void Add(RegexToken child)
        {
            child = Guard.ArgumentNotNull(child, nameof(child));
            child.Parent = this;
        }

        public void AddRange(IEnumerable<RegexToken> childs)
        {
            childs = Guard.ArgumentNotNull(childs, nameof(childs));
            childs.ForEach(Add);
        }
    }
}
