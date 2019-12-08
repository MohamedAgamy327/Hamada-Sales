using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace Sales.Views.SaleViews
{
    public partial class SaleRecallDialog : CustomDialog
    {
        public SaleRecallDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in pnl.Children)
            {
                if (item is NumericUpDown)
                {
                    NumericUpDown nud = item as NumericUpDown;
                    nud.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
                }
            }
        }
    }
}
