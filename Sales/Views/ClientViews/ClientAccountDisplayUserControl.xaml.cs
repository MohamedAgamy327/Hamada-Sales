using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.ClientViews
{
    public partial class ClientAccountDisplayUserControl : UserControl
    {
        public ClientAccountDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientAccountDisplay");
        }
    }
}
