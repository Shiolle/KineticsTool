using System;
using System.Windows;
using System.Windows.Data;

namespace FireControl.ValueConverters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    internal class HideZerosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value != 0) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
