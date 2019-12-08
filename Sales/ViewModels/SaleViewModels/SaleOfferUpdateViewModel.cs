using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Reports;
using Sales.Services;
using Sales.Views.SaleViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sales.ViewModels.SaleViewModels
{
    public class SaleOfferUpdateViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly CategoriesShowDialog _categoriesDialog;

        SaleOfferServices _saleOfferServ = new SaleOfferServices();
        ClientServices _clientServ = new ClientServices();
        CategoryServices _categoryServ = new CategoryServices();
        SaleCategoryServices _saleCategoryServ = new SaleCategoryServices();
        SaleOfferCategoryServices _saleOfferCategoryServ = new SaleOfferCategoryServices();

        public static int ID
        {
            get; set;
        }

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_categoryServ.GetCategoriesNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            Categories = new ObservableCollection<Category>(_categoryServ.SearchCategories(_key, _currentPage));
        }

        public SaleOfferUpdateViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _categoriesDialog = new CategoriesShowDialog();
            Clients = new ObservableCollection<Client>(_clientServ.GetClients());
            _selectedSaleOffer = _saleOfferServ.GetSaleOffer(ID);
            _saleOfferCategories = new ObservableCollection<SaleOfferCategoryVM>(_saleOfferCategoryServ.GetSaleOfferCategories(ID));
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

        private string _key = "";
        public string Key
        {
            get { return _key; }
            set
            {
                SetProperty(ref _key, value);
            }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private SaleOffer _selectedSaleOffer;
        public SaleOffer SelectedSaleOffer
        {
            get { return _selectedSaleOffer; }
            set { SetProperty(ref _selectedSaleOffer, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private SaleOfferCategoryVM _newSaleOfferCategory;
        public SaleOfferCategoryVM NewSaleOfferCategory
        {
            get { return _newSaleOfferCategory; }
            set { SetProperty(ref _newSaleOfferCategory, value); }
        }

        private SaleOfferCategoryVM _selectedSaleOfferCategory;
        public SaleOfferCategoryVM SelectedSaleOfferCategory
        {
            get { return _selectedSaleOfferCategory; }
            set { SetProperty(ref _selectedSaleOfferCategory, value); }
        }

        private SaleCategory _selectedOldPrice;
        public SaleCategory SelectedOldPrice
        {
            get { return _selectedOldPrice; }
            set { SetProperty(ref _selectedOldPrice, value); }
        }

        private ObservableCollection<SaleOfferCategoryVM> _saleOfferCategories;
        public ObservableCollection<SaleOfferCategoryVM> SaleOfferCategories
        {
            get { return _saleOfferCategories; }
            set { SetProperty(ref _saleOfferCategories, value); }
        }

        private ObservableCollection<SaleCategory> _oldPrices = new ObservableCollection<SaleCategory>();
        public ObservableCollection<SaleCategory> OldPrices
        {
            get { return _oldPrices; }
            set { SetProperty(ref _oldPrices, value); }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        // Bill

        private RelayCommand _getclientPrice;
        public RelayCommand GetClientPrice
        {
            get
            {
                return _getclientPrice ?? (_getclientPrice = new RelayCommand(
                   GetClientPriceMethod));
            }
        }
        private void GetClientPriceMethod()
        {
            try
            {

                if (NewSaleOfferCategory != null)
                    OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleOfferCategory.CategoryID, _selectedClient.ID));
                else
                    OldPrices = new ObservableCollection<SaleCategory>();
            }
            catch
            {
                OldPrices = new ObservableCollection<SaleCategory>();
            }
        }

        private RelayCommand _selectPrice;
        public RelayCommand SelectPrice
        {
            get
            {
                return _selectPrice ?? (_selectPrice = new RelayCommand(
                    ExecuteSelectPrice));
            }
        }
        private void ExecuteSelectPrice()
        {
            if (SelectedOldPrice == null)
                return;
            NewSaleOfferCategory.Price = _selectedOldPrice.Price;
            NewSaleOfferCategory.Discount = _selectedOldPrice.Discount;
        }

        private RelayCommand _addToBill;
        public RelayCommand AddToBill
        {
            get
            {
                return _addToBill
                    ?? (_addToBill = new RelayCommand(ExecuteAddToBillAsync, CanExecuteAddToBill));
            }
        }
        private async void ExecuteAddToBillAsync()
        {
            if (NewSaleOfferCategory.PriceTotalAfterDiscount == null || NewSaleOfferCategory.Category == null)
                return;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            };
            if (_saleOfferCategories.SingleOrDefault(s => s.CategoryID == _newSaleOfferCategory.CategoryID) != null)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", "هذا الصنف موجود مسبقاً فى الفاتورة", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            _saleOfferCategories.Add(_newSaleOfferCategory);
            NewSaleOfferCategory = new SaleOfferCategoryVM();
            SelectedSaleOffer.PriceAfterDiscount = SaleOfferCategories.Sum(s => s.PriceTotalAfterDiscount);
            SelectedSaleOffer.Price = SaleOfferCategories.Sum(s => s.PriceTotal);
            OldPrices = new ObservableCollection<SaleCategory>();
        }
        private bool CanExecuteAddToBill()
        {
            try
            {
                return !NewSaleOfferCategory.HasErrors;
            }
            catch
            {
                return false;
            }
        }

        private RelayCommand _delete;
        public RelayCommand Delete
        {
            get
            {
                return _delete
                    ?? (_delete = new RelayCommand(DeleteMethodAsync));
            }
        }
        private async void DeleteMethodAsync()
        {
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا الصنف؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _saleOfferCategories.Remove(_selectedSaleOfferCategory);
                SelectedSaleOffer.PriceAfterDiscount = SaleOfferCategories.Sum(s => s.PriceTotalAfterDiscount);
                SelectedSaleOffer.Price = SaleOfferCategories.Sum(s => s.PriceTotal);
            }
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
            NewSaleOfferCategory = SelectedSaleOfferCategory;
            _saleOfferCategories.Remove(_selectedSaleOfferCategory);
            _selectedCategory = _categoryServ.GetCategory(_newSaleOfferCategory.CategoryID);
            SelectedSaleOffer.PriceAfterDiscount = SaleOfferCategories.Sum(s => s.PriceTotalAfterDiscount);
            SelectedSaleOffer.Price = SaleOfferCategories.Sum(s => s.PriceTotal);
            OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleOfferCategory.CategoryID, _selectedSaleOffer.ClientID));
        }

        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(
                    ExecuteSave,
                    CanExecuteSave));
            }
        }
        private void ExecuteSave()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            _selectedSaleOffer.Client = _selectedClient;
            _saleOfferServ.UpdateSaleOffer(_selectedSaleOffer);       
            _saleOfferCategoryServ.DeleteSaleOfferCategories(ID);
            foreach (var item in _saleOfferCategories)
            {
                SaleOfferCategory _saleOfferCategory = new SaleOfferCategory
                {
                    CategoryID = item.CategoryID,
                    PriceAfterDiscount = item.PriceAfterDiscount,
                    PriceTotalAfterDiscount = item.PriceTotalAfterDiscount,
                    Price = item.Price,
                    PriceTotal = item.PriceTotal,
                    Discount = item.Discount,
                    DiscountValue = item.DiscountValue,
                    DiscountValueTotal = item.DiscountValueTotal,
                    SaleOfferID = ID,
                    Qty = item.Qty
                };
                _saleOfferCategoryServ.AddSaleOfferCategory(_saleOfferCategory);
            }

            DS ds = new DS();
            ds.Sale.Rows.Clear();
            int i = 0;
            foreach (var item in _saleOfferCategories)
            {
                ds.Sale.Rows.Add();
                ds.Sale[i]["Client"] = _selectedSaleOffer.Client.Name;
                ds.Sale[i]["Serial"] = i + 1;
                ds.Sale[i]["Category"] = item.Category + " " + item.Company;
                ds.Sale[i]["Qty"] = item.Qty;
                ds.Sale[i]["Price"] = Math.Round(Convert.ToDecimal(item.PriceAfterDiscount), 2);
                ds.Sale[i]["TotalPrice"] = Math.Round(Convert.ToDecimal(item.PriceTotalAfterDiscount), 2);
                ds.Sale[i]["BillPrice"] = Math.Round(Convert.ToDecimal(_selectedSaleOffer.PriceAfterDiscount), 2); ;
                i++;
            }
            ReportWindow rpt = new ReportWindow();
            SaleOfferReport saleOfferRPT = new SaleOfferReport();
            saleOfferRPT.SetDataSource(ds.Tables["Sale"]);
            rpt.crv.ViewerCore.ReportSource = saleOfferRPT;
            Mouse.OverrideCursor = null;
            _currentWindow.Hide();
            rpt.ShowDialog();
            _currentWindow.Close();
        }
        private bool CanExecuteSave()
        {
            try
            {
                if (SelectedSaleOffer.HasErrors || SelectedClient == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        //Categories

        private RelayCommand _browseCategories;
        public RelayCommand BrowseCategories
        {
            get
            {
                return _browseCategories ?? (_browseCategories = new RelayCommand(
                    ExecuteBrowseCategoriesAsync));
            }
        }
        private async void ExecuteBrowseCategoriesAsync()
        {
            OldPrices = new ObservableCollection<SaleCategory>();
            NewSaleOfferCategory = new SaleOfferCategoryVM();
            Key = "";
            Load();
            _categoriesDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_categoriesDialog);
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
            Categories = new ObservableCollection<Category>(_categoryServ.SearchCategories(_key, _currentPage));
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
            Categories = new ObservableCollection<Category>(_categoryServ.SearchCategories(_key, _currentPage));
        }

        private RelayCommand _selectCategory;
        public RelayCommand SelectCategory
        {
            get
            {
                return _selectCategory
                    ?? (_selectCategory = new RelayCommand(ExecuteSelectCategoryAsync));
            }
        }
        private async void ExecuteSelectCategoryAsync()
        {
            if (SelectedCategory == null)
                return;
            NewSaleOfferCategory.Category = SelectedCategory.Name;
            NewSaleOfferCategory.CategoryID = SelectedCategory.ID;
            NewSaleOfferCategory.Company = SelectedCategory.Company.Name;
            NewSaleOfferCategory.Price = SelectedCategory.Price;
            await _currentWindow.HideMetroDialogAsync(_categoriesDialog);
            OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleOfferCategory.CategoryID, _selectedSaleOffer.ClientID));
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
                case "Categories":
                    await _currentWindow.HideMetroDialogAsync(_categoriesDialog);
                    Load();
                    break;
                default:
                    break;
            }

        }
    }
}
