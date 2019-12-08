using MahApps.Metro.Controls;
using Sales.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Sales.Views.SaleViews
{
    public partial class SaleAddWindow : MetroWindow
    {
        public SaleAddWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SaleAdd");
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
            Price.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            OldDebt.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            TransportCost.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            Paid.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
        }
    }
}
