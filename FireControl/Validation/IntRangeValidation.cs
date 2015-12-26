using System;
using System.Windows.Controls;

namespace FireControl.Validation
{
    /// <summary>
    /// Validates that the value is integer and inside specified bounds.
    /// </summary>
    public class IntRangeValidation : ValidationRule
    {
        public string Message { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                int intValue = Convert.ToInt32(value);

                if (intValue >= Min && intValue <= Max)
                {
                    return new ValidationResult(true, null);
                }

                return new ValidationResult(false, Message);
            }
            catch (Exception)
            {
                return new ValidationResult(false, Message);
            }
        }
    }
}
