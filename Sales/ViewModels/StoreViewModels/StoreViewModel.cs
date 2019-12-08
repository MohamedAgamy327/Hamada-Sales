using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;

namespace Sales.ViewModels.StoreViewModels
{
    public class StoreViewModel : ValidatableBindableBase
    {
        private ViewModelBase _currentViewModel = new CompanyViewModel();
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
                case "Company":
                    CurrentViewModel = new CompanyViewModel();
                    break;

                case "Category":
                    CurrentViewModel = new CategoryViewModel();
                    break;
                case "CategoryRequired":
                    CurrentViewModel = new CategoryRequiredViewModel();
                    break;
                default:
                    break;
            }
        }
    }
}
