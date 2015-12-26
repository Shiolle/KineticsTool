using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FireControl.Views.UnitControl
{
    /// <summary>
    /// Interaction logic for AddUnitControl.xaml
    /// </summary>
    public partial class AddUnitControl : UserControl
    {
        public AddUnitControl()
        {
            InitializeComponent();       
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(tbName);
        }
    }
}
