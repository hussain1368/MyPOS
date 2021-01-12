using System.Globalization;
using System.Windows.Controls;

namespace POS.WPF.Validators
{
    public class IntegerValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int output;
            var result = int.TryParse(value as string, out output);
            return new ValidationResult(result, "Please enter a valid number");
        }
    }
}
