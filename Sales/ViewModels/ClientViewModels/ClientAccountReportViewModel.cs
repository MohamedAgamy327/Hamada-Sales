using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using System;
using System.Collections.ObjectModel;

namespace Sales.ViewModels.ClientViewModels
{
    public class ClientAccountReportViewModel : ValidatableBindableBase
    {
        ClientAccountServices _clientAccountServ = new ClientAccountServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _clientAccountServ.GetClientsAccountsNumer(Key, _dateFrom, _dateTo);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_clientAccountServ.GetClientsAccountsNumer(_key, _dateFrom, _dateTo) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            TotalDebit = _clientAccountServ.GetTotalDebit(_key, _dateFrom, _dateTo);
            TotalCredit = _clientAccountServ.GetTotalCredit(_key, _dateFrom, _dateTo);
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage, _dateFrom, _dateTo));
        }

        public ClientAccountReportViewModel()
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

        private decimal? _totalCredit;
        public decimal? TotalCredit
        {
            get { return _totalCredit; }
            set { SetProperty(ref _totalCredit, value); }
        }

        private decimal? _totalDebit;
        public decimal? TotalDebit
        {
            get { return _totalDebit; }
            set { SetProperty(ref _totalDebit, value); }
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

        private ObservableCollection<ClientAccount> _clientsAccounts;
        public ObservableCollection<ClientAccount> ClientsAccounts
        {
            get { return _clientsAccounts; }
            set { SetProperty(ref _clientsAccounts, value); }
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
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage, _dateFrom, _dateTo));
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
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage, _dateFrom, _dateTo));
        }
    }
}
