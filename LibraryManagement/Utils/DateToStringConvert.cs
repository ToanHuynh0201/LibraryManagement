using System;
using System.Globalization;
using System.Windows.Data;

namespace LibraryManagement.Utils
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
                return date.ToString("dd/MM/yyyy");
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParseExact(value as string, "dd/MM/yyyy", culture, DateTimeStyles.None, out DateTime result))
                return result;
            return DateTime.Now;
        }
    }
}
