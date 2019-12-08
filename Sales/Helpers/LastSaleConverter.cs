using System;
using System.Globalization;
using System.Windows.Data;
using Sales.Services;

namespace Sales.Helpers
{
    public class LastSaleConverter : IValueConverter
    {
        SaleServices saleServ = new SaleServices();

        public static readonly IValueConverter Instance = new LastSaleConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int bindingValue = (int)value;

            return saleServ.IsLastSale(bindingValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
