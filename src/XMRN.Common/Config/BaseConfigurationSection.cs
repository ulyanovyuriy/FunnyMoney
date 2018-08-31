using System.Configuration;
using System.IO;

namespace XMRN.Common.Config
{
    public class BaseConfigurationSection : ConfigurationSection
    {
        public object GetValue(string name)
            => this[name];
    }

    public class BaseConfigurationSection<TSection> : BaseConfigurationSection
        where TSection : BaseConfigurationSection<TSection>
    {
        public static TSection Global;

        static BaseConfigurationSection()
        {
            var location = Path.GetDirectoryName(typeof(TSection).Assembly.Location);
            var file = Path.Combine(location, typeof(TSection).Name + ".config");
            if (File.Exists(file))
            {
                var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = file
                }, ConfigurationUserLevel.None);

                Global = config.GetSection<TSection>();
            }
        }

        public override string ToString()
        {
            return this.ToDisplayText();
        }
    }
}
