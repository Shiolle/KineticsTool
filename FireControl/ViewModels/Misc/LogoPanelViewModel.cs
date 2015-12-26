using FireControl.Infrastructure.Interfaces;

namespace FireControl.ViewModels.Misc
{
    internal class LogoPanelViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public LogoPanelViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void ShowDisclaimerBox()
        {
            _navigationService.ShowDisclaimerBox();
        }
    }
}
