using System.Windows;
using FireControl.ViewModels.Shellstar;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for AssignedShellstars.xaml
    /// </summary>
    public partial class AssignedShellstars : Window
    {
        public AssignedShellstars()
        {
            InitializeComponent();
        }

        internal AssignedShellstars(ShellstarListViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
