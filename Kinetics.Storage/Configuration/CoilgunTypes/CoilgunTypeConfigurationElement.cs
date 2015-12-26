using System.Configuration;

namespace Kinetics.Storage.Configuration.CoilgunTypes
{
    internal class CoilgunTypeConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty(PropertyName.WeaponCode, IsKey = true, IsRequired = true)]
        public string WeaponCode
        {
            get
            {
                return (string)this[PropertyName.WeaponCode];
            }
        }

        [ConfigurationProperty(PropertyName.MvMultiplyer, IsRequired = true)]
        public string MvMultiplyer
        {
            get
            {
                return (string)this[PropertyName.MvMultiplyer];
            }
        }

        private static class PropertyName
        {
            public const string WeaponCode = "weaponCode";
            public const string MvMultiplyer = "mvMultiplyer";
        }
    }
}
