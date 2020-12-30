using System;
using System.Globalization;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var _value = System.Text.RegularExpressions.Regex.Match(value as string, @"\d+").Value;
            if (string.IsNullOrWhiteSpace(_value)) return null;
            return _value;
        }
    }
}
