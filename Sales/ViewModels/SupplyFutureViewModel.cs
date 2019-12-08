using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using Sales.ViewModels.SupplyViewModels;
using Sales.Views.SupplyViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Sales.ViewModels
{
    public class SupplyFutureViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        SupplyFutureServices _supplyFutureServ = new SupplyFutureServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _supplyFutureServ.GetSuppliesFutureNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_supplyFutureServ.GetSuppliesFutureNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            SuppliesFuture = new ObservableCollection<SupplyFuture>(_supplyFutureServ.SearchSuppliesFuture(_key, _currentPage));
        }

        public SupplyFutureViewModel()
        {
            Load();
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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

        private SupplyFuture _selectedSupplyFuture;
        public SupplyFuture SelectedSupplyFuture
        {
            get { return _selectedSupplyFuture; }
            set { SetProperty(ref _selectedSupplyFuture, value); }
        }

        private ObservableCollection<SupplyFuture> _suppliesFuture;
        public ObservableCollection<SupplyFuture> SuppliesFuture
        {
            get { return _suppliesFuture; }
            set { SetProperty(ref _suppliesFuture, value); }
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
            SuppliesFuture = new ObservableCollection<SupplyFuture>(_supplyFutureServ.SearchSuppliesFuture(_key, _currentPage));
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
            SuppliesFuture = new ObservableCollection<SupplyFuture>(_supplyFutureServ.SearchSuppliesFuture(_key, _currentPage));
        }

        // Show Bill

        private RelayCommand _showDetials;
        public RelayCommand ShowDetials
        {
            get
            {
                return _showDetials
                    ?? (_showDetials = new RelayCommand(ShowDetialsMethod));
            }
        }
        private void ShowDetialsMethod()
        {
            SupplyShowViewModel.ID = _selectedSupplyFuture.SupplyID;
            _currentWindow.Hide();
            new SupplyShowWindow().ShowDialog();
            _currentWindow.ShowDialog();
        }

    }
}
