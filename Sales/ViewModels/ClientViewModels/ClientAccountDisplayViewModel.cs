using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sales.Helpers;
using Sales.Models;
using Sales.Services;
using Sales.Views.ClientViews;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Sales.ViewModels.ClientViewModels
{
    public class ClientAccountDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly ClientAccountAddDialog _clientAccountAddDialog;

        SafeServices _safeServ = new SafeServices();
        ClientServices _clientServ = new ClientServices();
        ClientAccountServices _clientAccountServ = new ClientAccountServices();

        private void Load()
        {
            CurrentPage = 1;
            ISFirst = false;
            TotalRecords = _clientAccountServ.GetClientsAccountsNumer(Key);
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)_clientAccountServ.GetClientsAccountsNumer(_key) / 17));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                ISLast = false;
            else
                ISLast = true;
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage));
        }

        public ClientAccountDisplayViewModel()
        {
            Clients = new ObservableCollection<Client>(_clientServ.GetClients());
            _currentWindow = System.Windows.Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _clientAccountAddDialog = new ClientAccountAddDialog();
            _accountStatements.Add(new StatementVM { ID = 1, Statement = "سند دفع" });
            _accountStatements.Add(new StatementVM { ID = 2, Statement = "سند قبض" });
            _accountStatements.Add(new StatementVM { ID = 2, Statement = "دفع شيك" });
            _accountStatements.Add(new StatementVM { ID = 2, Statement = "قبض شيك" });
            _accountStatements.Add(new StatementVM { ID = 3, Statement = "دفع سلف" });
            _accountStatements.Add(new StatementVM { ID = 4, Statement = "قبض سلف" });
            _accountStatements.Add(new StatementVM { ID = 5, Statement = "تسوية إضافة" });
            _accountStatements.Add(new StatementVM { ID = 6, Statement = "تسوية تنزيل" });
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

        private decimal? _amount;
        [Required(ErrorMessage = "المبلغ مطلوب")]
        public decimal? Amount
        {
            get { return _amount; }
            set
            {
                SetProperty(ref _amount, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _oldDebt;
        public decimal? OldDebt
        {
            get { return _oldDebt; }
            set
            {
                SetProperty(ref _oldDebt, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private decimal? _newDebt;
        public decimal? NewDebt
        {
            get
            {
                if (SelectedStatement == null)
                    return _newDebt = null;
                else if (SelectedStatement.Statement == "سند دفع" || SelectedStatement.Statement == "دفع سلف" || SelectedStatement.Statement == "دفع شيك" || SelectedStatement.Statement == "تسوية تنزيل")
                {
                    return _newDebt = OldDebt - Amount;
                }
                else if (SelectedStatement.Statement == "سند قبض" || SelectedStatement.Statement == "قبض شيك" || SelectedStatement.Statement == "قبض سلف" || SelectedStatement.Statement == "تسوية إضافة")
                {
                    return _newDebt = OldDebt + Amount;
                }
                else
                {
                    return _newDebt = null;
                }
            }
            set
            { SetProperty(ref _newDebt, value); }
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

        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private ClientAccount _selectedClientAccount;
        public ClientAccount SelectedClientAccount
        {
            get { return _selectedClientAccount; }
            set { SetProperty(ref _selectedClientAccount, value); }
        }

        private StatementVM _selectedtatement = new StatementVM();
        public StatementVM SelectedStatement
        {
            get { return _selectedtatement; }
            set
            {
                SetProperty(ref _selectedtatement, value);
                OnPropertyChanged("NewDebt");
            }
        }

        private ClientAccount _newClientAccount = new ClientAccount();
        public ClientAccount NewClientAccount
        {
            get { return _newClientAccount; }
            set { SetProperty(ref _newClientAccount, value); }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<StatementVM> _accountStatements = new ObservableCollection<StatementVM>();
        public ObservableCollection<StatementVM> AccountStatements
        {
            get { return _accountStatements; }
            set { SetProperty(ref _accountStatements, value); }
        }

        private ObservableCollection<ClientAccount> _clientsAccounts;
        public ObservableCollection<ClientAccount> ClientsAccounts
        {
            get { return _clientsAccounts; }
            set { SetProperty(ref _clientsAccounts, value); }
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
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage));
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
            ClientsAccounts = new ObservableCollection<ClientAccount>(_clientAccountServ.SearchClientsAccounts(_key, _currentPage));
        }

        private RelayCommand _showPhoto;
        public RelayCommand ShowPhoto
        {
            get
            {
                return _showPhoto
                    ?? (_showPhoto = new RelayCommand(ShowPhotoMethod));
            }
        }
        private void ShowPhotoMethod()
        {
            Process.Start(SelectedClientAccount.Url);
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
            MessageDialogResult result = await _currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا البند؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                NegativeButtonText = "الغاء",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            if (result == MessageDialogResult.Affirmative)
            {
                _clientAccountServ.DeleteAccount(_selectedClientAccount);
                _safeServ.DeleteSafe(_selectedClientAccount.RegistrationDate);
                Load();
            }
        }

        //Add Account

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
            _clientAccountAddDialog.DataContext = this;
            Url = "";
            SelectedStatement = null;
            Amount = null;
            NewClientAccount = new ClientAccount
            {
                Date = DateTime.Now
            };
            await _currentWindow.ShowMetroDialogAsync(_clientAccountAddDialog);
        }

        private RelayCommand _browseFile;
        public RelayCommand BrowseFile
        {
            get
            {
                return _browseFile ?? (_browseFile = new RelayCommand(
                    ExecuteBrowseFile));
            }
        }
        private void ExecuteBrowseFile()
        {
            Url = "";
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "اختيار صورة الوصل",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            ofd.ShowDialog();
            Url = ofd.FileName;
        }

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
                OldDebt = _clientAccountServ.GetClientAccount(_selectedClient.ID) + _selectedClient.AccountStart;
            }
            catch
            {
                OldDebt = null;
            }
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
            if (NewDebt == null)
                return;
            DateTime dt = DateTime.Now;
            _newClientAccount.RegistrationDate = dt;
            _newClientAccount.CanDelete = true;
            _newClientAccount.Statement = SelectedStatement.Statement;
            if (_newClientAccount.Statement == "سند دفع")
            {
                _newClientAccount.Debit = _amount;
                _newClientAccount.Credit = 0;
                Safe safe = new Safe
                {
                    Amount = -_newClientAccount.Debit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "سند دفع للعميل : " + _selectedClient.Name,
                    Source = 5
                };
                _safeServ.AddSafe(safe);
            }

            else if (_newClientAccount.Statement == "دفع شيك")
            {
                _newClientAccount.Debit = _amount;
                _newClientAccount.Credit = 0;
                Safe safe = new Safe
                {
                    Amount = -_newClientAccount.Debit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "دفع شيك للعميل : " + _selectedClient.Name,
                    Source = 7
                };
                _safeServ.AddSafe(safe);
            }

            else if (_newClientAccount.Statement == "دفع سلف")
            {
                _newClientAccount.Debit = _amount;
                _newClientAccount.Credit = 0;
                Safe safe = new Safe
                {
                    Amount = -_newClientAccount.Debit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "دفع سلف للعميل : " + _selectedClient.Name,
                    Source = 7
                };
                _safeServ.AddSafe(safe);
            }
            else if (_newClientAccount.Statement == "تسوية تنزيل")
            {
                _newClientAccount.Debit = _amount;
                _newClientAccount.Credit = 0;
            }
            else if (_newClientAccount.Statement == "سند قبض")
            {
                _newClientAccount.Credit = _amount;
                _newClientAccount.Debit = 0;
                Safe safe = new Safe
                {
                    Amount = _newClientAccount.Credit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "سند قبض من العميل : " + _selectedClient.Name,
                    Source = 6
                };
                _safeServ.AddSafe(safe);
            }
            else if (_newClientAccount.Statement == "قبض شيك")
            {
                _newClientAccount.Credit = _amount;
                _newClientAccount.Debit = 0;
                Safe safe = new Safe
                {
                    Amount = _newClientAccount.Credit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "قبض شيك من العميل : " + _selectedClient.Name,
                    Source = 6
                };
                _safeServ.AddSafe(safe);
            }
            else if (_newClientAccount.Statement == "قبض سلف")
            {
                _newClientAccount.Credit = _amount;
                _newClientAccount.Debit = 0;
                Safe safe = new Safe
                {
                    Amount = _newClientAccount.Credit,
                    Date = _newClientAccount.Date,
                    RegistrationDate = dt,
                    Statement = "قبض سلف من العميل : " + _selectedClient.Name,
                    Source = 8
                };
                _safeServ.AddSafe(safe);
            }
            else if (_newClientAccount.Statement == "تسوية إضافة")
            {
                _newClientAccount.Credit = _amount;
                _newClientAccount.Debit = 0;
            }
            if (!string.IsNullOrEmpty(Url))
            {
                Directory.CreateDirectory(@"E:\صور الشيكات \" + SelectedClient.Name);
                _newClientAccount.Url = @"E:\صور الشيكات \" + SelectedClient.Name + @"\" + DateTime.Now.ToShortDateString().Replace('/', '-') + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-') + Path.GetExtension(Url);
                File.Copy(Url, _newClientAccount.Url, true);
                Url = "";
            }
            _clientAccountServ.AddAccount(_newClientAccount);
            SelectedStatement = null;
            NewClientAccount = new ClientAccount
            {
                Date = DateTime.Now
            };
            Amount = null;

            await _currentWindow.ShowMessageAsync("نجاح الإضافة", "تم الإضافة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
        }
        private bool CanExecuteSave()
        {
            try
            {
                if (NewClientAccount.HasErrors || SelectedClient == null || HasErrors || SelectedStatement == null)
                    return false;
                else
                    return true;
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
                    await _currentWindow.HideMetroDialogAsync(_clientAccountAddDialog);
                    Load();
                    break;
                default:
                    break;
            }

        }
    }
}
