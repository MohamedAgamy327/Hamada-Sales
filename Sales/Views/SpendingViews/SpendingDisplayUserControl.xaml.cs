using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.SpendingViews
{
    public partial class SpendingDisplayUserControl : UserControl
    {
        public SpendingDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingDisplay");
        }
    }
}
