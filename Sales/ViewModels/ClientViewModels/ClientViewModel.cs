using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;

namespace Sales.ViewModels.ClientViewModels
{
    public class ClientViewModel : ValidatableBindableBase
    {
        private ViewModelBase _currentViewModel = new ClientDisplayViewModel();
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethod));
            }
        }
        private void NavigateToViewMethod(string destination)
        {
            switch (destination)
            {
                case "ClientDisplay":
                    CurrentViewModel = new ClientDisplayViewModel();
                    break;
                case "ClientAccountDisplay":
                    CurrentViewModel = new ClientAccountDisplayViewModel();
                    break;
                case "ClientAccountReport":
                    CurrentViewModel = new ClientAccountReportViewModel();
                    break;
                default:
                    break;
            }
        }
    }
}
