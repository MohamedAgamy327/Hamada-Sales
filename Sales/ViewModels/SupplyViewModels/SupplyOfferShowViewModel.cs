using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using Sales.Helpers;
using Sales.Models;
using Sales.Reports;
using Sales.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sales.ViewModels.SupplyViewModels
{
    public class SupplyOfferShowViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;

        SupplyOfferServices _supplyOfferServ = new SupplyOfferServices();
        SupplyOfferCategoryServices _supplyOfferCategoryServ = new SupplyOfferCategoryServices();

        public static int ID
        {
            get; set;
        }

        public SupplyOfferShowViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _selectedSupplyOffer = _supplyOfferServ.GetSupplyOffer(ID);
            _supplyOfferCategories = new ObservableCollection<SupplyOfferCategoryVM>(_supplyOfferCategoryServ.GetSupplyOfferCategories(ID));
        }

        private SupplyOffer _selectedSupplyOffer;
        public SupplyOffer SelectedSupplyOffer
        {
            get { return _selectedSupplyOffer; }
            set { SetProperty(ref _selectedSupplyOffer, value); }
        }

        private SupplyOfferCategoryVM _selectedSupplyOfferCategory;
        public SupplyOfferCategoryVM SelectedSupplyOfferCategory
        {
            get { return _selectedSupplyOfferCategory; }
            set { SetProperty(ref _selectedSupplyOfferCategory, value); }
        }

        private ObservableCollection<SupplyOfferCategoryVM> _supplyOfferCategories;
        public ObservableCollection<SupplyOfferCategoryVM> SupplyOfferCategories
        {
            get { return _supplyOfferCategories; }
            set { SetProperty(ref _supplyOfferCategories, value); }
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
            foreach (var item in _supplyOfferCategories)
            {
                ds.Sale.Rows.Add();
                ds.Sale[i]["Client"] = _selectedSupplyOffer.Client.Name;
                ds.Sale[i]["Serial"] = i + 1;
                ds.Sale[i]["Category"] = item.Category + " " + item.Company;
                ds.Sale[i]["Qty"] = item.Qty;
                ds.Sale[i]["Price"] = Math.Round(Convert.ToDecimal(item.CostAfterTax), 2);
                ds.Sale[i]["TotalPrice"] = Math.Round(Convert.ToDecimal(item.CostTotalAfterTax), 2);
                ds.Sale[i]["BillPrice"] = Math.Round(Convert.ToDecimal(_selectedSupplyOffer.CostAfterTax), 2);
                i++;
            }
            ReportWindow rpt = new ReportWindow();
            SupplyOfferReport supplyOfferRPT = new SupplyOfferReport();
            supplyOfferRPT.SetDataSource(ds.Tables["Sale"]);
            rpt.crv.ViewerCore.ReportSource = supplyOfferRPT;
            Mouse.OverrideCursor = null;
            _currentWindow.Hide();
            rpt.ShowDialog();
            _currentWindow.ShowDialog();
        }

    }
}
