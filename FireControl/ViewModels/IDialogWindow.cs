using System;
using FireControl.Infrastructure;
using FireControl.Infrastructure.Interfaces;

namespace FireControl.ViewModels
{
    /// <summary>
    /// An interface for a view model of a control that creates or edits and object and returns it to another control.
    /// The two controls never reference each other directly, and interact through the display server and a command.
    /// </summary>
    /// <typeparam name="T">The type of an object this control manipulates.</typeparam>
    internal interface IDialogWindowViewModel<out T> : INavigationNode
    {
        /// <summary>
        /// Gets if the edit is confirmed or cancelled.
        /// </summary>
        bool IsConfirmed { get; }

        /// <summary>
        /// Gets the edit result.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Should be invoked as the edit is complete and 
        /// </summary>
        event Action EditFinished;
    }
}
