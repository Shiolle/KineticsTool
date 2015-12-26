using System;
using FireControl.Commands;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels;

namespace FireControl.Infrastructure.Implementation
{
    internal class NavigationRequest<T> : INavigationInterface
        where T : ViewModelBase, INavigationNode
    {
        private readonly IWindowDisplayService _displayService;
        private readonly GetDataContextDelegate _acquireContextDelegate;

        public NavigationRequest(IWindowDisplayService displayService, GetDataContextDelegate acquireContext)
        {
            _displayService = displayService;
            _acquireContextDelegate = acquireContext;
        }

        public event EventHandler NavigationRequired;

        public virtual void Invoke(ViewContext viewContext, bool asDialog)
        {
            var viewModel = InitViewModel();
            WindowDisplayService.ShowWindow(viewContext.ViewTypeName, viewModel, asDialog, viewContext.ParentWindow);
        }

        public void Navigate()
        {
            RequestNavigation();
        }

        protected IWindowDisplayService WindowDisplayService
        {
            get { return _displayService; }
        }

        protected T InitViewModel()
        {
            object dataContext = _acquireContextDelegate != null ? _acquireContextDelegate() : null;

            return InitViewModel(dataContext);
        }

        protected T InitViewModel(object dataContext)
        {
            var viewModel = _displayService.GetViewModel<T>();
            if (viewModel == null)
            {
                throw new ArgumentException(string.Format("View model '{0}' does not support navigation or doesn't exist.", typeof(T)));
            }

            viewModel.Initialize(dataContext);
            return viewModel;
        }

        private void RequestNavigation()
        {
            if (NavigationRequired != null)
            {
                NavigationRequired(this, new EventArgs());
            }
        }
    }
}
