using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels.DataContexts;

namespace FireControl.ViewModels.Counterfire
{
    /// <summary>
    /// This view model does not react to any changes in unit lists and positions.
    /// </summary>
    internal class CounterfireSetupViewModel : ViewModelBase, INavigationNode
    {
        private readonly List<CounterfireImpulseViewModel> _impulseTrack;
        private readonly List<CounterfireUnitViewModel> _units;

        public string Tag { get; private set; }

        public IEnumerable<CounterfireImpulseViewModel> ImpulseTrack
        {
            get { return _impulseTrack; }
        }

        public IEnumerable<CounterfireUnitViewModel> Units
        {
            get { return _units; }
        }

        public void Initialize(object dataContext)
        {
            var context = VerifyContext<CounterfireDataContext>(dataContext);

            if (context != null)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
