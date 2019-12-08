using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.MainViews
{
    public partial class SupplyFutureDisplayWindow : MetroWindow
    {
        public SupplyFutureDisplayWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SupplyFuture");
        }
    }
}
