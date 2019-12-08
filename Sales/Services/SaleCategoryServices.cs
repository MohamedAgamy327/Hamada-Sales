using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SaleCategoryServices
    {
        public void AddSaleCategory(SaleCategory saleCategory)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesCategories.Add(saleCategory);
                db.SaveChanges();
            }
        }
      
        public void DeleteSaleCategories(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesCategories.RemoveRange(db.SalesCategories.Where(x => x.SaleID == saleID));
                db.SaveChanges();
            }
        }

        public decimal? GetCategoryQty(int saleID, int categoryID)
        {
            using (SalesDB db = new SalesDB())
            {
                var saleCategory = db.SalesCategories.SingleOrDefault(w => w.SaleID == saleID && w.CategoryID == categoryID);
                if (saleCategory == null)
                    return 0;
                else
                    return saleCategory.Qty;
            }
        }

        public List<SaleCategory> GetOldPrices(int categoryID,int clientID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesCategories.Where(w=>w.CategoryID==categoryID).Include(i=>i.Sale).Where(w=>w.Sale.ClientID==clientID).OrderByDescending(o=>o.Sale.Date).Take(4).ToList();
            }
        }
       
        public List<SaleCategory> GetSaleCategories(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesCategories.Where(w => w.SaleID == saleID).ToList();
            }
        }

        public List<SaleCategoryVM> GetSaleCategoriesVM(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesCategories.Where(w => w.SaleID == saleID).Include(c => c.Category).Include(c => c.Category.Company).Select(s => new SaleCategoryVM
                {
                    Category = s.Category.Name,
                    CategoryID = s.CategoryID,
                    Company = s.Category.Company.Name,
                    Cost = s.Cost,
                    PriceAfterDiscount = s.PriceAfterDiscount,
                    CostTotal = s.CostTotal,
                    PriceTotalAfterDiscount = s.PriceTotalAfterDiscount,
                    Discount = s.Discount,
                    DiscountValue = s.DiscountValue,
                    DiscountValueTotal = s.DiscountValueTotal,
                    Qty = s.Qty,
                    SaleID = s.SaleID,
                    Price = s.Price,
                    PriceTotal = s.PriceTotal
                }).ToList();
            }
        }
    }
}
