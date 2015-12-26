using FireControl.Commands;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels;

namespace FireControl.Infrastructure.Implementation
{
    internal class InteractionRequest<T, TR> : NavigationRequest<T>
        where T : ViewModelBase, INavigationNode, IDialogWindowViewModel<TR>
    {
        private readonly ProcessInteractionResultDelegate<TR> _processInteractionResult;

        public InteractionRequest(IWindowDisplayService displayService)
            :base(displayService, null)
        { }

        public InteractionRequest(
            IWindowDisplayService displayService,
            GetDataContextDelegate acquireDataContext,
            ProcessInteractionResultDelegate<TR> processInteractionResult)
            : base(displayService, acquireDataContext)
        {
            _processInteractionResult = processInteractionResult;
        }

        public override void Invoke(ViewContext viewContext, bool asDialog)
        {
            T viewModel = InitViewModel();
            WindowDisplayService.ShowDialog<TR>(viewContext.ViewTypeName, viewModel, viewContext.ParentWindow);

            if (viewModel.IsConfirmed)
            {
                _processInteractionResult(viewModel.Result);
            }
        }
    }
}
