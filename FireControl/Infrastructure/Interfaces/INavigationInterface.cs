using System;
using FireControl.Commands;

namespace FireControl.Infrastructure.Interfaces
{
    internal interface INavigationInterface
    {
        event EventHandler NavigationRequired;

        void Invoke(ViewContext viewContext, bool asDialog);

        void Navigate();
    }
}
