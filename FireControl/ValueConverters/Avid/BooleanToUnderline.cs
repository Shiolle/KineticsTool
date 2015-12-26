using System;
using System.Windows;
using System.Windows.Data;

namespace FireControl.ValueConverters.Avid
{
    [ValueConversion(typeof(bool), typeof(TextDecorations))]
    internal class BooleanToUnderline : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? TextDecorations.Underline : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
