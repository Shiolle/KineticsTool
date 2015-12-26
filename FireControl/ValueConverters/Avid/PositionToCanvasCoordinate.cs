using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FireControl.ViewModels.AvidElementBoard;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.ValueConverters.Avid
{
    [ValueConversion(typeof(AvidMarkViewModel), typeof(double))]
    internal class PositionToCanvasCoordinate : Freezable, IValueConverter
    {
        private readonly IAvidCalculator _avidCalculator;

        public PositionToCanvasCoordinate()
        {
            _avidCalculator = ServiceFactory.Library.AvidCalculator;
        }

        public static readonly DependencyProperty CanvasSizeProperty =
            DependencyProperty.Register("CanvasSize", typeof(double), typeof(PositionToCanvasCoordinate));

        public static readonly DependencyProperty RingWidthProperty =
            DependencyProperty.Register("RingWidth", typeof(double), typeof(PositionToCanvasCoordinate));

        public static readonly DependencyProperty ElementDiameterProperty =
            DependencyProperty.Register("ElementDiameter", typeof(double), typeof(PositionToCanvasCoordinate));

        public static readonly DependencyProperty ScalingFactorProperty =
            DependencyProperty.Register("ScalingFactor", typeof(double), typeof(PositionToCanvasCoordinate));

        public ConversionAxis Axis { get; set; }

        public double CanvasSize
        {
            get { return (double)GetValue(CanvasSizeProperty); }
            set { SetValue(CanvasSizeProperty, value);}
        }

        public double RingWidth
        {
            get { return (double)GetValue(RingWidthProperty); }
            set { SetValue(RingWidthProperty, value); }
        }

        public double ElementDiameter
        {
            get { return (double)GetValue(ElementDiameterProperty); }
            set { SetValue(ElementDiameterProperty, value); }
        }

        public double ScalingFactor
        {
            get { return (double)GetValue(ScalingFactorProperty); }
            set { SetValue(ScalingFactorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mark = value as AvidMarkViewModel;

            if (mark != null)
            {
                var centerCoordinate = CalculateWindowCenter(mark.Window);
                centerCoordinate += SharingPositionToCenterShift(mark.SharingPosition);
                return CalculateElementCorner(centerCoordinate, mark.SharingPosition >= 0);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double CalculateWindowCenter(AvidWindow window)
        {
            double angle = _avidCalculator.DirectionToAngle(window.Direction);
            var length = (int)(RingWidth * GetAvidRingProjectionNumber(window.Ring));
            double coordinate = Axis == ConversionAxis.AxisY ?
                              CanvasSize / 2d - (length * Math.Sin(angle)) :
                              CanvasSize / 2d + (length * Math.Cos(angle));

            return coordinate;
        }

        private double CalculateElementCorner(double elementCenter, bool scale)
        {
            double halfLength = ElementDiameter / 2d;

            if (scale)
            {
                halfLength = halfLength / ScalingFactor;
            }

            return elementCenter - halfLength;
        }

        private double SharingPositionToCenterShift(int sharingPosition)
        {
            if (sharingPosition < 0)
            {
                return 0;
            }

            int shiftDirection;
            if (Axis == ConversionAxis.AxisY)
            {
                // Positions 0 and 3 are above cell center; 1 and 2 are below.
                shiftDirection = sharingPosition % 3 == 0 ? -1 : 1;
            }
            else
            {
                // Positions 0 and 2 are to the left of cell center; 1 and 3 are to the right.
                shiftDirection = sharingPosition % 2 == 0 ? -1 : 1;
            }

            return ElementDiameter * shiftDirection * 0.375d / 2d;
        }

        private int GetAvidRingProjectionNumber(AvidRing ring)
        {
            return 4 - (byte)ring;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new PositionToCanvasCoordinate();
        }
    }
}
