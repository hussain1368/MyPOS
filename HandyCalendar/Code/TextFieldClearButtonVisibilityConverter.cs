﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HandyCalendar.Code
{
    public class TextFieldClearButtonVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)values[0]) // TextFieldAssist.HasClearButton
                return Visibility.Collapsed;

            return (bool)values[1] // Hint.IsContentNullOrEmpty
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}