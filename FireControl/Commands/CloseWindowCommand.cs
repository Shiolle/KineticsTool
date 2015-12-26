using System.Windows;

namespace FireControl.Commands
{
    /// <summary>
    /// Performs an action and closes the window.
    /// </summary>
    internal class CloseWindowCommand : SimpleCommand
    {
        public override void Execute(object parameter)
        {
            base.Execute(null);
            ((Window)parameter).Close();
        }
    }
}
