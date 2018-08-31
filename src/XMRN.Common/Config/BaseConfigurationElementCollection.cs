using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using XMRN.Common.System;

namespace XMRN.Common.Config
{
    public abstract class BaseConfigurationElementCollection<TItem>
        : ConfigurationElementCollection, IEnumerable<TItem>
        where TItem : BaseConfigurationElement, new()
    {
        public TItem this[int index] { get { return (TItem)base.BaseGet(index); } }

        public new TItem this[string key] { get { return (TItem)base.BaseGet(key); } }

        protected override object GetElementKey(ConfigurationElement element)
        {
            Guard.ArgumentNotNull(element, nameof(element));

            var item = (BaseConfigurationElement)element;
            var key = item.GetElementKey();
            return key;
        }

        protected override ConfigurationElement CreateNewElement()
            => new TItem();

        #region IEnumerable Support

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
            => this.OfType<TItem>().GetEnumerator();

        #endregion
    }
}
