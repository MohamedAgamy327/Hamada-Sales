using Sales.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Sales.ViewModels.SpendingViewModels
{
    public class SpendingViewModel : ValidatableBindableBase
    {
        private ViewModelBase _currentViewModel = new SpendingDisplayViewModel();
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
                case "SpendingDisplay":
                    CurrentViewModel = new SpendingDisplayViewModel();
                    break;
                case "SpendingReport":
                    CurrentViewModel = new SpendingReportViewModel();
                    break;

                default:
                    break;
            }
        }
    }
}
