using MahApps.Metro.Controls;
using Sales.Helpers;
using Sales.Views.ClientViews;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Sales.Models;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Sales.Reports;

namespace Sales.ViewModels.ClientViewModels
{
    public class ClientDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly ClientAddDialog _clientAddDialog;
        private readonly ClientUpdateDialog _clientUpdateDialog;
        private readonly ClientAccountShowDialog _clientAccountShowDialog;

        ClientServices _clientServ = new ClientServices();
        ClientAccountServices _accountServ = new ClientAccountServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _clientServ.GetClientsNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_clientServ.GetClientsNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            Clients = new ObservableCollection<ClientVM>(_clientServ.SearchClients(_key, _currentPage));
        }

        public ClientDisplayViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _clientAddDialog = new ClientAddDialog();
            _clientUpdateDialog = new ClientUpdateDialog();
            _clientAccountShowDialog = new ClientAccountShowDialog();
            _namesSuggestions = _clientServ.GetNamesSuggetions();
            _addressSuggestions = _clientServ.GetAddressSuggetions();
            Load();
        }

        private bool _isFocused = true;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set { SetProperty(ref _canEdit, value); }
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

        private bool _accountStartType;
        public bool AccountStartType
        {
            get { return _accountStartType; }
            set { SetProperty(ref _accountStartType, value); }
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

        private decimal? _accountStart;
        [Required(ErrorMessage = "رصيد بداية المدة مطلوب")]
        public decimal? AccountStart
        {
            get { return _accountStart; }
            set { SetProperty(ref _accountStart, value); }
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

        private string _isDiscount;
        public string IsDiscount
        {
            get { return _isDiscount; }
            set { SetProperty(ref _isDiscount, value); }
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

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private ClientVM _selectedClientVM;
        public ClientVM SelectedClientVM
        {
            get { return _selectedClientVM; }
            set { SetProperty(ref _selectedClientVM, value); }
        }

        private Client _newClient = new Client();
        public Client NewClient
        {
            get { return _newClient; }
            set { SetProperty(ref _newClient, value); }
        }

        private ClientAccountVM _selectedClientAccount;
        public ClientAccountVM SelectedClientAccount
        {
            get { return _selectedClientAccount; }
            set { SetProperty(ref _selectedClientAccount, value); }
        }

        private List<string> _namesSuggestions = new List<string>();
        public List<string> NamesSuggestions
        {
            get { return _namesSuggestions; }
            set { SetProperty(ref _namesSuggestions, value); }
        }

        private List<string> _addressSuggestions = new List<string>();
        public List<string> AddressSuggestions
        {
            get { return _addressSuggestions; }
            set { SetProperty(ref _addressSuggestions, value); }
        }

        private ObservableCollection<ClientVM> _clients;
        public ObservableCollection<ClientVM> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<ClientAccount> _clientAccounts;
        public ObservableCollection<ClientAccount> ClientAccounts
        {
            get { return _clientAccounts; }
            set
            {
                SetProperty(ref _clientAccounts, value);
                OnPropertyChanged("DuringAccount");
                OnPropertyChanged("NotDuringAccount");
            }
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
            Clients = new ObservableCollection<ClientVM>(_clientServ.SearchClients(_key, _currentPage));
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
            Clients = new ObservableCollection<ClientVM>(_clientServ.SearchClients(_key, _currentPage));
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
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا العميل؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _selectedClient = _clientServ.GetClient(_selectedClientVM.ID);
                _clientServ.DeleteClient(_selectedClient);
                Load();
            }
        }

        // Add Client

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
            _clientAddDialog.DataContext = this;
            AccountStart = null;
            NewClient = new Client();
            await _currentWindow.ShowMetroDialogAsync(_clientAddDialog);
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
            if (AccountStart == null || NewClient.Name == null)
                return;

            if (_clientServ.GetClient(_newClient.Name) != null)
            {
                await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالعميل موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
            }
            else
            {
                if (_accountStartType == true)
                    _newClient.AccountStart = _accountStart;
                else
                    _newClient.AccountStart = -_accountStart;
                _clientServ.AddClient(_newClient);
                _namesSuggestions.Add(_newClient.Name);
                _addressSuggestions.Add(_newClient.Address);
                NewClient = new Client();
                AccountStart = null;
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
            return !NewClient.HasErrors && !HasErrors;
        }

        // Show Account

        private RelayCommand _showAccount;
        public RelayCommand ShowAccount
        {
            get
            {
                return _showAccount
                    ?? (_showAccount = new RelayCommand(ShowAccountMethod));
            }
        }
        private async void ShowAccountMethod()
        {
            DateFrom = _accountServ.GetFirstDate(_selectedClientVM.ID);
            DateTo = _accountServ.GetLastDate(_selectedClientVM.ID);
            GetClientAccountsMethod();
            IsDiscount = _accountServ.IsDiscount(_selectedClientVM.ID);
            _clientAccountShowDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_clientAccountShowDialog);
        }

        private RelayCommand _getClientAccounts;
        public RelayCommand GetClientAccounts
        {
            get
            {
                return _getClientAccounts
                    ?? (_getClientAccounts = new RelayCommand(GetClientAccountsMethod));
            }
        }
        private void GetClientAccountsMethod()
        {
            SelectedClientAccount = _accountServ.GetAccountInfo(_selectedClientVM.ID, _dateFrom, _dateTo);
            ClientAccounts = new ObservableCollection<ClientAccount>(_accountServ.GetClientAccounts(_selectedClientVM.ID, _dateFrom, _dateTo));
        }

        private RelayCommand _print;
        public RelayCommand Print
        {
            get
            {
                return _print
                    ?? (_print = new RelayCommand(PrintMethod));
            }
        }
        private void PrintMethod()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DS ds = new DS();
            ds.ClientAccount.Rows.Clear();
            int i = 0;
            foreach (var item in _clientAccounts)
            {
                ds.ClientAccount.Rows.Add();
                ds.ClientAccount[i]["Serial"] = i + 1;
                ds.ClientAccount[i]["Client"] = _selectedClientVM.Name;
                ds.ClientAccount[i]["DateFrom"] = _dateFrom;
                ds.ClientAccount[i]["DateTo"] = _dateTo;
                ds.ClientAccount[i]["Date"] = item.Date;
                ds.ClientAccount[i]["Statement"] = item.Statement;
                ds.ClientAccount[i]["Debit"] = item.Debit;
                ds.ClientAccount[i]["Credit"] = item.Credit;
                ds.ClientAccount[i]["Details"] = item.Details;
                ds.ClientAccount[i]["TotalDebit"] = _selectedClientAccount.WithoutTotalDebit;
                ds.ClientAccount[i]["TotalCredit"] = _selectedClientAccount.WithoutTotalCredit;
                ds.ClientAccount[i]["DuringAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.WithoutDuringAccount));
                if (_selectedClientAccount.WithoutDuringAccount > 0)
                    ds.ClientAccount[i]["DuringAccountType"] = "له";
                else if (_selectedClientAccount.WithoutDuringAccount < 0)
                    ds.ClientAccount[i]["DuringAccountType"] = "عليه";
                ds.ClientAccount[i]["NotDuringAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.WithoutOldAccount));
                if (_selectedClientAccount.WithoutOldAccount > 0)
                    ds.ClientAccount[i]["NotDuringAccountType"] = "له";
                else if (_selectedClientAccount.WithoutOldAccount < 0)
                    ds.ClientAccount[i]["NotDuringAccountType"] = "عليه";
                ds.ClientAccount[i]["CurrentAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.WithoutCurrentAccount));
                if (_selectedClientAccount.WithoutCurrentAccount > 0)
                    ds.ClientAccount[i]["CurrentAccountType"] = "له";
                else if (_selectedClientAccount.WithoutCurrentAccount < 0)
                    ds.ClientAccount[i]["CurrentAccountType"] = "عليه";

                if (IsDiscount == "Visible")
                {
                    ds.ClientAccount[i]["DiscountTotalDebit"] = _selectedClientAccount.DiscountTotalDebit;
                    ds.ClientAccount[i]["DiscountTotalCredit"] = _selectedClientAccount.DiscountTotalCredit;
                    ds.ClientAccount[i]["DiscountDuringAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.DiscountDuringAccount));
                    if (_selectedClientAccount.DiscountDuringAccount > 0)
                        ds.ClientAccount[i]["DiscountDuringAccountType"] = "له";
                    else if (_selectedClientAccount.DiscountDuringAccount < 0)
                        ds.ClientAccount[i]["DiscountDuringAccountType"] = "عليه";
                    ds.ClientAccount[i]["DiscountOldAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.DiscountOldAccount));
                    if (_selectedClientAccount.DiscountOldAccount > 0)
                        ds.ClientAccount[i]["DiscountOldAccountType"] = "له";
                    else if (_selectedClientAccount.DiscountOldAccount < 0)
                        ds.ClientAccount[i]["DiscountOldAccountType"] = "عليه";
                    ds.ClientAccount[i]["DiscountCurrentAccount"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.DiscountCurrentAccount));
                    if (_selectedClientAccount.DiscountCurrentAccount > 0)
                        ds.ClientAccount[i]["DiscountCurrentAccountType"] = "له";
                    else if (_selectedClientAccount.DiscountCurrentAccount < 0)
                        ds.ClientAccount[i]["DiscountCurrentAccountType"] = "عليه";
                    ds.ClientAccount[i]["Account"] = Math.Abs(Convert.ToDecimal(_selectedClientAccount.CurrentAccount));
                    if (_selectedClientAccount.CurrentAccount > 0)
                        ds.ClientAccount[i]["AccountType"] = "له";
                    else if (_selectedClientAccount.CurrentAccount < 0)
                        ds.ClientAccount[i]["AccountType"] = "عليه";
                }
                i++;
            }
            ReportWindow rpt = new ReportWindow();

            if (IsDiscount == "Visible")
            {
                ClientAccountReportDiscount accountClientRPT = new ClientAccountReportDiscount();
                accountClientRPT.SetDataSource(ds.Tables["ClientAccount"]);
                rpt.crv.ViewerCore.ReportSource = accountClientRPT;
                Mouse.OverrideCursor = null;
                rpt.ShowDialog();
            }
            else
            {
                ClientAccountReport accountClientRPT = new ClientAccountReport();
                accountClientRPT.SetDataSource(ds.Tables["ClientAccount"]);
                rpt.crv.ViewerCore.ReportSource = accountClientRPT;
                Mouse.OverrideCursor = null;
                rpt.ShowDialog();
            }

        }

        // Update Account

        private RelayCommand _showUpdate;
        public RelayCommand ShowUpdate
        {
            get
            {
                return _showUpdate
                    ?? (_showUpdate = new RelayCommand(ShowUpdateMethod));
            }
        }
        private async void ShowUpdateMethod()
        {
            _selectedClient = _clientServ.GetClient(_selectedClientVM.ID);

            if (_selectedClient.ClientAccounts.Count > 0)
                CanEdit = false;
            else
                CanEdit = true;

            AccountStart = Math.Abs(Convert.ToDecimal(_selectedClient.AccountStart));
            if (_selectedClient.AccountStart > 0)
                AccountStartType = true;
            else
                AccountStartType = false;
            _clientUpdateDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_clientUpdateDialog);
        }

        private RelayCommand _update;
        public RelayCommand Update
        {
            get
            {
                return _update ?? (_update = new RelayCommand(
                    ExecuteUpdateAsync,
                    CanExecuteUpdate));
            }
        }
        private async void ExecuteUpdateAsync()
        {
            if (AccountStart == null)
                return;

            if (_accountStartType == true)
                _selectedClient.AccountStart = _accountStart;
            else
                _selectedClient.AccountStart = -_accountStart;
            _clientServ.UpdateClient(_selectedClient);
            _namesSuggestions.Add(_selectedClient.Name);
            await _currentWindow.HideMetroDialogAsync(_clientUpdateDialog);
            Load();
        }
        private bool CanExecuteUpdate()
        {
            try
            {
                return !SelectedClient.HasErrors && !HasErrors;
            }
            catch
            {
                return false;
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
                case "Add":
                    await _currentWindow.HideMetroDialogAsync(_clientAddDialog);
                    Load();
                    break;
                case "Update":
                    await _currentWindow.HideMetroDialogAsync(_clientUpdateDialog);
                    Load();
                    break;
                case "AccountShow":
                    await _currentWindow.HideMetroDialogAsync(_clientAccountShowDialog);
                    break;
                default:
                    break;
            }

        }
    }
}
