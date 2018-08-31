using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using XMRN.Common.Config;

namespace FunnyMoney.SBER.Config
{
    public class SberSmsPropertyMap : BaseConfigurationElement
    {
        [ConfigurationProperty(nameof(Name), IsKey = true, IsRequired = true)]
        public string Name { get => this.GetValue<string>(nameof(Name)); }

        [ConfigurationProperty(nameof(Token), IsRequired = true)]
        public string Token { get => this.GetValue<string>(nameof(Token)); }
    }

    [ConfigurationCollection(typeof(SberSmsPropertyMap)
            , AddItemName = "Map"
            , CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class SberSmsPropertyMaps : BaseConfigurationElementCollection<SberSmsPropertyMap>
    {
    }
}
