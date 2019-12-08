using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyReportUserControl : UserControl
    {
        public SupplyReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SupplyReport");
        }
    }
}
