using System.Configuration;

namespace Kinetics.Storage.Configuration.MissileTypes
{
    internal class MissileTypesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", DefaultValue = null, IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(MissileTypeConfigurationCollection), AddItemName = "missileType", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MissileTypeConfigurationCollection MissileTypes
        {
            get
            {
                return (MissileTypeConfigurationCollection)this[string.Empty];
            }
        }
    }
}
