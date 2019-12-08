using System;
using System.Globalization;
using System.Windows.Data;

namespace Sales.Helpers
{
    public class CheckUrlConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new GreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string bindingValue = value == null ? string.Empty : value.ToString();

            if (string.IsNullOrEmpty(bindingValue))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
