using System;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Implementation.LaunchBoard;
using FireControl.Models.Implementation.ShellStars;
using FireControl.Models.Implementation.TimeControl;
using FireControl.Models.Implementation.UnitControl;
using FireControl.Models.Interfaces.LaunchBoard;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.TimeControl;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.ViewModels;
using FireControl.ViewModels.Avid;
using FireControl.ViewModels.LaunchBoard;
using FireControl.ViewModels.Misc;
using FireControl.ViewModels.Shellstar;
using FireControl.ViewModels.TimeControl;
using FireControl.ViewModels.UnitControl;
using FireControl.ViewModels.Windows;
using Kinetics.Storage;
using Microsoft.Practices.Unity;

namespace FireControl.Infrastructure.Implementation
{
    internal class FireControlUnityConfiguration : IFireControlContainer
    {
        private readonly IUnityContainer _container;

        public FireControlUnityConfiguration()
        {
            _container = new UnityContainer();
            RegisterInfrastructure();
            RegisterModels();
            RegisterViewModels();
        }

        public WindowDisplayService WindowDisplayService()
        {
            return _container.Resolve<WindowDisplayService>();
        }

        public T GetViewModel<T>() where T : ViewModelBase
        {
            return _container.Resolve<T>();
        }

        public ViewModelBase GetViewModel(Type viewModelType)
        {
            return _container.Resolve(viewModelType) as ViewModelBase;
        }

        private void RegisterModels()
        {
            _container.RegisterInstance<ICurrentTurnModel>(_container.Resolve<CurrentTurnModel>());
            _container.RegisterInstance<IUnitListModel>(_container.Resolve<UnitListModel>());
            _container.RegisterInstance<IUnitSetupModel>(_container.Resolve<UnitSetupModel>());

            _container.RegisterType<IUnitModel, UnitModel>();
            _container.RegisterType<ILaunchBoardModel, LaunchBoardModel>();
            _container.RegisterType<IWeaponSelectionModel, WeaponSelectionModel>();
            _container.RegisterType<IShellstarModel, ShellstarModel>();
        }

        private void RegisterViewModels()
        {
            _container.RegisterInstance(_container.Resolve<TurnControlViewModel>());
            _container.RegisterInstance(_container.Resolve<MainWindowViewModel>());
            _container.RegisterInstance(_container.Resolve<UnitListViewModel>());
            _container.RegisterType<AddUnitViewModel>();
            _container.RegisterType<VectorsControlViewModel>();
            _container.RegisterType<PositionControlViewModel>();
            _container.RegisterType<SelectedUnitViewModel>();
            _container.RegisterType<LogoPanelViewModel>();

            _container.RegisterType<LaunchPrerequisitesViewModel>();
            _container.RegisterType<WeaponSelectionViewModel>();
            _container.RegisterType<ShotGeometryTableViewModel>();
            _container.RegisterType<RoCWorksheetViewModel>();
            _container.RegisterType<MpatViewModel>();
            _container.RegisterType<MissileAccelerationViewModel>();
            _container.RegisterType<ShellstarInfoViewModel>();
            _container.RegisterType<ShellstarListViewModel>();
            _container.RegisterType<ShellstarDetailsViewModel>();
            _container.RegisterType<LaunchWindowControlViewModel>();
        }

        private void RegisterInfrastructure()
        {
            _container.RegisterInstance(new WindowDisplayService(this));
            _container.RegisterInstance<IStorageController>(new StorageController());
            _container.RegisterInstance<IStaticConfigurationController>(new StaticConfigurationController());
            _container.RegisterInstance<INavigationService>(_container.Resolve<NavigationService>());
        }
    }
}
