using Sales.ViewModels;
using MahApps.Metro.Controls;
using Sales.SplashScreen;
using System.Threading;
using System.ServiceProcess;
using Sales.Models;
using Sales.Services;
using System.Linq;
using System.Data.Entity;

namespace Sales.Views.MainViews
{
    public partial class MainWindow : MetroWindow
    {
        CategoryServices catServ = new CategoryServices();
        SalesDB db = new SalesDB();

        public MainWindow()
        {
            InitializeComponent();

            ServiceController service = new ServiceController("MSSQL$SQLEXPRESS");
            if (service.Status != ServiceControllerStatus.Running)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            //var categories = db.Categories;
            //foreach (var item in categories)
            //{
            //    var salesSum = db.SalesCategories.Where(w => w.CategoryID == item.ID).Sum(s => s.Qty);
            //    if (salesSum == null)
            //        salesSum = 0;
            //    var suppliesSum = db.SuppliesCategories.Where(w => w.CategoryID == item.ID).Sum(s => s.Qty);
            //    if (suppliesSum == null)
            //        suppliesSum = 0;
            //    item.Qty = item.QtyStart + suppliesSum - salesSum;
            //    db.Entry(item).State = EntityState.Modified;
            //}
            //db.SaveChanges();
            Hide();
            Splasher.Splash = new SplashScreenWindow();
            Splasher.ShowSplash();

            for (int i = 0; i < 100; i++)
            {
                if (i % 10 == 0)
                    MessageListener.Instance.ReceiveMessage(string.Format("Loading " + "{0}" + " %", i));
                Thread.Sleep(40);
            }
            Splasher.CloseSplash();
            Show();
            Closing += (s, e) => ViewModelLocator.Cleanup("Main");
        }
    }
}