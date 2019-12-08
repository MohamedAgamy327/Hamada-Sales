using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.StoreViews
{
    public partial class CompanyUserControl : UserControl
    {
        public CompanyUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("Company");
        }
    }
}
