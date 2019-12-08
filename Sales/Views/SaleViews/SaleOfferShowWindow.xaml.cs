using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SaleViews
{
    public partial class SaleOfferShowWindow : MetroWindow
    {
        public SaleOfferShowWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SaleOfferShow");
        }
    }
}
