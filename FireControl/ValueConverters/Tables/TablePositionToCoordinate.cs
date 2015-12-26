using System;
using System.Windows.Data;

namespace FireControl.ValueConverters.Tables
{
    /// <summary>
    /// This converter allows to turn the number of row/column in a table into canvas coordinate.
    /// </summary>
    [ValueConversion(typeof(int), typeof(double))]
    public class TablePositionToCoordinate : IValueConverter
    {
        public TablePositionToCoordinate()
        {
            Direction = 1;
        }

        /// <summary>
        /// Gets or sets the value that is added to the initial value.
        /// </summary>
        public int PositionShift { get; set; }

        /// <summary>
        /// Gets or sets the width or height of a table row or column correspondingly.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Gets or sets the value that is added to the resulting coordinate.
        /// </summary>
        public double CoordinateShift { get; set; }

        /// <summary>
        /// Gets or sets the value that specifies direction. Sohuld be either -1 or 1.
        /// </summary>
        public int Direction { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CoordinateShift + Step * (Direction * (int)value + PositionShift);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
