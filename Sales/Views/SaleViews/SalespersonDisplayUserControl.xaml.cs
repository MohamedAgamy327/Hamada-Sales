using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SaleViews
{

    public partial class SalespersonDisplayUserControl : UserControl
    {
        public SalespersonDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SalespersonDisplay");
        }
    }
}
