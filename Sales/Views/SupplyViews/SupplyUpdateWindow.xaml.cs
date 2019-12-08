using MahApps.Metro.Controls;
using Sales.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Sales.Views.SupplyViews
{
    public partial class SupplyUpdateWindow : MetroWindow
    {
        public SupplyUpdateWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SupplyUpdate");
            btn1.Click += Btn1_Click;
            btn2.Click += Btn2_Click;
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in pnl1.Children)
            {
                if (item is TextBox)
                {
                    TextBox txt = item as TextBox;
                    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
                if (item is NumericUpDown)
                {
                    NumericUpDown nud = item as NumericUpDown;
                    nud.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
                }
            }
        }
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            Cost.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            OldDebt.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            CashPaid.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            DiscountPaid.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
        }
    }
}
