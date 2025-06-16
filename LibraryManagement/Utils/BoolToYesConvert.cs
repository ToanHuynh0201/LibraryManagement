using System;
using System.Globalization;
using System.Windows.Data;

namespace LibraryManagement.Utils
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Không";

            try
            {
                if (value is int intValue)
                    return intValue == 1 ? "Có" : "Không";

                if (value is bool boolValue)
                    return boolValue ? "Có" : "Không";

                if (int.TryParse(value.ToString(), out int parsed))
                    return parsed == 1 ? "Có" : "Không";
            }
            catch { }

            return "Không";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "Có" ? 1 : 0;
        }
    }
}
