using System;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels;

namespace FireControl.Infrastructure.Implementation
{
    internal class InternalInteractionRequest<T, TR> : NavigationRequest<T>
        where T : ViewModelBase, INavigationNode, IDialogWindowViewModel<TR>
    {
        private readonly string _viewType;

        public InternalInteractionRequest(IWindowDisplayService displayService, string viewTypeName)
            :base(displayService, null)
        {
            if (string.IsNullOrEmpty(viewTypeName))
            {
                throw new ArgumentNullException("viewTypeName");
            }

            _viewType = viewTypeName;
        }

        public TR Invoke(object dataContext)
        {
            T viewModel = InitViewModel(dataContext);
            WindowDisplayService.ShowDialog<TR>(_viewType, viewModel, null);

            return viewModel.IsConfirmed ? viewModel.Result : default(TR);
        }
    }
}
