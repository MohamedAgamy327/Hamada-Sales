using GalaSoft.MvvmLight.CommandWpf;
using Sales.Helpers;
using Sales.Views.MainViews;
using Sales.Views.SafeViews;
using Sales.Views.SpendingViews;
using Sales.Views.StoreViews;
using Sales.Views.ClientViews;
using MahApps.Metro.Controls;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Sales.Models;
using System;
using System.Data.Entity;
using System.Windows.Input;
using System.Data.SqlClient;
using Sales.Services;
using System.Data.Entity.Migrations;
using Sales.Views.SupplyViews;
using Sales.Views.SaleViews;
using System.Data.Entity.Infrastructure;
using Sales.Migrations;
using System.IO;

namespace Sales.ViewModels
{
    public class MainViewModel : ValidatableBindableBase
    {
        UserServices userServ = new UserServices();
        SupplyFutureServices supplyFutureServ = new SupplyFutureServices();

        MetroWindow _currentWindow;
        private readonly BackupDialog _backupDialog;
        private readonly RestoreBackupDialog _restoreBackupDialog;
        private readonly UserDialog _userDialog;
        private readonly Views.MainViews.LoginDialog _loginDialog;

        public MainViewModel()
        {
            _currentWindow = System.Windows.Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _backupDialog = new BackupDialog();
            _restoreBackupDialog = new RestoreBackupDialog();
            _userDialog = new UserDialog();
            _loginDialog = new Views.MainViews.LoginDialog();


        }

        private bool _isFocused = true;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private string _url;
        [Required(ErrorMessage = "يجب اختيار المسار")]
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private string _isFuuture;
        public string IsFuuture
        {
            get { return _isFuuture; }
            set { SetProperty(ref _isFuuture, value); }
        }

        // Main

