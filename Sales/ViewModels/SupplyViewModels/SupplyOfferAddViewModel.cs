using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Reports;
using Sales.Services;
using Sales.Views.SupplyViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sales.ViewModels.SupplyViewModels
{
    public class SupplyOfferAddViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly CategoriesShowDialog _categoriesDialog;
        ClientServices _clientServ = new ClientServices();
        SupplyCategoryServices _supplyCategoryServ = new SupplyCategoryServices();
        SupplyOfferServices _supplyOfferServ = new SupplyOfferServices();
        CategoryServices _categoryServ = new CategoryServices();
        SupplyOfferCategoryServices _supplyOfferCategoryServ = new SupplyOfferCategoryServices();

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

        public SupplyOfferAddViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _categoriesDialog = new CategoriesShowDialog();
            Clients = new ObservableCollection<Client>(_clientServ.GetClients());
            NewSupplyOffer.Date = DateTime.Now;
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

        private SupplyOffer _newSupplyOffer = new SupplyOffer();
        public SupplyOffer NewSupplyOffer
        {
            get { return _newSupplyOffer; }
            set { SetProperty(ref _newSupplyOffer, value); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        private SupplyCategory _selectedOldCost;
        public SupplyCategory SelectedOldCost
        {
            get { return _selectedOldCost; }
            set { SetProperty(ref _selectedOldCost, value); }
        }

        private SupplyOfferCategoryVM _newSupplyOfferCategory;
        public SupplyOfferCategoryVM NewSupplyOfferCategory
        {
            get { return _newSupplyOfferCategory; }
            set { SetProperty(ref _newSupplyOfferCategory, value); }
        }

        private SupplyOfferCategoryVM _selectedSupplyOfferCategory;
        public SupplyOfferCategoryVM SelectedSupplyOfferCategory
        {
            get { return _selectedSupplyOfferCategory; }
            set { SetProperty(ref _selectedSupplyOfferCategory, value); }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<SupplyCategory> _oldCosts = new ObservableCollection<SupplyCategory>();
        public ObservableCollection<SupplyCategory> OldCosts
        {
            get { return _oldCosts; }
            set { SetProperty(ref _oldCosts, value); }
        }

        private ObservableCollection<SupplyOfferCategoryVM> _supplyOfferCategories = new ObservableCollection<SupplyOfferCategoryVM>();
        public ObservableCollection<SupplyOfferCategoryVM> SupplyOfferCategories
        {
            get { return _supplyOfferCategories; }
            set { SetProperty(ref _supplyOfferCategories, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        // Bill

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
                if (NewSupplyOfferCategory != null)
                    OldCosts = new ObservableCollection<SupplyCategory>(_supplyCategoryServ.GetOldCosts(_newSupplyOfferCategory.CategoryID, _selectedClient.ID));
                else
                    OldCosts = new ObservableCollection<SupplyCategory>();
            }
            catch
            {
                OldCosts = new ObservableCollection<SupplyCategory>();
            }
        }

        private RelayCommand _selectCost;
        public RelayCommand SelectCost
        {
            get
            {
                return _selectCost ?? (_selectCost = new RelayCommand(
                    ExecuteSelectCost));
            }
        }
        private void ExecuteSelectCost()
        {
            if (SelectedOldCost == null)
                return;
            NewSupplyOfferCategory.Cost = _selectedOldCost.Cost;
            NewSupplyOfferCategory.Tax = _selectedOldCost.Tax;
            NewSupplyOfferCategory.Discount = _selectedOldCost.Discount;
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
            if (NewSupplyOfferCategory.CostTotalAfterDiscount == null || NewSupplyOfferCategory.Category == null)
                return;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            };
            if (_supplyOfferCategories.SingleOrDefault(s => s.CategoryID == _newSupplyOfferCategory.CategoryID) != null)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", "هذا الصنف موجود مسبقاً فى الطلبيه", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            _supplyOfferCategories.Add(_newSupplyOfferCategory);
            NewSupplyOfferCategory = new SupplyOfferCategoryVM();
            NewSupplyOffer.Cost = SupplyOfferCategories.Sum(s => s.CostTotal);
            NewSupplyOffer.CostAfterTax = SupplyOfferCategories.Sum(s => s.CostTotalAfterTax);
            NewSupplyOffer.TotalDiscount = SupplyOfferCategories.Sum(s => s.DiscountValueTotal);
            OldCosts = new ObservableCollection<SupplyCategory>();
        }
        private bool CanExecuteAddToBill()
        {
            try
            {
                return !NewSupplyOfferCategory.HasErrors;
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
                _supplyOfferCategories.Remove(_selectedSupplyOfferCategory);
                NewSupplyOffer.Cost = SupplyOfferCategories.Sum(s => s.CostTotal);
                NewSupplyOffer.CostAfterTax = SupplyOfferCategories.Sum(s => s.CostTotalAfterTax);
                NewSupplyOffer.TotalDiscount = SupplyOfferCategories.Sum(s => s.DiscountValueTotal);
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
            NewSupplyOfferCategory = SelectedSupplyOfferCategory;
            _supplyOfferCategories.Remove(_selectedSupplyOfferCategory);
            NewSupplyOffer.Cost = SupplyOfferCategories.Sum(s => s.CostTotal);
            NewSupplyOffer.CostAfterTax = SupplyOfferCategories.Sum(s => s.CostTotalAfterTax);
            NewSupplyOffer.TotalDiscount = SupplyOfferCategories.Sum(s => s.DiscountValueTotal);
            if (SelectedClient != null)
                OldCosts = new ObservableCollection<SupplyCategory>(_supplyCategoryServ.GetOldCosts(_newSupplyOfferCategory.CategoryID, _selectedClient.ID));
            else
                OldCosts = new ObservableCollection<SupplyCategory>();
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
            _supplyOfferServ.AddSupplyOffer(_newSupplyOffer);
            int _supplyOfferID = _supplyOfferServ.GetLastSupplyOfferID();

            foreach (var item in _supplyOfferCategories)
            {
                SupplyOfferCategory _supplyOfferCategory = new SupplyOfferCategory
                {
                    CategoryID = item.CategoryID,
                    Cost = item.Cost,
                    CostAfterDiscount = item.CostAfterDiscount,
                    CostAfterTax = item.CostAfterTax,
                    CostTotal = item.CostTotal,
                    CostTotalAfterDiscount = item.CostTotalAfterDiscount,
                    CostTotalAfterTax = item.CostTotalAfterTax,
                    Discount = item.Discount,
                    DiscountValue = item.DiscountValue,
                    DiscountValueTotal = item.DiscountValueTotal,
                    SupplyOfferID = _supplyOfferID,
                    Qty = item.Qty,
                    Tax = item.Tax,
                    TaxValue = item.TaxValue,
                    TaxValueTotal = item.TaxValueTotal
                };
                _supplyOfferCategoryServ.AddSupplyOfferCategory(_supplyOfferCategory);
            }
            DS ds = new DS();
            ds.Sale.Rows.Clear();
            int i = 0;
            foreach (var item in _supplyOfferCategories)
            {
                ds.Sale.Rows.Add();
                ds.Sale[i]["Client"] = _selectedClient.Name;
                ds.Sale[i]["Serial"] = i + 1;
                ds.Sale[i]["Category"] = item.Category + " " + item.Company;
                ds.Sale[i]["Qty"] = item.Qty;
                ds.Sale[i]["Price"] = Math.Round(Convert.ToDecimal(item.CostAfterTax), 2);
                ds.Sale[i]["TotalPrice"] = Math.Round(Convert.ToDecimal(item.CostTotalAfterTax), 2);
                ds.Sale[i]["BillPrice"] = Math.Round(Convert.ToDecimal(_newSupplyOffer.CostAfterTax), 2);
                i++;
            }
            ReportWindow rpt = new ReportWindow();
            SupplyOfferReport supplyOfferRPT = new SupplyOfferReport();
            supplyOfferRPT.SetDataSource(ds.Tables["Sale"]);
            rpt.crv.ViewerCore.ReportSource = supplyOfferRPT;
            Mouse.OverrideCursor = null;
            _currentWindow.Hide();
            rpt.ShowDialog();

            NewSupplyOffer = new SupplyOffer();
            NewSupplyOfferCategory = new SupplyOfferCategoryVM();
            SupplyOfferCategories = new ObservableCollection<SupplyOfferCategoryVM>();
            NewSupplyOffer.Date = DateTime.Now;
            OldCosts = new ObservableCollection<SupplyCategory>();
            _currentWindow.ShowDialog();
        }
        private bool CanExecuteSave()
        {
            return !NewSupplyOffer.HasErrors;
        }

        // Categories

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
            OldCosts = new ObservableCollection<SupplyCategory>();
            NewSupplyOfferCategory = new SupplyOfferCategoryVM();
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
            NewSupplyOfferCategory.Category = SelectedCategory.Name;
            NewSupplyOfferCategory.CategoryID = SelectedCategory.ID;
            NewSupplyOfferCategory.Company = SelectedCategory.Company.Name;
            NewSupplyOfferCategory.Cost = SelectedCategory.Cost;
            await _currentWindow.HideMetroDialogAsync(_categoriesDialog);
            if (SelectedClient != null)
                OldCosts = new ObservableCollection<SupplyCategory>(_supplyCategoryServ.GetOldCosts(_newSupplyOfferCategory.CategoryID, _selectedClient.ID));
            else
                OldCosts = new ObservableCollection<SupplyCategory>();
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
