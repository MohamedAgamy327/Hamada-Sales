using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SpendingViews
{
    public partial class SpendingReportUserControl : UserControl
    {
        public SpendingReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingReport");
        }
    }
}
