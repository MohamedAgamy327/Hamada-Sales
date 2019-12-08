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

namespace Sales.ViewModels.SaleViewModels
{
    public class SaleOfferShowViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;

        SaleOfferServices _saleOfferServ = new SaleOfferServices();
        SaleOfferCategoryServices _saleOfferCategoryServ = new SaleOfferCategoryServices();

        public static int ID
        {
            get; set;
        }

        public SaleOfferShowViewModel()
        {
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _selectedSaleOffer = _saleOfferServ.GetSaleOffer(ID);
            _saleOfferCategories = new ObservableCollection<SaleOfferCategoryVM>(_saleOfferCategoryServ.GetSaleOfferCategories(ID));
        }

        private SaleOffer _selectedSaleOffer;
        public SaleOffer SelectedSaleOffer
        {
            get { return _selectedSaleOffer; }
            set { SetProperty(ref _selectedSaleOffer, value); }
        }

        private ObservableCollection<SaleOfferCategoryVM> _saleOfferCategories;
        public ObservableCollection<SaleOfferCategoryVM> SaleOfferCategories
        {
            get { return _saleOfferCategories; }
            set { SetProperty(ref _saleOfferCategories, value); }
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
            _currentWindow.ShowDialog();

        }

    }
}
