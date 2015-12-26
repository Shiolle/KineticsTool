using System;
using FireControl.ViewModels;

namespace FireControl.Infrastructure.Interfaces
{
    internal interface IFireControlContainer
    {
        T GetViewModel<T>() where T : ViewModelBase;

        ViewModelBase GetViewModel(Type viewModelType);
    }
}
