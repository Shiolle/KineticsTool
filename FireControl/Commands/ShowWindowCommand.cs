using System.Windows;
using FireControl.Infrastructure;
using FireControl.Infrastructure.Interfaces;

namespace FireControl.Commands
{
    internal delegate Window GetWindowDelegate();

    /// <summary>
    /// Shows a window, new or existing with an owner. Allows setting owner through markup.
    /// This, with the combination of WindowDisplayService allows view models to launch windows
    /// with owners other than the main window and without explicitly creating them.
    /// </summary>
    internal class ShowWindowCommand : SimpleCommand
    {
        private INavigationInterface _navigationRequest;

        public bool AsDialog { get; set; }

        public string ViewType { get; set; }

        public override void Execute(object parameter)
        {
            var parentWindow = parameter as Window;
            var viewContext = new ViewContext
            {
                ParentWindow = parentWindow,
                ViewTypeName = ViewType
            };

            _navigationRequest.Invoke(viewContext, AsDialog);
        }

        protected override void TargetChanged()
        {
            _navigationRequest = Target as INavigationInterface;
            if (_navigationRequest == null)
            {
                IsReady = false;
            }
        }
    }
}
