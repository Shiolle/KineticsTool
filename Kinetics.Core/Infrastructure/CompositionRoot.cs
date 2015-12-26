using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.Infrastructure;
using Kinetics.Core.Interfaces.RefData;
using Kinetics.Core.Interfaces.Utility;
using Kinetics.Core.Logic.Calculators;
using Kinetics.Core.Logic.RefData;
using Kinetics.Core.Logic.Utility;
using Microsoft.Practices.Unity;

namespace Kinetics.Core.Infrastructure
{
    internal class CompositionRoot
    {
        private readonly IUnityContainer _container;
        private IServiceLibrary _serviceLibrary;

        public CompositionRoot(IUnityContainer container)
        {
            if (container != null)
            {
                _container = container;
            }
            else
            {
                _container = new UnityContainer();
            }

            RegisterUtility();
            RegisterCalculators();
            RegisterTableData();
            RegisterInfrastructure();
        }

        public IServiceLibrary ServiceLibrary
        {
            get
            {
                return _serviceLibrary ?? (_serviceLibrary = _container.Resolve<ServiceLibrary>());
            }
        }

        private void RegisterUtility()
        {
            _container.RegisterType<IVectorLibrary, VectorLibrary>();
        }

        private void RegisterInfrastructure()
        {
            _container.RegisterType<IServiceLibrary, ServiceLibrary>();
        }

        private void RegisterCalculators()
        {
            _container.RegisterType<IAvidCalculator, AvidCalculator>();
            _container.RegisterType<IHexGridCalculator, HexGridCalculator>();
            _container.RegisterType<IHexVectorUtility, HexVectorUtility>();
            _container.RegisterType<IFiringSolutionCalculator, FiringSolutionCalculator>();
            _container.RegisterType<IShellstarBuilder, ShellstarBuilder>();
            _container.RegisterType<IAvidPathfinder, AvidPathfinder>();
        }

        private void RegisterTableData()
        {
            _container.RegisterType<IRangeAltitudeTable, RangeAltitudeTable>();
            _container.RegisterType<IShotGeometryTable, ShotGeometryTable>();
            _container.RegisterType<IAvidModelBuilder, AvidModelBuilder>();
            _container.RegisterType<IMissilePositionAdjustmentTable, MissilePositionAdjustmentTable>();
            _container.RegisterType<IProjectileDamageTable, ProjectileDamageTable>();
            _container.RegisterType<IAvidModelBuilder, AvidModelBuilder>();
        }
    }
}