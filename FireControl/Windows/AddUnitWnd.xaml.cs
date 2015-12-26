using System.Windows;
using FireControl.ViewModels.UnitControl;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for AddUnitWnd.xaml
    /// </summary>
    public partial class AddUnitWnd : Window
    {
        public AddUnitWnd()
        {
            InitializeComponent();
        }

        internal AddUnitWnd(AddUnitViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
