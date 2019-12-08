using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SaleViews
{
    public partial class SaleOfferDisplayUserControl : UserControl
    {
        public SaleOfferDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SaleOfferDisplay");
        }
    }
}
