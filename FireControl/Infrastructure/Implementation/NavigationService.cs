using FireControl.Commands;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels;
using FireControl.ViewModels.DataContexts;
using FireControl.ViewModels.Misc;
using FireControl.ViewModels.Shellstar;
using FireControl.ViewModels.UnitControl;
using FireControl.ViewModels.Windows;
using Kinetics.Core.Data.Avid;

namespace FireControl.Infrastructure.Implementation
{
    internal class NavigationService : INavigationService
    {
        private readonly WindowDisplayService _displayService;

        public NavigationService(WindowDisplayService displayService)
        {
            _displayService = displayService;
        }

        public INavigationInterface GetAddUnitRequest()
        {
            return GetNavigationRequest<AddUnitViewModel>(null);
        }

        public INavigationInterface GetLaunchBoardRequest(GetDataContextDelegate acquireDataContext)
        {
            return GetNavigationRequest<LaunchBoardViewModel>(acquireDataContext);
        }

        public INavigationInterface GetShellstarDetailsRequest(GetDataContextDelegate acquireDataContext)
        {
            return GetNavigationRequest<ShellstarDetailsViewModel>(acquireDataContext);
        }

        public INavigationInterface GetShellstarListRequest(GetDataContextDelegate acquireDataContext)
        {
            return GetNavigationRequest<ShellstarListViewModel>(acquireDataContext);
        }

        public INavigationInterface GetAvidWindowSelectionRequest(GetDataContextDelegate acquireDataContext, ProcessInteractionResultDelegate<AvidWindow> processResult)
        {
            return GetInteractionRequest<WindowSelectionViewModel, AvidWindow>(acquireDataContext, processResult);
        }

        private INavigationInterface GetNavigationRequest<T>(GetDataContextDelegate acquireDataContext)
            where T : ViewModelBase, INavigationNode
        {
            return new NavigationRequest<T>(_displayService, acquireDataContext);
        }

        private INavigationInterface GetInteractionRequest<T, TR>(GetDataContextDelegate acquireDataContext, ProcessInteractionResultDelegate<TR> processResult)
            where T : ViewModelBase, INavigationNode, IDialogWindowViewModel<TR>
        {
            return new InteractionRequest<T, TR>(_displayService, acquireDataContext, processResult);
        }

        public void ShowDisclaimerBox()
        {
            _displayService.ShowDisclaimerBox();
        }

        public string GetSavePath()
        {
            return _displayService.GetSavePath();
        }

        public string GetLoadPath()
        {
            return _displayService.GetLoadPath();
        }

        public AvidWindow SelectWindow(WindowSelectionContext context)
        {
            var windowSelectionRequest = new InternalInteractionRequest<WindowSelectionViewModel, AvidWindow>(_displayService, "FireControl.Windows.AvidWindowSelection");
            return windowSelectionRequest.Invoke(context);
        }
    }
}
