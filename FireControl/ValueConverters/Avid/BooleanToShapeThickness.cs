using System;
using System.Windows.Data;

namespace FireControl.ValueConverters.Avid
{
    /// <summary>
    /// This converter allows vanishing borders without hiding everything in them.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(int))]
    internal class BooleanToShapeThickness : IValueConverter
    {
        public BooleanToShapeThickness()
        {
            DefaultThickness = 2;
        }

        public int DefaultThickness { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value ? DefaultThickness : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
