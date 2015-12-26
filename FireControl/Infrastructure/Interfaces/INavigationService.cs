using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.Avid;

namespace FireControl.Infrastructure.Interfaces
{
    internal delegate object GetDataContextDelegate();

    internal delegate void ProcessInteractionResultDelegate<in T>(T result);

    internal interface INavigationService
    {
        INavigationInterface GetAddUnitRequest();

        INavigationInterface GetLaunchBoardRequest(GetDataContextDelegate acquireDataContext);

        INavigationInterface GetShellstarDetailsRequest(GetDataContextDelegate acquireDataContext);

        INavigationInterface GetShellstarListRequest(GetDataContextDelegate acquireDataContext);

        INavigationInterface GetAvidWindowSelectionRequest(GetDataContextDelegate acquireDataContext, ProcessInteractionResultDelegate<AvidWindow> processResult);

        void ShowDisclaimerBox();

        string GetSavePath();

        string GetLoadPath();

        AvidWindow SelectWindow(WindowSelectionContext context);
    }
}
