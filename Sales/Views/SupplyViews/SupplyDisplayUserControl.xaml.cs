using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyDisplayUserControl : UserControl
    {
        public SupplyDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SupplyDisplay");
        }
    }
}
