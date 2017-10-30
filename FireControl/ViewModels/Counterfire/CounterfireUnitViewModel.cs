using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireControl.ViewModels.Counterfire
{
    /// <summary>
    /// This unit view model substitutes position for distance from target and does not have any actions.
    /// </summary>
    internal class CounterfireUnitViewModel
    {

        public string UnitName
        {
            get { throw new NotImplementedException(); }
        }

        public int DistanceFromTarget
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsTarget
        {
            get { throw new NotImplementedException(); }
        }
    }
}
