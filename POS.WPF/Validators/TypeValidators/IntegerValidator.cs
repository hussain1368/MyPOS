using System.Globalization;
using System.Windows.Controls;

namespace POS.WPF.Validators.TypeValidators
{
    public class IntegerValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isInteger = true;
            if (value != null)
            {
                isInteger = int.TryParse(value.ToString().Replace(",", ""), out _);
            }
            return new ValidationResult(isInteger, "Please enter a valid integer");
        }
    }
}
