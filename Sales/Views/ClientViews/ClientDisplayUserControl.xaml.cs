using Sales.ViewModels;
using System.Windows.Controls;

namespace Sales.Views.ClientViews
{

    public partial class ClientDisplayUserControl : UserControl
    {
        public ClientDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientDisplay");
        }
    }
}
