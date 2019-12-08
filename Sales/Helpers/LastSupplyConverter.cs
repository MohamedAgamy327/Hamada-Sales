using Sales.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Sales.Helpers
{
    public class LastSupplyConverter : IValueConverter
    {
        SupplyServices supplyServ = new SupplyServices();

        public static readonly IValueConverter Instance = new LastSupplyConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int bindingValue = (int)value;

            return supplyServ.IsLastSupply(bindingValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
