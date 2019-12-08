using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Sales
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
