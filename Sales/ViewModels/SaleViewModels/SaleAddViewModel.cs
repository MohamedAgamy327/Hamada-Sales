using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Reports;
using Sales.Services;
using Sales.Views.SaleViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sales.ViewModels.SaleViewModels
{
    public class SaleAddViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly CategoriesShowDialog _categoriesDialog;
        private readonly SaleCategoryInfromationDialog _categoryDialog;

        SafeServices _safeServ = new SafeServices();
        SaleServices _saleServ = new SaleServices();
        ClientServices _clientServ = new ClientServices();
        CategoryServices _categoryServ = new CategoryServices();
        SalespersonServices _salespersonServ = new SalespersonServices();
        SaleCategoryServices _saleCategoryServ = new SaleCategoryServices();
        ClientAccountServices _clientAccountServ = new ClientAccountServices();

        List<CurrencyInfo> currencies = new List<CurrencyInfo>();

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

        public SaleAddViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _categoriesDialog = new CategoriesShowDialog();
            _categoryDialog = new SaleCategoryInfromationDialog();
            Clients = new ObservableCollection<Client>(_clientServ.GetClients());
            Salespersons = new ObservableCollection<Salesperson>(_salespersonServ.GetSalespersons());
            NewSale.Date = DateTime.Now;
            Report = "تقرير الفاتورة";
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Egypt));
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

        private string _report;
        public string Report
        {
            get { return _report; }
            set { SetProperty(ref _report, value); }
        }

        private Sale _newSale = new Sale();
        public Sale NewSale
        {
            get { return _newSale; }
            set { SetProperty(ref _newSale, value); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private Salesperson _selectedSalesperson;
        public Salesperson SelectedSalesperson
        {
            get { return _selectedSalesperson; }
            set { SetProperty(ref _selectedSalesperson, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private SaleCategoryVM _newSaleCategory;
        public SaleCategoryVM NewSaleCategory
        {
            get { return _newSaleCategory; }
            set { SetProperty(ref _newSaleCategory, value); }
        }

        private SaleCategoryVM _selectedSaleCategory;
        public SaleCategoryVM SelectedSaleCategory
        {
            get { return _selectedSaleCategory; }
            set { SetProperty(ref _selectedSaleCategory, value); }
        }

        private SaleCategory _selectedOldPrice;
        public SaleCategory SelectedOldPrice
        {
            get { return _selectedOldPrice; }
            set { SetProperty(ref _selectedOldPrice, value); }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<Salesperson> _salespersons;
        public ObservableCollection<Salesperson> Salespersons
        {
            get { return _salespersons; }
            set { SetProperty(ref _salespersons, value); }
        }

        private ObservableCollection<SaleCategoryVM> _saleCategories = new ObservableCollection<SaleCategoryVM>();
        public ObservableCollection<SaleCategoryVM> SaleCategories
        {
            get { return _saleCategories; }
            set { SetProperty(ref _saleCategories, value); }
        }

        private ObservableCollection<SaleCategory> _oldPrices = new ObservableCollection<SaleCategory>();
        public ObservableCollection<SaleCategory> OldPrices
        {
            get { return _oldPrices; }
            set { SetProperty(ref _oldPrices, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        //Bill

        private RelayCommand _getclientAccount;
        public RelayCommand GetClientAccount
        {
            get
            {
                return _getclientAccount ?? (_getclientAccount = new RelayCommand(
                   GetClientAccountMethod));
            }
        }
        private void GetClientAccountMethod()
        {
            try
            {
                NewSale.OldDebt = _clientAccountServ.GetClientAccount(_selectedClient.ID) + _selectedClient.AccountStart;
                if (NewSaleCategory != null)
                    OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleCategory.CategoryID, _selectedClient.ID));
                else
                    OldPrices = new ObservableCollection<SaleCategory>();
            }
            catch
            {
                _newSale.OldDebt = null;
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
            NewSaleCategory.Price = _selectedOldPrice.Price;
            NewSaleCategory.Discount = _selectedOldPrice.Discount;
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
            if (NewSaleCategory.PriceTotalAfterDiscount == null || NewSaleCategory.Category == null)
                return;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            };
            if (_saleCategories.SingleOrDefault(s => s.CategoryID == _newSaleCategory.CategoryID) != null)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", "هذا الصنف موجود مسبقاً فى الفاتورة", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            if (_newSaleCategory.Qty > _selectedCategory.Qty)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", "هذه الكمية غير متوفرة فى المخزن", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            _saleCategories.Add(_newSaleCategory);
            NewSaleCategory = new SaleCategoryVM();
            NewSale.Cost = SaleCategories.Sum(s => s.CostTotal);
            NewSale.PriceAfterDiscount = SaleCategories.Sum(s => s.PriceTotalAfterDiscount);
            NewSale.Price = SaleCategories.Sum(s => s.PriceTotal);
            OldPrices = new ObservableCollection<SaleCategory>();
        }
        private bool CanExecuteAddToBill()
        {
            try
            {
                return !NewSaleCategory.HasErrors;
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
                _saleCategories.Remove(_selectedSaleCategory);
                NewSale.Cost = SaleCategories.Sum(s => s.CostTotal);
                NewSale.PriceAfterDiscount = SaleCategories.Sum(s => s.PriceTotalAfterDiscount);
                NewSale.Price = SaleCategories.Sum(s => s.PriceTotal);
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
            NewSaleCategory = SelectedSaleCategory;
            _saleCategories.Remove(_selectedSaleCategory);
            _selectedCategory = _categoryServ.GetCategory(_newSaleCategory.CategoryID);
            NewSale.Cost = SaleCategories.Sum(s => s.CostTotal);
            NewSale.PriceAfterDiscount = SaleCategories.Sum(s => s.PriceTotalAfterDiscount);
            NewSale.Price = SaleCategories.Sum(s => s.PriceTotal);
            if (SelectedClient != null)
                OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleCategory.CategoryID, _selectedClient.ID));
            else
                OldPrices = new ObservableCollection<SaleCategory>();
        }

        private RelayCommand _showReport;
        public RelayCommand ShowReport
        {
            get
            {
                return _showReport ?? (_showReport = new RelayCommand(
                    ExecuteShowReport));
            }
        }
        private void ExecuteShowReport()
        {
            if (NewSale.Cost != null && NewSale.Cost != 0)
                Report = Math.Round(Convert.ToDecimal(NewSale.PriceAfterDiscount - NewSale.Cost), 2) + " جنيهاً";
            else
                Report = "لا يوجد أصناف فى الفاتورة";
        }

        private RelayCommand _hideReport;
        public RelayCommand HideReport
        {
            get
            {
                return _hideReport ?? (_hideReport = new RelayCommand(
                    ExecuteHideReport));
            }
        }
        private void ExecuteHideReport()
        {
            Report = "تقرير الفاتورة";
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
            DateTime _dt = DateTime.Now;
            _newSale.RegistrationDate = _dt;
            _saleServ.AddSale(_newSale);
            int _saleID = _saleServ.GetLastSaleID();

            foreach (var item in _saleCategories)
            {
                SaleCategory _saleCategory = new SaleCategory
                {
                    CategoryID = item.CategoryID,
                    Cost = item.Cost,
                    PriceAfterDiscount = item.PriceAfterDiscount,
                    PriceTotalAfterDiscount = item.PriceTotalAfterDiscount,
                    CostTotal = item.CostTotal,
                    Price = item.Price,
                    PriceTotal = item.PriceTotal,
                    Discount = item.Discount,
                    DiscountValue = item.DiscountValue,
                    DiscountValueTotal = item.DiscountValueTotal,
                    SaleID = _saleID,
                    Qty = item.Qty
                };
                _saleCategoryServ.AddSaleCategory(_saleCategory);

                Category cat = _categoryServ.GetCategory(item.CategoryID);
                cat.Qty = cat.Qty - item.Qty;
                _categoryServ.UpdateCategory(cat);
            }

            ClientAccount _account = new ClientAccount
            {
                ClientID = _newSale.ClientID,
                Date = _newSale.Date,
                RegistrationDate = _dt,
                Statement = "فاتورة مبيعات رقم " + _saleID,
                Credit = _newSale.Paid,
                Debit = _newSale.PriceAfterDiscount + _newSale.TransportCost
            };
            _clientAccountServ.AddAccount(_account);

            if (_newSale.Paid > 0)
            {
                Safe _safe = new Safe
                {
                    Date = _newSale.Date,
                    RegistrationDate = _dt,
                    Statement = "فاتورة مبيعات رقم " + _saleID + " للعميل: " + _selectedClient.Name,
                    Amount = _newSale.Paid,
                    Source = 4
                };
                _safeServ.AddSafe(_safe);
            }


            DS ds = new DS();
            ds.Sale.Rows.Clear();
            int i = 0;
            foreach (var item in _saleCategories)
            {
                ds.Sale.Rows.Add();
                ds.Sale[i]["ID"] = _saleID;
                ds.Sale[i]["Date"] = _newSale.Date;
                ds.Sale[i]["Client"] = _selectedClient.Name;
                ds.Sale[i]["Serial"] = i + 1;
                ds.Sale[i]["Category"] = item.Category + " " + item.Company;
                ds.Sale[i]["Qty"] = item.Qty;
                ds.Sale[i]["Price"] = Math.Round(Convert.ToDecimal(item.PriceAfterDiscount), 2);
                ds.Sale[i]["TotalPrice"] = Math.Round(Convert.ToDecimal(item.PriceTotalAfterDiscount), 2);
                ds.Sale[i]["BillPrice"] = Math.Round(Convert.ToDecimal(_newSale.PriceAfterDiscount), 2); ;
                ds.Sale[i]["OldDebt"] = Math.Abs(Convert.ToDecimal(_newSale.OldDebt));
                ds.Sale[i]["TransportCost"] = _newSale.TransportCost;
                ds.Sale[i]["BillTotal"] = Math.Abs(Math.Round(Convert.ToDecimal(_newSale.PriceTotal), 2));
                ds.Sale[i]["Paid"] = _newSale.Paid;
                ds.Sale[i]["NewDebt"] = Math.Abs(Math.Round(Convert.ToDecimal(_newSale.NewDebt), 2));
                if(_newSale.NewDebt > 0)
                    ds.Sale[i]["Type"] = "له";
                else if (_newSale.NewDebt < 0)
                    ds.Sale[i]["Type"] = "عليه";
                ds.Sale[i]["Salesperson"] = _selectedSalesperson.Name;
                ds.Sale[i]["PrintingMan"] = _newSale.PrintingMan;

                if (_newSale.OldDebt > 0)
                    ds.Sale[i]["Type2"] = "له";
                else if (_newSale.OldDebt < 0)
                    ds.Sale[i]["Type2"] = "عليه";
                ds.Sale[i]["Address"] = _selectedClient.Address;
                try
                {
                    ToWord toWord = new ToWord(Math.Abs(Convert.ToDecimal(_newSale.NewDebt)), currencies[0]);
                    ds.Sale[i]["NewDebtAR"] = toWord.ConvertToArabic();
                }
                catch
                {
                    ds.Sale[i]["NewDebtAR"] = string.Empty;
                }
                i++;
            }
            ReportWindow rpt = new ReportWindow();
            SaleReport saleRPT = new SaleReport();
            saleRPT.SetDataSource(ds.Tables["Sale"]);
            rpt.crv.ViewerCore.ReportSource = saleRPT;
            Mouse.OverrideCursor = null;
            _currentWindow.Hide();
            rpt.ShowDialog();
            NewSale = new Sale();
            NewSaleCategory = new SaleCategoryVM();
            SaleCategories = new ObservableCollection<SaleCategoryVM>();
            NewSale.Date = DateTime.Now;
            OldPrices = new ObservableCollection<SaleCategory>();
            _currentWindow.ShowDialog();
        }
        private bool CanExecuteSave()
        {
            try
            {
                if (NewSale.HasErrors || SelectedClient == null || SelectedSalesperson == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        // Categories Show

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
            NewSaleCategory = new SaleCategoryVM();
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
            NewSaleCategory.Category = SelectedCategory.Name;
            NewSaleCategory.CategoryID = SelectedCategory.ID;
            NewSaleCategory.Company = SelectedCategory.Company.Name;
            NewSaleCategory.Cost = SelectedCategory.Cost;
            NewSaleCategory.Price = SelectedCategory.Price;
            await _currentWindow.HideMetroDialogAsync(_categoriesDialog);
            if (SelectedClient != null)
                OldPrices = new ObservableCollection<SaleCategory>(_saleCategoryServ.GetOldPrices(_newSaleCategory.CategoryID, _selectedClient.ID));
            else
                OldPrices = new ObservableCollection<SaleCategory>();
        }

        // Show Category Detials

        private RelayCommand _showDetials;
        public RelayCommand ShowDetials
        {
            get
            {
                return _showDetials
                    ?? (_showDetials = new RelayCommand(ShowDetialsMethodAsync));
            }
        }
        private async void ShowDetialsMethodAsync()
        {
            _categoryDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_categoryDialog);
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
                case "Category":
                    await _currentWindow.HideMetroDialogAsync(_categoryDialog);
                    break;
                default:
                    break;
            }

        }

    }
}
