using System;
using System.Globalization;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class RadioToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || value == null) return false;
            var _value = Enum.Parse(value.GetType(), parameter.ToString());
            return _value.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return null;
            return Enum.Parse(targetType, parameter.ToString());
        }
    }
}
