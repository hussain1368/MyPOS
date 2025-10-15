using System;
using System.Globalization;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(value!=null)System.Diagnostics.Debug.WriteLine($"Convert:{value}:{value.GetType()}");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(value!=null)System.Diagnostics.Debug.WriteLine($"ConvertBack:{value}:{value.GetType()}");

            //if (value == null) return null;
            //if (string.IsNullOrEmpty(value.ToString())) return null;
            //var _value = double.Parse(value.ToString());
            //System.Diagnostics.Debug.WriteLine($"ConvertBack:{_value}:{_value.GetType()}");
            //return _value;

            return value;
        }
    }
}
