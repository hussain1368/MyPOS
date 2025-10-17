using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace POS.WPF.Converters
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str)) return value;

            str = Regex.Replace(str, @"[^0-9.]", "");
            var parts = str.Split(".");
            if (int.TryParse(parts[0], out var number))
            {
                var suffix = "";
                if (str.Contains(".")) suffix = $".{parts[1].Substring(0, Math.Min(3, parts[1].Length))}";
                return $"{number.ToString("#,0")}{suffix}";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString().Replace(",", "");
            }
            return value;
        }
    }
}
