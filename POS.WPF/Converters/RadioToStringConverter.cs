using System;
using System.Globalization;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class RadioToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null) return false;
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null) return false;
            return parameter.ToString();
        }
    }
}
