using System.Configuration;

namespace Radyn.Congress.Settings
{
    public class CongressSetting : ConfigurationSection
    {
        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }
    }

    public static class CongressConfiguration
    {
        private static CongressSetting Setting
        {
            get
            {
                var smtpSection = (CongressSetting)ConfigurationManager.GetSection("radyn/congress");
                return smtpSection;
            }
        }

        public static string Key
        {
            get { return Setting.Key; }
        }
    }
}
