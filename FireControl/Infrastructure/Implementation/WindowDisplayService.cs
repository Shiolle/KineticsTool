using System;
using System.Windows;
using FireControl.Infrastructure.Interfaces;
using FireControl.ViewModels;
using FireControl.ViewModels.Windows;
using FireControl.Windows;
using Microsoft.Win32;

namespace FireControl.Infrastructure.Implementation
{
    internal class WindowDisplayService : IWindowDisplayService
    {
        private const string UnitListFileFilter = "Situation reports (*.SRP;*.XML)|*.SRP;*.XML";
        private const string UnitListDefaultExtension = "srp";

        private readonly IFireControlContainer _fireControlContainer;

        private MainWindow _mainWindow;

        public WindowDisplayService(IFireControlContainer fireControlContainer)
        {
            _fireControlContainer = fireControlContainer;
        }

        public void ShowMainWindow()
        {
            _mainWindow = new MainWindow(_fireControlContainer.GetViewModel<MainWindowViewModel>());
            _mainWindow.Show();
        }

        public void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowError(Exception exception)
        {
            ShowMessageBox(exception.Message);
        }

        public void ShowWindow(string typeName, ViewModelBase viewModel, bool asDialog, Window parent)
        {
            var window = GetWindow(typeName, viewModel, parent);
            if (asDialog)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public IDialogWindowViewModel<T> ShowDialog<T>(string typeName, ViewModelBase viewModel, Window parent)
        {
            var dialogViewModel = viewModel as IDialogWindowViewModel<T>;

            if (dialogViewModel == null)
            {
                throw new ArgumentException(string.Format("View model of type {0} is not a dialog view model.", viewModel.GetType()));
            }

            var window = GetWindow(typeName, viewModel, parent);
            window.ShowDialog();
            return dialogViewModel;
        }

        public T GetViewModel<T>() where T : ViewModelBase
        {
            return _fireControlContainer.GetViewModel<T>();
        }

        private Window GetWindow(string typeName, ViewModelBase viewModel, Window parent)
        {
            var type = Type.GetType(typeName, true, true);
            var window = Activator.CreateInstance(type) as Window;

            if (window == null)
            {
                throw new ArgumentException(string.Format("Type '{0}' does is not a window.", typeName));
            }

            window.DataContext = viewModel;
            window.Owner = parent ?? _mainWindow;
            return window;
        }

        public void ShowDisclaimerBox()
        {
            var disclaimerBox = new DisclaimerBox
            {
                Owner = _mainWindow
            };
            disclaimerBox.ShowDialog();
        }

        public string GetSavePath()
        {
            var saveListDialog = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = UnitListFileFilter,
                FilterIndex = 0,
                AddExtension = true,
                DefaultExt = UnitListDefaultExtension
            };

            bool? result = saveListDialog.ShowDialog();

            return result == true ? saveListDialog.FileName : string.Empty;
        }

        public string GetLoadPath()
        {
            var openListDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = UnitListFileFilter,
                FilterIndex = 0,
                Multiselect = false
            };

            bool? result = openListDialog.ShowDialog();

            return result == true ? openListDialog.FileName : string.Empty;
        }
    }
}
