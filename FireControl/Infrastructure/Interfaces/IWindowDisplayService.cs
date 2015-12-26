using System.Windows;
using FireControl.ViewModels;

namespace FireControl.Infrastructure.Interfaces
{
    internal interface IWindowDisplayService
    {
        T GetViewModel<T>() where T : ViewModelBase;

        void ShowWindow(string typeName, ViewModelBase viewModel, bool asDialog, Window parent);

        IDialogWindowViewModel<T> ShowDialog<T>(string typeName, ViewModelBase viewModel, Window parent);
    }
}
