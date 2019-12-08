using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SpendingViews
{
    public partial class SpendingWindow : MetroWindow
    {
        public SpendingWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Spnending");
        }
    }
}
