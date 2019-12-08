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
    public class SupplyShowViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly SupplyRecallDialog _supplyRecallDialog;
        private readonly SupplyCategoryInfromationDialog _categoryDialog;

        SupplyServices _supplyServ = new SupplyServices();
        CategoryServices _categoryServ = new CategoryServices();
        SupplyRecallServices _supplyRecallServ = new SupplyRecallServices();
        ClientAccountServices _clientAccountServ = new ClientAccountServices();
        SupplyCategoryServices _supplyCategoryServ = new SupplyCategoryServices();

        public static int ID
        {
            get; set;
        }

        public SupplyShowViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _categoryDialog = new SupplyCategoryInfromationDialog();
            _supplyRecallDialog = new SupplyRecallDialog();
            _selectedSupply = _supplyServ.GetSupply(ID);
            _supplyCategories = new ObservableCollection<SupplyCategoryVM>(_supplyCategoryServ.GetSupplyCategoriesVM(ID));
            _categories = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyCategoriesVM(ID));
            RecallsQty = _supplyRecallServ.GetSupplyRecallsSum(ID);
        }

        private bool _isFocused = true;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private string _state = "Normal";
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private decimal? _recallsQty;
        public decimal? RecallsQty
        {
            get { return _recallsQty; }
            set { SetProperty(ref _recallsQty, value); }
        }

        private Supply _selectedSupply;
        public Supply SelectedSupply
        {
            get { return _selectedSupply; }
            set { SetProperty(ref _selectedSupply, value); }
        }

        private SupplyCategoryVM _selectedSupplyCategory;
        public SupplyCategoryVM SelectedSupplyCategory
        {
            get { return _selectedSupplyCategory; }
            set { SetProperty(ref _selectedSupplyCategory, value); }
        }

        private SupplyRecallVM _newRecall;
        public SupplyRecallVM NewRecall
        {
            get { return _newRecall; }
            set { SetProperty(ref _newRecall, value); }
        }

        private SupplyRecallVM _selectedRecall;
        public SupplyRecallVM SelectedRecall
        {
            get { return _selectedRecall; }
            set { SetProperty(ref _selectedRecall, value); }
        }

        private ObservableCollection<SupplyCategoryVM> _supplyCategories;
        public ObservableCollection<SupplyCategoryVM> SupplyCategories
        {
            get { return _supplyCategories; }
            set { SetProperty(ref _supplyCategories, value); }
        }

        private ObservableCollection<SupplyRecallVM> _categories;
        public ObservableCollection<SupplyRecallVM> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private ObservableCollection<SupplyRecallVM> _supplyRecalls;
        public ObservableCollection<SupplyRecallVM> SupplyRecalls
        {
            get { return _supplyRecalls; }
            set { SetProperty(ref _supplyRecalls, value); }
        }

        // Show

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
            ds.Sale.Rows.Clear();
            int i = 0;
            foreach (var item in _supplyCategories)
            {
                ds.Sale.Rows.Add();
                ds.Sale[i]["ID"] = ID;
                ds.Sale[i]["Date"] = _selectedSupply.Date;
                ds.Sale[i]["Client"] = _selectedSupply.Client.Name;
                ds.Sale[i]["Serial"] = i + 1;
                ds.Sale[i]["Category"] = item.Category + " " + item.Company;
                ds.Sale[i]["Qty"] = item.Qty;
                ds.Sale[i]["Price"] = Math.Round(Convert.ToDecimal(item.CostAfterTax), 2);
                ds.Sale[i]["TotalPrice"] = Math.Round(Convert.ToDecimal(item.CostTotalAfterTax), 2);
                ds.Sale[i]["BillPrice"] = Math.Round(Convert.ToDecimal(_selectedSupply.CostAfterTax), 2);
                ds.Sale[i]["OldDebt"] = Math.Abs(Math.Round(Convert.ToDecimal(_selectedSupply.OldDebt), 2));

                ds.Sale[i]["Paid"] = Math.Abs(Math.Round(Convert.ToDecimal(_selectedSupply.CashPaid + _selectedSupply.DiscountPaid), 2));
                ds.Sale[i]["NewDebt"] = Math.Abs(Math.Round(Convert.ToDecimal(_selectedSupply.NewDebt), 2));
                if (_selectedSupply.NewDebt > 0)
                    ds.Sale[i]["PrintingMan"] = "له";
                else if (_selectedSupply.NewDebt < 0)
                    ds.Sale[i]["PrintingMan"] = "عليه";

                if (_selectedSupply.OldDebt > 0)
                    ds.Sale[i]["Type"] = "له";
                else if (_selectedSupply.OldDebt < 0)
                    ds.Sale[i]["Type"] = "عليه";
                ds.Sale[i]["BillTotal"] = Math.Abs(Math.Round(Convert.ToDecimal(_selectedSupply.OldDebt), 2) + Math.Round(Convert.ToDecimal(_selectedSupply.CostAfterTax), 2));
                if (Math.Round(Convert.ToDecimal(_selectedSupply.OldDebt), 2) + Math.Round(Convert.ToDecimal(_selectedSupply.CostAfterTax), 2) > 0)
                    ds.Sale[i]["Type2"] = "له";
                else if (Math.Round(Convert.ToDecimal(_selectedSupply.OldDebt), 2) + Math.Round(Convert.ToDecimal(_selectedSupply.CostAfterTax), 2) < 0)
                    ds.Sale[i]["Type2"] = "عليه";
                i++;
            }
            ReportWindow rpt = new ReportWindow();
            SupplyReport supplyRPT = new SupplyReport();
            supplyRPT.SetDataSource(ds.Tables["Sale"]);
            rpt.crv.ViewerCore.ReportSource = supplyRPT;
            Mouse.OverrideCursor = null;
            _currentWindow.Hide();
            rpt.ShowDialog();
            _currentWindow.ShowDialog();
        }

        // Category Details

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

        // Recalls

        private RelayCommand _showRecall;
        public RelayCommand ShowRecall
        {
            get
            {
                return _showRecall
                    ?? (_showRecall = new RelayCommand(ShowRecallMethodAsync));
            }
        }
        private async void ShowRecallMethodAsync()
        {
            _supplyRecallDialog.DataContext = this;
            State = "Maximized";
            Categories = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyCategoriesVM(ID));
            SupplyRecalls = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyRecallsVM(ID));
            await _currentWindow.ShowMetroDialogAsync(_supplyRecallDialog);
        }

        private RelayCommand _addRecall;
        public RelayCommand AddRecall
        {
            get
            {
                return _addRecall
                    ?? (_addRecall = new RelayCommand(ExecuteAddRecallAsync, CanExecuteAddRecall));
            }
        }
        private async void ExecuteAddRecallAsync()
        {
            if (NewRecall.CostTotalAfterTax == null)
                return;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            };
            var supplyCategoryQty = _supplyCategoryServ.GetCategoryQty(ID, _newRecall.CategoryID);
            if (_newRecall.Qty > supplyCategoryQty)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", "هذه الكمية اكبر من الكمية الخاصة بالفاتورة", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            var recallQty = _supplyRecallServ.GetRecallCategoryQty(ID, _newRecall.CategoryID);
            if (_newRecall.Qty + recallQty > supplyCategoryQty)
            {
                MessageDialogResult result = await _currentWindow.ShowMessageAsync("خطأ", " هذه الكمية والمرتجعات اكبر من الكمية الخاصة بالفاتورة", MessageDialogStyle.Affirmative, mySettings);
                return;
            }
            DateTime dt = DateTime.Now;
            SupplyRecall supplyRecall = new SupplyRecall
            {
                CategoryID = _newRecall.CategoryID,
                CostAfterDiscount = _newRecall.CostAfterDiscount,
                CostAfterTax = _newRecall.CostAfterTax,
                CostTotalAfterDiscount = _newRecall.CostTotalAfterDiscount,
                CostTotalAfterTax = _newRecall.CostTotalAfterTax,
                Date = _newRecall.Date,
                DiscountValue = _newRecall.DiscountValue,
                DiscountValueTotal = _newRecall.DiscountValueTotal,
                RegistrationDate = dt,
                Qty = _newRecall.Qty,
                SupplyID = _newRecall.SupplyID
            };
            _supplyRecallServ.AddSupplyRecall(supplyRecall);
            Category cat = _categoryServ.GetCategory(_newRecall.CategoryID);
            if (cat.Qty - _newRecall.Qty != 0)
                cat.Cost = ((cat.Cost * cat.Qty) - _newRecall.CostTotalAfterDiscount) / (cat.Qty - _newRecall.Qty);
            cat.Qty = cat.Qty - _newRecall.Qty;
            _categoryServ.UpdateCategory(cat);
            ClientAccount _account = new ClientAccount
            {
                ClientID = _selectedSupply.ClientID,
                Date = _newRecall.Date,
                RegistrationDate = dt,
                Statement = "مرتجعات فاتورة مشتريات رقم " + ID,
                Credit = 0,
                Debit = _newRecall.CostTotalAfterTax
            };
            _clientAccountServ.AddAccount(_account);
            if (_newRecall.DiscountValue != 0)
            {
                _account = new ClientAccount
                {
                    ClientID = _selectedSupply.ClientID,
                    Date = _newRecall.Date,
                    RegistrationDate = dt,
                    Statement = "خصومات مرتجعات فاتورة مشتريات رقم " + ID,
                    Credit = _newRecall.DiscountValueTotal,
                    Debit = 0
                };
                _clientAccountServ.AddAccount(_account);
            }
            Categories = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyCategoriesVM(ID));
            SupplyRecalls = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyRecallsVM(ID));
            await _currentWindow.ShowMessageAsync("نجاح الإضافة", "تم الإضافة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
            {
                AffirmativeButtonText = "موافق",
                DialogMessageFontSize = 25,
                DialogTitleFontSize = 30
            });
        }
        private bool CanExecuteAddRecall()
        {
            try
            {
                if (NewRecall.HasErrors || NewRecall == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private RelayCommand _deleteRecall;
        public RelayCommand DeleteRecall
        {
            get
            {
                return _deleteRecall
                    ?? (_deleteRecall = new RelayCommand(DeleteRecallMethod));
            }
        }
        private async void DeleteRecallMethod()
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
                _clientAccountServ.DeleteAccount(_selectedRecall.RegistrationDate);
                _supplyRecallServ.DeleteSupplyRecall(_selectedRecall.RegistrationDate);
                Category cat = _categoryServ.GetCategory(_selectedRecall.CategoryID);
                if (cat.Qty + _selectedRecall.Qty != 0)
                    cat.Cost = ((cat.Cost * cat.Qty) + _selectedRecall.CostTotalAfterDiscount) / (cat.Qty + _selectedRecall.Qty);
                cat.Qty = cat.Qty + _selectedRecall.Qty;
                _categoryServ.UpdateCategory(cat);
                SupplyRecalls = new ObservableCollection<SupplyRecallVM>(_supplyRecallServ.GetSupplyRecallsVM(ID));
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
                case "Category":
                    await _currentWindow.HideMetroDialogAsync(_categoryDialog);
                    break;
                case "Recall":
                    await _currentWindow.HideMetroDialogAsync(_supplyRecallDialog);
                    State = "Normal";
                    RecallsQty = _supplyRecallServ.GetSupplyRecallsSum(ID);
                    break;
                default:
                    break;
            }

        }
    }
}
