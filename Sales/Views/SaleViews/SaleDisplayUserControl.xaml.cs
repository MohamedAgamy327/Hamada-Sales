using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SaleViews
{
    public partial class SaleDisplayUserControl : UserControl
    {
        public SaleDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SaleDisplay");
        }
    }
}
