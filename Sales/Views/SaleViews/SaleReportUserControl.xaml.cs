using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SaleViews
{
    public partial class SaleReportUserControl : UserControl
    {
        public SaleReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SaleReport");
        }
    }
}
