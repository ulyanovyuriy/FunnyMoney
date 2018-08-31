using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace XMRN.Common.Config
{
    public abstract class BaseConfigurationElement : ConfigurationElement
    {
        public object GetValue(string name)
            => this[name];

        public override string ToString()
        {
            return this.ToDisplayText();
        }
    }
}
