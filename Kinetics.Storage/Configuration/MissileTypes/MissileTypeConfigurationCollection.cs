using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Kinetics.Storage.Configuration.MissileTypes
{
    internal class MissileTypeConfigurationCollection : ConfigurationElementCollection, IEnumerable<MissileTypeConfigurationElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MissileTypeConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MissileTypeConfigurationElement)element).TypeName;
        }

        public new IEnumerator<MissileTypeConfigurationElement> GetEnumerator()
        {
            return this.OfType<MissileTypeConfigurationElement>().GetEnumerator();
        }
    }
}
