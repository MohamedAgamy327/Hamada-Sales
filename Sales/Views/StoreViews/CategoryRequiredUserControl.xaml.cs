using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.StoreViews
{
    public partial class CategoryRequiredUserControl : UserControl
    {
        public CategoryRequiredUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("CategoryRequired");
        }
    }
}
