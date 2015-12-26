using System.Globalization;
using System.Windows.Controls;

namespace FireControl.Validation
{
    public class EmptyStringValidation : ValidationRule
    {
        public string ErrorMsg { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, ErrorMsg);
            }

            return new ValidationResult(true, null);
        }
    }
}
