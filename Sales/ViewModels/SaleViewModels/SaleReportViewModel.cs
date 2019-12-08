using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using System;
using System.Collections.ObjectModel;

namespace Sales.ViewModels.SaleViewModels
{
    public class SaleReportViewModel : ValidatableBindableBase
    {
        SaleServices _saleServ = new SaleServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _saleServ.GetSalesNumer(Key, _dateFrom, _dateTo);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_saleServ.GetSalesNumer(_key, _dateFrom, _dateTo) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            TotalPrice = _saleServ.GetSalesPrice(_key, _dateFrom, _dateTo);
            TotalCost = _saleServ.GetSalesCost(_key, _dateFrom, _dateTo);
            Sales = new ObservableCollection<Sale>(_saleServ.SearchSales(_key, _currentPage, _dateFrom, _dateTo));
        }

        public SaleReportViewModel()
        {
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
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

        private decimal? _totalCost;
        public decimal? TotalCost
        {
            get { return _totalCost; }
            set
            {
                SetProperty(ref _totalCost, value);
                OnPropertyChanged("TotalProfit");
            }
        }

        private decimal? _totalPrice;
        public decimal? TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                SetProperty(ref _totalPrice, value);
                OnPropertyChanged("TotalProfit");
            }
        }

        private decimal? _totalProfit;
        public decimal? TotalProfit
        {
            get { return _totalProfit = TotalPrice - TotalCost; }
            set { SetProperty(ref _totalProfit, value); }
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

        private ObservableCollection<Sale> _sales;
        public ObservableCollection<Sale> Sales
        {
            get { return _sales; }
            set { SetProperty(ref _sales, value); }
        }

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
            Sales = new ObservableCollection<Sale>(_saleServ.SearchSales(_key, _currentPage, _dateFrom, _dateTo));
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
            Sales = new ObservableCollection<Sale>(_saleServ.SearchSales(_key, _currentPage, _dateFrom, _dateTo));
        }
    }
}
