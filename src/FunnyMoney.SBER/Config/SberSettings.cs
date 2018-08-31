using System.Configuration;
using XMRN.Common.Config;

namespace FunnyMoney.SBER.Config
{
    public class SberSettings : BaseConfigurationSection<SberSettings>
    {
        [ConfigurationProperty(nameof(SmsTemplates), IsDefaultCollection = true)]
        public SberSmsTemplates SmsTemplates { get => this.GetValue<SberSmsTemplates>(nameof(SmsTemplates)); }

        [ConfigurationProperty(nameof(SmsPropertyMaps), IsDefaultCollection = false)]
        public SberSmsPropertyMaps SmsPropertyMaps { get => this.GetValue<SberSmsPropertyMaps>(nameof(SmsPropertyMaps)); }
    }
}
