using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyShowWindow : MetroWindow
    {
        public SupplyShowWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SupplyShow");
        }
    }
}
