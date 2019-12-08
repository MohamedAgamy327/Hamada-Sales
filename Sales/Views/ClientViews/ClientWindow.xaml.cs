using MahApps.Metro.Controls;
using Sales.ViewModels;

namespace Sales.Views.ClientViews
{
    public partial class ClientWindow : MetroWindow
    {
        public ClientWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Client");
        }
    }
}
