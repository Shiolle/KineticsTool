using Kinetics.Core.Infrastructure;
using Kinetics.Core.Interfaces.Infrastructure;

namespace Kinetics.Core
{
    /// <summary>
    /// Provides external access to all utilities, calculators and such.
    /// </summary>
    public class ServiceFactory
    {
        private static CompositionRoot _compositionRoot;

        public static IServiceLibrary Library
        {
            get
            {
                if (_compositionRoot == null)
                {
                    _compositionRoot = new CompositionRoot(null);
                }

                return _compositionRoot.ServiceLibrary;
            }
        }
    }
}