        private User _currentedUser = new User();
        public User CurrentUser
        {
            get { return _currentedUser; }
            set { SetProperty(ref _currentedUser, value); }
        }

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethodAsync));
            }
        }
        private async void NavigateToViewMethodAsync(string destination)
        {
            switch (destination)
            {
                case "Store":
                    _currentWindow.Hide();
                    new StoreWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "Client":
                    _currentWindow.Hide();
                    new ClientWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "Supply":
                    _currentWindow.Hide();
                    new SupplyWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "Sale":
                    _currentWindow.Hide();
                    new SaleWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "Spending":
                    _currentWindow.Hide();
                    new SpendingWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "Safe":
                    _currentWindow.Hide();
                    new SafeWindow().ShowDialog();
                    _currentWindow.Show();
                    break;
                case "User":
                    _userDialog.DataContext = this;
                    CurrentUser = userServ.GetUser();
                    await _currentWindow.ShowMetroDialogAsync(_userDialog);
                    break;
                case "SupplyFuture":
                    new SupplyFutureDisplayWindow().ShowDialog();
                    break;
                case "Backup":
                    _backupDialog.DataContext = this;
                    Url = "";
                    await _currentWindow.ShowMetroDialogAsync(_backupDialog);
                    break;
                case "BackupRestore":
                    _restoreBackupDialog.DataContext = this;
                    Url = "";
                    await _currentWindow.ShowMetroDialogAsync(_restoreBackupDialog);
                    break;
                default:
                    break;
            }
        }

        // Login

        private RelayCommand _showLogin;
        public RelayCommand ShowLogin
        {
            get
            {
                return _showLogin ?? (_showLogin = new RelayCommand(
                    ExecuteShowLoginAsync));
            }
        }
        private async void ExecuteShowLoginAsync()
        {
            try
            {
                using (SalesDB db = new SalesDB())
                {
                    if (!db.Database.Exists())
                    {
                        db.Users.AddOrUpdate(x => x.ID,
                     new User() { ID = "حماده", Password = "222" });
                        db.SaveChanges();
                    }
                    var user = userServ.GetUser();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                var configuration = new Configuration();
                configuration.TargetDatabase = new DbConnectionInfo("Server=.\\sqlexpress;Database=HamadaSalesDB;Trusted_Connection=True;", "System.Data.SqlClient");
                var migrator = new DbMigrator(configuration);
                migrator.Update();
                //_restoreBackupDialog.DataContext = this;
                //await _currentWindow.ShowMetroDialogAsync(_restoreBackupDialog);
                //return;
            }
            _loginDialog.DataContext = this;
            await _currentWindow.ShowMetroDialogAsync(_loginDialog);
        }

        private RelayCommand _signIn;
        public RelayCommand SignIn
        {
            get
            {
                return _signIn ?? (_signIn = new RelayCommand(
                    ExecuteSignInAsync,
                    CanExecuteSignIn));
            }
        }
        private async void ExecuteSignInAsync()
        {
            if (_currentedUser.ID == null || _currentedUser.Password == null)
                return;
            var user = userServ.GetUser();
            if (_currentedUser.ID == user.ID && _currentedUser.Password == user.Password)
            {
                await _currentWindow.HideMetroDialogAsync(_loginDialog);
                var supply = supplyFutureServ.GetSupplyFuture();
                if (supply != null)
                {
                    IsFuuture = "Red";
                    await _currentWindow.ShowMessageAsync("تنبيه", "نرجو مراجعه الفواتير الآجله لدفع المتبقى", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                }
                else
                    IsFuuture = "White";
            }
            else
            {
                await _currentWindow.HideMetroDialogAsync(_loginDialog);
                await _currentWindow.ShowMessageAsync("فشل الدخول", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
                await _currentWindow.ShowMetroDialogAsync(_loginDialog);
            }
        }
        private bool CanExecuteSignIn()
        {
            return !CurrentUser.HasErrors;
        }

        // Backup

        private RelayCommand _browseFloder;
        public RelayCommand BrowseFloder
        {
            get
            {
                return _browseFloder ?? (_browseFloder = new RelayCommand(
                    ExecuteBrowseFloder));
            }
        }
        private void ExecuteBrowseFloder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            Url = fbd.SelectedPath;
        }

        private RelayCommand _backup;
        public RelayCommand Backup
        {
            get
            {
                return _backup ?? (_backup = new RelayCommand(
                    ExecuteBackupAsync,
                    CanExecuteBackup));
            }
        }
        private async void ExecuteBackupAsync()
        {
            if (Url == null)
                return;
            using (SalesDB db = new SalesDB())
            {
                try
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    string fileName = _url + "\\SALESDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                            + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                    string dbname = db.Database.Connection.Database;
                    string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                    db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                    Url = "";
                    Mouse.OverrideCursor = null;
                    await _currentWindow.HideMetroDialogAsync(_backupDialog);
                    await _currentWindow.ShowMessageAsync("نجاح الحفظ", "تم الحفظ بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    await _currentWindow.ShowMetroDialogAsync(_backupDialog);
                }
                catch
                {
                    Mouse.OverrideCursor = null;
                    await _currentWindow.HideMetroDialogAsync(_backupDialog);
                    await _currentWindow.ShowMessageAsync("فشل الحفظ", "يجب إختيار مكان آخر لحفظ النسخة الإحتياطية", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    await _currentWindow.ShowMetroDialogAsync(_backupDialog);
                    return;
                }
            }
        }
        private bool CanExecuteBackup()
        {
            return !HasErrors;
        }

        // Restore Backup

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
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "استرجاع نسخة احتياطية",
                Filter = "Backup files(*.Bak)|*.Bak"
            };
            ofd.ShowDialog();
            Url = ofd.FileName;
        }

        private RelayCommand _restore;
        public RelayCommand Restore
        {
            get
            {
                return _restore ?? (_restore = new RelayCommand(
                    ExecuteRestoreAsync,
                    CanExecuteRestore));
            }
        }
        private async void ExecuteRestoreAsync()
        {
            if (Url == "" || Url == string.Empty || Url == null)
                return;
            SqlConnection sqlconnection = new SqlConnection(@"Server=.\sqlexpress; Database=master; Integrated Security=true");
            string strQuery = "ALTER Database HamadaSalesDB SET OFFLINE WITH ROLLBACK IMMEDIATE; Restore Database HamadaSalesDB From Disk='" + _url + "'WITH REPLACE";
            SqlCommand cmd = new SqlCommand(strQuery, sqlconnection);
            sqlconnection.Open();
            cmd.ExecuteNonQuery();
            strQuery = "ALTER Database HamadaSalesDB SET ONLINE WITH ROLLBACK IMMEDIATE";
            cmd = new SqlCommand(strQuery, sqlconnection);
            cmd.ExecuteNonQuery();
            sqlconnection.Close();
            Url = "";
            await _currentWindow.HideMetroDialogAsync(_restoreBackupDialog);
            await _currentWindow.ShowMessageAsync("نجاح الإستعادة", "تم الإستعادة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
            await _currentWindow.ShowMetroDialogAsync(_restoreBackupDialog);
        }
        private bool CanExecuteRestore()
        {
            return !HasErrors;
        }

        // User

        private RelayCommand _updateUser;
        public RelayCommand UpdateUser
        {
            get
            {
                return _updateUser ?? (_updateUser = new RelayCommand(
                    ExecuteUpdateUserAsync,
                    CanExecuteUpdateUser));
            }
        }
        private async void ExecuteUpdateUserAsync()
        {
            userServ.UpdateUser(_currentedUser);
            await _currentWindow.HideMetroDialogAsync(_userDialog);
        }
        private bool CanExecuteUpdateUser()
        {
            return !CurrentUser.HasErrors;
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
                case "Backup":
                    await _currentWindow.HideMetroDialogAsync(_backupDialog);
                    break;
                case "Restore":
                    await _currentWindow.HideMetroDialogAsync(_restoreBackupDialog);
                    break;
                case "User":
                    await _currentWindow.HideMetroDialogAsync(_userDialog);
                    break;
                case "Login":
                    await _currentWindow.HideMetroDialogAsync(_loginDialog);
                    _currentWindow.Close();
                    break;
                default:
                    break;
            }

        }

        private RelayCommand _shutdown;
        public RelayCommand Shutdown
        {
            get
            {
                return _shutdown ?? (_shutdown = new RelayCommand(
                    ExecuteShutdown));
            }
        }
        private void ExecuteShutdown()
        {
            string _path = "";
            bool _exists;
            try
            {
                _path = @"D:\النسخ الاحتياطية";
                _exists = Directory.Exists(_path);
                if (!_exists)
                    Directory.CreateDirectory(_path);
            }
            catch
            {

            }
            try
            {
                _path = @"E:\النسخ الاحتياطية";
                _exists = Directory.Exists(_path);
                if (!_exists)
                    Directory.CreateDirectory(_path);
            }
            catch
            {
                _path = @"D:\النسخ الاحتياطية";
            }
            using (SalesDB db = new SalesDB())
            {
                try
                {
                    string fileName = _path + "\\SALESDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                            + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                    string dbname = db.Database.Connection.Database;
                    string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                    db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                }
                catch
                {
                }
            }
        }
    }
}
