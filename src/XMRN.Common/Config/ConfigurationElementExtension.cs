using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using XMRN.Common.System;

namespace XMRN.Common.Config
{
    public static class ConfigurationElementExtension
    {
        public static string ToDisplayText(this ConfigurationElement element)
        {
            element.ArgumentNotNull(nameof(element));

            var values = element.ElementInformation.Properties
                .OfType<PropertyInformation>()
                .Where(p => (p.Value is ConfigurationElement) == false)
                .Select(p => $"{p.Name}: \"{p.Value}\"");

            return string.Join("; ", values);
        }

        public static string GetElementKey(this ConfigurationElement element)
        {
            Guard.ArgumentNotNull(element, nameof(element));

            var keys = element.ElementInformation.Properties
                .OfType<PropertyInformation>()
                .Where(p => p.IsKey)
                .Select(p => Convert.ToString(p.Value))
                .ToArray();

            return string.Join(", ", keys);
        }

        public static T GetValue<T>(this BaseConfigurationElement element, string name)
        {
            return ConvertValue<T>(element, element.GetValue(name), name);
        }

        public static T GetValue<T>(this BaseConfigurationSection section, string name)
        {
            return ConvertValue<T>(section, section.GetValue(name), name);
        }

        private static T ConvertValue<T>(ConfigurationElement element, object v, string name)
        {
            try
            {
                var r = (T)v;
                return (T)r;
            }
            catch (InvalidCastException e)
            {
                e.Data.Add("PropertyName", name);
                e.Data.Add("ConfigurationElementName", element.GetType().Name);
                e.Data.Add("CastType", typeof(T).Name);
                e.Data.Add("ValueType", v?.GetType().Name);
                e.Data.Add("SettingValue", v);
                throw;
            }
        }
    }
}
