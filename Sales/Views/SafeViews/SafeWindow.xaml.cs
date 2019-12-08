using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.SafeViews
{
    public partial class SafeWindow : MetroWindow
    {
        public SafeWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Safe");
        }
    }
}
