﻿using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using Sales.Views.SaleViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Sales.ViewModels.SaleViewModels
{
    public class SaleOfferDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;

        SaleOfferServices _saleOfferServ = new SaleOfferServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _saleOfferServ.GetSaleOffersNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_saleOfferServ.GetSaleOffersNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            SalesOffers = new ObservableCollection<SaleOffer>(_saleOfferServ.SearchSaleOffers(_key, _currentPage));
        }

        public SaleOfferDisplayViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            Load();
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

        private SaleOffer _selectedSaleOffer;
        public SaleOffer SelectedSaleOffer
        {
            get { return _selectedSaleOffer; }
            set { SetProperty(ref _selectedSaleOffer, value); }
        }

        private ObservableCollection<SaleOffer> _salesOffers;
        public ObservableCollection<SaleOffer> SalesOffers
        {
            get { return _salesOffers; }
            set { SetProperty(ref _salesOffers, value); }
        }

        //Display

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
            SalesOffers = new ObservableCollection<SaleOffer>(_saleOfferServ.SearchSaleOffers(_key, _currentPage));
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
            SalesOffers = new ObservableCollection<SaleOffer>(_saleOfferServ.SearchSaleOffers(_key, _currentPage));
        }

        private RelayCommand _edit;
        public RelayCommand Edit
        {
            get
            {
                return _edit
                    ?? (_edit = new RelayCommand(EditMethod));
            }
        }
        private void EditMethod()
        {
            SaleOfferUpdateViewModel.ID = _selectedSaleOffer.ID;
            _currentWindow.Hide();
            new SaleOfferUpdateWindow().ShowDialog();
            Load();
            _currentWindow.ShowDialog();
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
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذه الطلبيه؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _saleOfferServ.DeleteSaleOffer(_selectedSaleOffer);
                Load();
            }
        }

        private RelayCommand _convert;
        public RelayCommand ConvertToBill
        {
            get
            {
                return _convert
                    ?? (_convert = new RelayCommand(ConvertMethod));
            }
        }
        private  void ConvertMethod()
        {
            SaleOfferConvertViewModel.ID = _selectedSaleOffer.ID;
            _currentWindow.Hide();
            new SaleOfferConvertWindow().ShowDialog();
            Load();
            _currentWindow.ShowDialog();
        }


        // Add Bill

        private RelayCommand _showAdd;
        public RelayCommand ShowAdd
        {
            get
            {
                return _showAdd
                    ?? (_showAdd = new RelayCommand(ShowAddMethod));
            }
        }
        private void ShowAddMethod()
        {
            _currentWindow.Hide();
            new SaleOfferAddWindow().ShowDialog();
            Load();
            _currentWindow.ShowDialog();
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
            SaleOfferShowViewModel.ID = _selectedSaleOffer.ID;
            _currentWindow.Hide();
            new SaleOfferShowWindow().ShowDialog();
            Load();
            _currentWindow.ShowDialog();
        }
    }
}
