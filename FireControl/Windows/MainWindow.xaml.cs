using System.Windows;
using FireControl.ViewModels;
using FireControl.ViewModels.Windows;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal MainWindow(MainWindowViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
