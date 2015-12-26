using System.Windows;
using FireControl.ViewModels.Shellstar;
using FireControl.ViewModels.Windows;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for ShellstarDetails.xaml
    /// </summary>
    public partial class ShellstarDetails : Window
    {
        public ShellstarDetails()
        {
            InitializeComponent();
        }

        internal ShellstarDetails(ShellstarDetailsViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
