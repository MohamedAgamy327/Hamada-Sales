using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SafeViews
{
    public partial class SafeReportUserControl : UserControl
    {
        public SafeReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeReport");
        }
    }
}
