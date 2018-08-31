using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using XMRN.Common.System;

namespace XMRN.Common.Config
{
    public static class ConfigurationElementCollectionExtension
    {
        public static Dictionary<string, T> ToDictionary<T>(this BaseConfigurationElementCollection<T> items)
            where T : BaseConfigurationElement, new()
        {
            items.ArgumentNotNull(nameof(items));

            return items.ToDictionary(i => i.GetElementKey());
        }
    }
}
