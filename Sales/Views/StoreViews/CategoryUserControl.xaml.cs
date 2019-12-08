using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.StoreViews
{
    public partial class CategoryUserControl : UserControl
    {
        public CategoryUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("Category");
        }
    }
}
