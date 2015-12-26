using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Kinetics.Storage.Configuration.CoilgunTypes
{
    internal class CoilgunTypeConfigurationCollection : ConfigurationElementCollection, IEnumerable<CoilgunTypeConfigurationElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CoilgunTypeConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CoilgunTypeConfigurationElement)element).WeaponCode;
        }

        public new IEnumerator<CoilgunTypeConfigurationElement> GetEnumerator()
        {
            return this.OfType<CoilgunTypeConfigurationElement>().GetEnumerator();
        }
    }
}
