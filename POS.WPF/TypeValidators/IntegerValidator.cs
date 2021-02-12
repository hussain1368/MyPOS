using System.Globalization;
using System.Windows.Controls;

namespace POS.WPF.TypeValidators
{
    public class IntegerValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isInteger = true;
            if (value != null)
            {
                int output;
                isInteger = int.TryParse(value.ToString().Replace(",", ""), out output);
            }
            return new ValidationResult(isInteger, "Please enter a valid number");
        }
    }
}
