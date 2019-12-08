using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SaleViews
{
    public partial class SaleShowWindow : MetroWindow
    {
        public SaleShowWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SaleShow");
        }

     
    }
}
