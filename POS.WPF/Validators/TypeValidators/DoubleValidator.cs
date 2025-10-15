using System.Globalization;
using System.Windows.Controls;

namespace POS.WPF.Validators.TypeValidators
{
    public class DoubleValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isDouble = true;
            if (value != null)
            {
                //isDouble = double.TryParse(value.ToString().Replace(",", ""), out _);
                isDouble = double.TryParse(value as string, out _);
            }
            return new ValidationResult(isDouble, "Please enter a valid decimal");
        }
    }
}
