using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using System;
using System.Collections.ObjectModel;
using Sales.Views.StoreViews;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace Sales.ViewModels.StoreViewModels
{
    public class CompanyViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly CompanyAddDialog _companyAddDialog;

        CompanyServices _companyServ = new CompanyServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _companyServ.GetCompaniesNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_companyServ.GetCompaniesNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            Companies = new ObservableCollection<Company>(_companyServ.SearchCompanies(_key, _currentPage));
        }

        public CompanyViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _companyAddDialog = new CompanyAddDialog();
            _companiesSuggestions = _companyServ.GetCompaniesSuggetions();
            Load();
        }

        private bool _isFocused = true;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private bool _isFirst;
        public bool ISFirst
        {
            get { return _isFirst; }
            set { SetProperty(ref _isFirst, value); }
        }

        private bool _isLast;
        public bool ISLast
        {
            get { return _isLast; }
            set { SetProperty(ref _isLast, value); }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        private int _lastPage;
        public int LastPage
        {
            get { return _lastPage; }
            set { SetProperty(ref _lastPage, value); }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }

        private string _key = "";
        public string Key
        {
            get { return _key; }
            set
            {
                SetProperty(ref _key, value);
            }
        }

        //private string _validError;
        //public string ValidError
        //{
        //    get { return _validError; }
        //    set { SetProperty(ref _validError, value); }
        //}

        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get { return _selectedCompany; }
            set { SetProperty(ref _selectedCompany, value); }
        }

        private Company _newCompany = new Company();
        public Company NewCompany
        {
            get { return _newCompany; }
            set { SetProperty(ref _newCompany, value); }
        }

        private List<string> _companiesSuggestions = new List<string>();
        public List<string> CompaniesSuggestions
        {
            get { return _companiesSuggestions; }
            set { SetProperty(ref _companiesSuggestions, value); }
        }

        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }

        // Display

        private RelayCommand _search;
        public RelayCommand Search
        {
            get
            {
                return _search
                    ?? (_search = new RelayCommand(SearchMethod));
            }
        }
        private void SearchMethod()
        {
            Load();
        }

        private RelayCommand _next;
        public RelayCommand Next
        {
            get
            {
                return _next
                    ?? (_next = new RelayCommand(NextMethod));
            }
        }
        private void NextMethod()
        {
            CurrentPage++;
            ISFirst = true;
            if (_currentPage == _lastPage)
                ISLast = false;
            Companies = new ObservableCollection<Company>(_companyServ.SearchCompanies(_key, _currentPage));
        }

        private RelayCommand _previous;
        public RelayCommand Previous
        {
            get
            {
                return _previous
                    ?? (_previous = new RelayCommand(PreviousMethod));
            }
        }
        private void PreviousMethod()
        {
            CurrentPage--;
            ISLast = true;
            if (_currentPage == 1)
                ISFirst = false;
            Companies = new ObservableCollection<Company>(_companyServ.SearchCompanies(_key, _currentPage));
        }

        private RelayCommand _delete;
        public RelayCommand Delete
        {
            get
            {
                return _delete
                    ?? (_delete = new RelayCommand(DeleteMethod));
            }
        }
        private async void DeleteMethod()
        {
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذه الشركة؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _companyServ.DeleteCompany(_selectedCompany);
                Load();
            }
        }

        // Add

        private RelayCommand _showAdd;
        public RelayCommand ShowAdd
        {
            get
            {
                return _showAdd
                    ?? (_showAdd = new RelayCommand(ShowAddMethod));
            }
        }
        private async void ShowAddMethod()
        {
            _companyAddDialog.DataContext = this;
            NewCompany = new Company();
            await _currentWindow.ShowMetroDialogAsync(_companyAddDialog);
        }

        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(
                    ExecuteSaveAsync,
                    CanExecuteSave));
            }
        }
        private async void ExecuteSaveAsync()
        {
            if (_companyServ.GetCompany(_newCompany.Name) != null)
            {
                await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذه الشركة موجودة مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
            }
            else
            {
                _companyServ.AddCompany(_newCompany);
                _companiesSuggestions.Add(_newCompany.Name);
                NewCompany = new Company();
                await _currentWindow.ShowMessageAsync("نجاح الإضافة", "تم الإضافة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
            }
        }
        private bool CanExecuteSave()
        {
            return !NewCompany.HasErrors;
        }

        private RelayCommand<string> _closeDialog;
        public RelayCommand<string> CloseDialog
        {
            get
            {
                return _closeDialog
                    ?? (_closeDialog = new RelayCommand<string>(ExecuteCloseDialogAsync));
            }
        }
        private async void ExecuteCloseDialogAsync(string parameter)
        {
            switch (parameter)
            {
                case "Add":
                    await _currentWindow.HideMetroDialogAsync(_companyAddDialog);
                    Load();
                    break;
                default:
                    break;
            }

        }

    }

}
