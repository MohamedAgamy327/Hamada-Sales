using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Sales.Views.StoreViews
{
    public partial class CompanyAddDialog : CustomDialog
    {
        public CompanyAddDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item  in pnl.Children)
            {
                if (item is TextBox)
                {
                    TextBox txt = item as TextBox;
                    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }
    }
}
