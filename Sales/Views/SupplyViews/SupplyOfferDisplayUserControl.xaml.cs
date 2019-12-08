using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyOfferDisplayUserControl : UserControl
    {
        public SupplyOfferDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SupplyOfferDisplay");
        }
    }
}
