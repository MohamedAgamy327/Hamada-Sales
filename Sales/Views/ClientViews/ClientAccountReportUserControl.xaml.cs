using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.ClientViews
{
    public partial class ClientAccountReportUserControl : UserControl
    {
        public ClientAccountReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientAccountReport");
        }
    }
}
