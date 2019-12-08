using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using Sales.Views.SafeViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Sales.ViewModels.SafeViewModels
{
    public class SafeReportViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        SafeServices _safeServ = new SafeServices();
        private readonly SafeDetailsDialog _safeDetailsDialog;

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _safeServ.GetSafesNumer(Key, _dateFrom, _dateTo);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_safeServ.GetSafesNumer(_key, _dateFrom, _dateTo) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            TotalIncome = _safeServ.GetTotalIncome(_key, _dateFrom, _dateTo);
            TotalOutgoings = _safeServ.GetTotalOutgoings(_key, _dateFrom, _dateTo);
            Safes = new ObservableCollection<Safe>(_safeServ.SearchSafes(_key, _currentPage, _dateFrom, _dateTo));
        }

        public SafeReportViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _safeDetailsDialog = new SafeDetailsDialog();
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _currentAccount = _safeServ.GetCurrentAccount();
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

        private decimal? _currentAccount;
        public decimal? CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }

        private decimal? _totalIncome;
        public decimal? TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                SetProperty(ref _totalIncome, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _totalOutgoings;
        public decimal? TotalOutgoings
        {
            get { return _totalOutgoings; }
            set
            {
                SetProperty(ref _totalOutgoings, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _difference;
        public decimal? Difference
        {
            get { return _difference = _totalIncome + _totalOutgoings; }
            set { SetProperty(ref _difference, value); }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }

        private decimal? _safeItem;
        public decimal? SafeItem
        {
            get { return _safeItem; }
            set{ SetProperty(ref _safeItem, value);}
        }

        private decimal? _supplyItem;
        public decimal? SupplyItem
        {
            get { return _supplyItem; }
            set { SetProperty(ref _supplyItem, value); }
        }

        private decimal? _saleItem;
        public decimal? SaleItem
        {
            get { return _saleItem; }
            set { SetProperty(ref _saleItem, value); }
        }

        private decimal? _spendingItem;
        public decimal? SpendingItem
        {
            get { return _spendingItem; }
            set { SetProperty(ref _spendingItem, value); }
        }

        private decimal? _accountCatchItem;
        public decimal? AccountCatchItem
        {
            get { return _accountCatchItem; }
            set { SetProperty(ref _accountCatchItem, value); }
        }

        private decimal? _accountPayItem;
        public decimal? AccountPayItem
        {
            get { return _accountPayItem; }
            set { SetProperty(ref _accountPayItem, value); }
        }

        private decimal? _debtPayItem;
        public decimal? DebtPayItem
        {
            get { return _debtPayItem; }
            set { SetProperty(ref _debtPayItem, value); }
        }

        private decimal? _debtCatchItem;
        public decimal? DebtCatchItem
        {
            get { return _debtCatchItem; }
            set { SetProperty(ref _debtCatchItem, value); }
        }

        private decimal? _premiumPayItem;
        public decimal? PremiumPayItem
        {
            get { return _premiumPayItem; }
            set { SetProperty(ref _premiumPayItem, value); }
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

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private ObservableCollection<Safe> _safes;
        public ObservableCollection<Safe> Safes
        {
            get { return _safes; }
            set { SetProperty(ref _safes, value); }
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
            Safes = new ObservableCollection<Safe>(_safeServ.SearchSafes(_key, _currentPage, _dateFrom, _dateTo));
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
            Safes = new ObservableCollection<Safe>(_safeServ.SearchSafes(_key, _currentPage, _dateFrom, _dateTo));
        }

        //Report

        private RelayCommand _showDetails;
        public RelayCommand ShowDetails
        {
            get
            {
                return _showDetails
                    ?? (_showDetails = new RelayCommand(ShowDetailsMethod));
            }
        }
        private async void ShowDetailsMethod()
        {
            SafeItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 1);
            SpendingItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 2);
            SupplyItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 3);
            SaleItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 4);
            AccountPayItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 5);
            AccountCatchItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 6);
            DebtPayItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 7);
            DebtCatchItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 8);
            PremiumPayItem = _safeServ.GetItemSum(_key, _dateFrom, _dateTo, 9);
            _safeDetailsDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_safeDetailsDialog);
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
                case "Details":
                    await _currentWindow.HideMetroDialogAsync(_safeDetailsDialog);
                    Load();
                    break;
                default:
                    break;
            }

        }

    }
}
