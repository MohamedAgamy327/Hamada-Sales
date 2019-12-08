using Sales.Helpers;
using System;
using Sales.Services;
using System.Collections.ObjectModel;
using Sales.Models;
using GalaSoft.MvvmLight.CommandWpf;
using Sales.Views.SupplyViews;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Sales.ViewModels.SupplyViewModels
{
    public class SupplyDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly SupplyPremiumAddDialog _supplyPremiumAddDialog;

        SafeServices _safeServ = new SafeServices();
        SupplyServices _supplyServ = new SupplyServices();
        CategoryServices _categoryServ = new CategoryServices();
        SupplyFutureServices _supplyFutureServ = new SupplyFutureServices();
        SupplyPremiumServices _supplyPremiumServ = new SupplyPremiumServices();
        ClientAccountServices _clientAccountServ = new ClientAccountServices();
        SupplyCategoryServices _supplyCategoryServ = new SupplyCategoryServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _supplyServ.GetSuppliesNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_supplyServ.GetSuppliesNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            Supplies = new ObservableCollection<Supply>(_supplyServ.SearchSupplies(_key, _currentPage));
        }

        public SupplyDisplayViewModel()
        {
            Load();
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _supplyPremiumAddDialog = new SupplyPremiumAddDialog();
            _statements.Add(new StatementVM { ID = 1, Statement = "دفع" });
            _statements.Add(new StatementVM { ID = 2, Statement = "تسوية" });
            PlacesSuggestions = _supplyFutureServ.GetPlacesSuggetions();

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

        private string _visibility;
        public string Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        private string _paidType;
        public string PaidType
        {
            get { return _paidType; }
            set { SetProperty(ref _paidType, value); }
        }

        private StatementVM _selectedstatement = new StatementVM();
        public StatementVM SelectedStatement
        {
            get { return _selectedstatement; }
            set { SetProperty(ref _selectedstatement, value); }
        }

        private Supply _selectedSupply;
        public Supply SelectedSupply
        {
            get { return _selectedSupply; }
            set { SetProperty(ref _selectedSupply, value); }
        }

        private SupplyPremium _newPremium;
        public SupplyPremium NewPremium
        {
            get { return _newPremium; }
            set { SetProperty(ref _newPremium, value); }
        }

        private SupplyPremium _selectedPremium;
        public SupplyPremium SelectedPremium
        {
            get { return _selectedPremium; }
            set { SetProperty(ref _selectedPremium, value); }
        }

        private List<string> _placesSuggestions = new List<string>();
        public List<string> PlacesSuggestions
        {
            get { return _placesSuggestions; }
            set { SetProperty(ref _placesSuggestions, value); }
        }

        private ObservableCollection<StatementVM> _statements = new ObservableCollection<StatementVM>();
        public ObservableCollection<StatementVM> Statements
        {
            get { return _statements; }
            set { SetProperty(ref _statements, value); }
        }

        private ObservableCollection<SupplyPremium> _supplyPremiums;
        public ObservableCollection<SupplyPremium> SupplyPremiums
        {
            get { return _supplyPremiums; }
            set { SetProperty(ref _supplyPremiums, value); }
        }

        private ObservableCollection<Supply> _supplies;
        public ObservableCollection<Supply> Supplies
        {
            get { return _supplies; }
            set { SetProperty(ref _supplies, value); }
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
            Supplies = new ObservableCollection<Supply>(_supplyServ.SearchSupplies(_key, _currentPage));
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
            Supplies = new ObservableCollection<Supply>(_supplyServ.SearchSupplies(_key, _currentPage));
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
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذه الفاتورة؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _safeServ.DeleteSafe(_selectedSupply.RegistrationDate);
                _clientAccountServ.DeleteAccount(_selectedSupply.RegistrationDate);
                var supplyCategories = _supplyCategoryServ.GetSupplyCategories(_selectedSupply.ID);
                foreach (var item in supplyCategories)
                {
                    Category cat = _categoryServ.GetCategory(item.CategoryID);
                    if (cat.Qty - item.Qty != 0)
                        cat.Cost = ((cat.Cost * cat.Qty) - item.CostTotalAfterDiscount) / (cat.Qty - item.Qty);
                    cat.Qty = cat.Qty - item.Qty;
                    _categoryServ.UpdateCategory(cat);
                }
                _supplyServ.DeleteSupply(_selectedSupply);
                Load();
            }
        }

        // Update Bill

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
            SupplyUpdateViewModel.ID = _selectedSupply.ID;
            _currentWindow.Hide();
            new SupplyUpdateWindow().ShowDialog();
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
            new SupplyAddWindow().ShowDialog();
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
            SupplyShowViewModel.ID = _selectedSupply.ID;
            //_currentWindow.Hide();
            new SupplyShowWindow().ShowDialog();
            //Load();
            //_currentWindow.ShowDialog();
        }

        // Premium

        private RelayCommand _payPremium;
        public RelayCommand PayPremium
        {
            get
            {
                return _payPremium
                    ?? (_payPremium = new RelayCommand(PayPremiumMethod));
            }
        }
        private async void PayPremiumMethod()
        {
            _supplyPremiumAddDialog.DataContext = this;
            if (SelectedSupply.SupplyFuture.Change == 0)
                Visibility = "Collapsed";
            else
                Visibility = "Visible";
            if (_selectedSupply.SupplyFuture.Cheque == true)
                PaidType = "نوع الدفع: شيك  رقم الشيك: " + _selectedSupply.SupplyFuture.ChequeNumber;
            else
                PaidType = "نوع الدفع: كاش";
            SupplyPremiums = new ObservableCollection<SupplyPremium>(_supplyPremiumServ.GetPremiums(_selectedSupply.ID));
            NewPremium = new SupplyPremium();
            NewPremium.Date = DateTime.Now;
            SelectedStatement = null;
            await _currentWindow.ShowMetroDialogAsync(_supplyPremiumAddDialog);
        }

        private RelayCommand _addPremium;
        public RelayCommand AddPremium
        {
            get
            {
                return _addPremium
                    ?? (_addPremium = new RelayCommand(ExecuteAddPremiumAsync, CanExecuteAddPremium));
            }
        }
        private async void ExecuteAddPremiumAsync()
        {
            if (NewPremium.Amount == null || SelectedStatement == null)
                return;
            if (_newPremium.Amount > _selectedSupply.SupplyFuture.Change)
                return;
            DateTime _dt = DateTime.Now;
            _newPremium.RegistrationDate = _dt;
            _newPremium.SupplyID = _selectedSupply.ID;
            _newPremium.Statement = _selectedstatement.Statement;
            _supplyPremiumServ.AddPremium(_newPremium);

            if (_selectedstatement.Statement == "دفع")
            {
                ClientAccount _account = new ClientAccount
                {
                    ClientID = _selectedSupply.ClientID,
                    Date = _newPremium.Date,
                    RegistrationDate = _dt,
                    Statement = "دفع قسط فاتورة المشتريات رقم  " + _selectedSupply.ID,
                    Credit = 0,
                    Debit = _newPremium.Amount
                };
                _clientAccountServ.AddAccount(_account);
                Safe _safe = new Safe
                {
                    Date = _newPremium.Date,
                    RegistrationDate = _dt,
                    Statement = "دفع قسط فاتورة المشتريات رقم  " + _selectedSupply.ID + " للعميل: " + _selectedSupply.Client.Name,
                    Amount = -_newPremium.Amount,
                    Source = 9
                };
                _safeServ.AddSafe(_safe);
            }
            else
            {
                ClientAccount _account = new ClientAccount
                {
                    ClientID = _selectedSupply.ClientID,
                    Date = _newPremium.Date,
                    RegistrationDate = _dt,
                    Statement = "تسوية قسط فاتورة المشتريات رقم  " + _selectedSupply.ID,
                    Credit = 0,
                    Debit = _newPremium.Amount
                };
                _clientAccountServ.AddAccount(_account);
            }
            var supplyFuture = _supplyFutureServ.GetSupplyFuture(_selectedSupply.ID);
            supplyFuture.Change -= _newPremium.Amount;
            _supplyFutureServ.UpdateSupplyFuture(supplyFuture);
            SelectedSupply = _supplyServ.GetSupply(_selectedSupply.ID);
            _placesSuggestions.Add(_newPremium.Place);
            NewPremium = new SupplyPremium();
            NewPremium.Date = DateTime.Now;
            SelectedStatement = null;
            SupplyPremiums = new ObservableCollection<SupplyPremium>(_supplyPremiumServ.GetPremiums(_selectedSupply.ID));
            if (SelectedSupply.SupplyFuture.Change == 0)
                Visibility = "Collapsed";
            else
                Visibility = "Visible";
            await _currentWindow.ShowMessageAsync("نجاح الإضافة", "تم الإضافة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
        }
        private bool CanExecuteAddPremium()
        {
            try
            {
                if (NewPremium.HasErrors || SelectedStatement == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private RelayCommand _deletePremium;
        public RelayCommand DeletePremium
        {
            get
            {
                return _deletePremium
                    ?? (_deletePremium = new RelayCommand(DeletePremiumMethod));
            }
        }
        private async void DeletePremiumMethod()
        {
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا القسط؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _supplyPremiumServ.DeletePremium(_selectedPremium);
                _clientAccountServ.DeleteAccount(_selectedPremium.RegistrationDate);
                _safeServ.DeleteSafe(_selectedPremium.RegistrationDate);
                var supplyFuture = _supplyFutureServ.GetSupplyFuture(_selectedSupply.ID);
                supplyFuture.Change += _selectedPremium.Amount;
                _supplyFutureServ.UpdateSupplyFuture(supplyFuture);
                SelectedSupply = _supplyServ.GetSupply(_selectedSupply.ID);
                SupplyPremiums = new ObservableCollection<SupplyPremium>(_supplyPremiumServ.GetPremiums(_selectedSupply.ID));
                if (SelectedSupply.SupplyFuture.Change == 0)
                    Visibility = "Collapsed";
                else
                    Visibility = "Visible";
            }
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
                case "Premium":
                    await _currentWindow.HideMetroDialogAsync(_supplyPremiumAddDialog);
                    Load();
                    break;
                default:
                    break;
            }

        }
    }
}
