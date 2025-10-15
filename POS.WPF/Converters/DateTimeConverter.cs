using System;
using System.Globalization;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) System.Diagnostics.Debug.WriteLine($"Convert:{value}:{value.GetType()}");
            if (value != null)
            {
                var _value = ((DateTime) value).ToString("yyyy-MM-dd");
                System.Diagnostics.Debug.WriteLine($"_value:{_value}");
                return _value;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) System.Diagnostics.Debug.WriteLine($"Convert:{value}:{value.GetType()}");
            return value;
        }
    }
}
