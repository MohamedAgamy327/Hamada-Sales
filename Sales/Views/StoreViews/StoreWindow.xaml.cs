using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.StoreViews
{
    public partial class StoreWindow : MetroWindow
    {
        public StoreWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Store");
        }
    }
}
