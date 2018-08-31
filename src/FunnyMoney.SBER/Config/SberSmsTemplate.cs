using FunnyMoney.SBER.Sms;
using System.Configuration;
using XMRN.Common.Config;

namespace FunnyMoney.SBER.Config
{
    public class SberSmsTemplate : BaseConfigurationElement
    {
        [ConfigurationProperty(nameof(Id), IsKey = true, IsRequired = true)]
        public int Id
        {
            get => this.GetValue<int>(nameof(Id));
        }

        [ConfigurationProperty(nameof(Type), IsRequired = true)]
        public SberSmsType Type
        {
            get => this.GetValue<SberSmsType>(nameof(Type));
        }

        [ConfigurationProperty(nameof(Regex), IsRequired = true)]
        public string Regex
        {
            get => this.GetValue<string>(nameof(Regex));
        }
    }

    [ConfigurationCollection(typeof(SberSmsTemplate)
            , AddItemName = "Template"
            , CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class SberSmsTemplates : BaseConfigurationElementCollection<SberSmsTemplate>
    {

    }
}
