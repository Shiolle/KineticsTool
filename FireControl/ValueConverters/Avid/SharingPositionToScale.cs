using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FireControl.ValueConverters.Avid
{
    [ValueConversion(typeof(int), typeof(double))]
    internal class SharingPositionToScale : Freezable, IValueConverter
    {
        public static readonly DependencyProperty ScalingFactorProperty =
            DependencyProperty.Register("ScalingFactor", typeof(double), typeof(SharingPositionToScale));

        public double ScalingFactor
        {
            get { return (double)GetValue(ScalingFactorProperty); }
            set { SetValue(ScalingFactorProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SharingPositionToScale();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sharingPosition = (int)value;
            return sharingPosition < 0 ? 1.0 : 1 / ScalingFactor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
