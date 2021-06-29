using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HandyCalendar.Code
{
    public sealed class CalendarYearMonthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            long ticks = long.MaxValue;
            foreach (var value in values ?? Enumerable.Empty<object>())
            {
                if (value is DateTime dt) ticks = dt.Ticks;
            }
            if (ticks == long.MaxValue) return null;

            try
            {
                return PersianCalendarHelper.ToCurrentCultureString(new DateTime(ticks), "yyyy MMM");
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(ticks).ToString("Y", CultureInfo.InvariantCulture);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
