using System;
using System.Windows.Data;
using FireControl.ViewModels.Avid;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.ValueConverters.Avid
{
    [ValueConversion(typeof(AvidVectorViewModel), typeof(double))]
    internal class AvidWindowToCanvasCoordinate : IValueConverter
    {
        private readonly IAvidCalculator _avidCalculator;

        public AvidWindowToCanvasCoordinate()
        {
            _avidCalculator = ServiceFactory.Library.AvidCalculator;
        }

        public int RingRadius { get; set; }

        public int CanvasSize { get; set; }

        public int ElementRadius { get; set; }

        public ConversionAxis Axis { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var window = value as AvidWindow;

            if (window != null)
            {
                return CalculateCoordinate(window);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double CalculateCoordinate(AvidWindow window)
        {
            double angle = _avidCalculator.DirectionToAngle(window.Direction);
            int length = RingRadius * GetAvidRingProjectionNumber(window.Ring);
            double coordinate = Axis == ConversionAxis.AxisY ?
                              CanvasSize / 2d - (length * Math.Sin(angle)) :
                              CanvasSize / 2d + (length * Math.Cos(angle));

            return coordinate - ElementRadius / 2d;
        }

        private int GetAvidRingProjectionNumber(AvidRing ring)
        {
            return 4 - (byte)ring;
        }
    }
}
