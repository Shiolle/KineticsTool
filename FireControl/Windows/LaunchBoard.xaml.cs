using System.Windows;
using FireControl.ViewModels.Windows;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for KineticsWorksheet.xaml
    /// </summary>
    public partial class LaunchBoard : Window
    {
        public LaunchBoard()
        {
            InitializeComponent();
        }

        internal LaunchBoard(LaunchBoardViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
