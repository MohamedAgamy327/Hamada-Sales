using System;
using System.Globalization;
using System.Windows.Data;
using Sales.Services;
namespace Sales.Helpers
{
    class RequiredCategoryConverter : IValueConverter
    {
        CategoryServices categoryServ = new CategoryServices();
        public static readonly IValueConverter Instance = new RequiredCategoryConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;

            var category = categoryServ.GetCategory(id);

            if ( category.Qty >category.RequestLimit)
                return 1;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
