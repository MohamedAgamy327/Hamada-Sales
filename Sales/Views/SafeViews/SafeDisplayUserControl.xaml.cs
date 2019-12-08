using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SafeViews
{
    public partial class SafeDisplayUserControl : UserControl
    {
        public SafeDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeDisplay");
        }
    }
}
