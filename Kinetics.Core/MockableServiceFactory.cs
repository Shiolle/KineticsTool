using Kinetics.Core.Infrastructure;
using Kinetics.Core.Interfaces.Infrastructure;
using Microsoft.Practices.Unity;

namespace Kinetics.Core
{
    /// <summary>
    /// A service factory that you can initialize with your own dependencies.
    /// </summary>
    public class MockableServiceFactory
    {
        private readonly CompositionRoot _compositionRoot;

        public MockableServiceFactory(IUnityContainer container)
        {
            _compositionRoot = new CompositionRoot(container);
        }

        public IServiceLibrary Library
        {
            get
            {
                return _compositionRoot.ServiceLibrary;
            }
        }
    }
}
