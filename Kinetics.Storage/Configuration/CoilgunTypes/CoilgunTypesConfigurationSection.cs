using System.Configuration;

namespace Kinetics.Storage.Configuration.CoilgunTypes
{
    internal class CoilgunTypesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", DefaultValue = null, IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(CoilgunTypeConfigurationCollection), AddItemName = "coilgunType", ClearItemsName = "clear", RemoveItemName = "remove")]
        public CoilgunTypeConfigurationCollection CoilgunTypes
        {
            get
            {
                return (CoilgunTypeConfigurationCollection)this[string.Empty];
            }
        }
    }
}
