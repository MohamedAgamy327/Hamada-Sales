using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SaleViews
{
    public partial class SaleWindow : MetroWindow
    {
        public SaleWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Sale");
        }
    }
}
