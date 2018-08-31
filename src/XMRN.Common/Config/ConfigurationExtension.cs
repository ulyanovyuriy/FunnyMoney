using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using XMRN.Common.System;

namespace XMRN.Common.Config
{
    public static class ConfigurationExtension
    {
        public static T GetSection<T>(this Configuration config, string name = null)
            where T : ConfigurationSection
        {
            config = Guard.ArgumentNotNull(config, nameof(config));
            name = name.IfNullOrEmpty(typeof(T).Name);

            var section = config.GetSection(name);
            if (section == null)
                throw new ConfigurationErrorsException($"Configuration section {name} ({typeof(T).Name}) not found");

            return (T)section;
        }
    }
}
