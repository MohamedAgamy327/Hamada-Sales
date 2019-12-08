using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyWindow : MetroWindow
    {
        public SupplyWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Supply");
        }
    }
}
