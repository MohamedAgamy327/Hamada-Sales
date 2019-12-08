using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyFutureDisplayUserControl : UserControl
    {
        public SupplyFutureDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SupplyFutureDisplay");
        }
    }
}
