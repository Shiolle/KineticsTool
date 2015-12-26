using System;
using System.Globalization;
using System.Windows.Controls;
using Kinetics.Core.Data.HexGrid;

namespace FireControl.Validation
{
    internal class HexCoordinatesValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var coordinateString = value as string;

            if (string.IsNullOrEmpty(coordinateString))
            {
                return new ValidationResult(false, "Coordinates cannot be empty.");
            }

            try
            {
                HexGridCoordinate.Parse(coordinateString);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Coordinates must follow <CF> <DA>:<Altitude> format.");
            }

            return new ValidationResult(true, null);
        }
    }
}
