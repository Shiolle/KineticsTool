using System.Windows;
using FireControl.ViewModels.Misc;

namespace FireControl.Windows
{
    /// <summary>
    /// Interaction logic for AvidWindowSelection.xaml
    /// </summary>
    public partial class AvidWindowSelection : Window
    {
        public AvidWindowSelection()
        {
            InitializeComponent();
        }

        internal AvidWindowSelection(WindowSelectionViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
