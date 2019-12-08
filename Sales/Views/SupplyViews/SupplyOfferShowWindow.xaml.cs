using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyOfferShowWindow :  MetroWindow
    {
        public SupplyOfferShowWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SupplyOfferShow");
        }
    }
}
