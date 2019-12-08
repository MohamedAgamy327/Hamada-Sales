using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;

namespace Sales.ViewModels.SafeViewModels
{
    public class SafeViewModel : ValidatableBindableBase
    {
        private ViewModelBase _currentViewModel = new SafeDisplayViewModel();
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
                case "SafeDisplay":
                    CurrentViewModel = new SafeDisplayViewModel();
                    break;
                case "SafeReport":
                    CurrentViewModel = new SafeReportViewModel();
                    break;
                
                default:
                    break;
            }
        }
    }
}
