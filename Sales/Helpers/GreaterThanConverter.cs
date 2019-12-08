using System;
using System.Globalization;
using System.Windows.Data;

namespace Sales.Helpers
{
    public class GreaterThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new GreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal bindingValue = System.Convert.ToDecimal(value);

            if (bindingValue > 0)
                return 1;
            else if (bindingValue < 0)
                return -1;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
