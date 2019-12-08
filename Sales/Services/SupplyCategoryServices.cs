using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SupplyCategoryServices
    {
        public void AddSupplyCategory(SupplyCategory supplyCategory)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesCategories.Add(supplyCategory);
                db.SaveChanges();
            }
        }

        public void DeleteSupplyCategories(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesCategories.RemoveRange(db.SuppliesCategories.Where(x => x.SupplyID == supplyID));
                db.SaveChanges();
            }
        }

        public decimal? GetCategoryQty(int supplyID, int categoryID)
        {
            using (SalesDB db = new SalesDB())
           {
                var saleCategory = db.SuppliesCategories.SingleOrDefault(w => w.SupplyID == supplyID && w.CategoryID == categoryID);
                if (saleCategory == null)
                    return 0;
                else
                    return saleCategory.Qty;
            }
        }

        public List<SupplyCategory> GetOldCosts (int categoryID, int clientID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesCategories.Where(w => w.CategoryID == categoryID).Include(i => i.Supply).Where(w => w.Supply.ClientID == clientID).OrderByDescending(o => o.Supply.Date).Take(4).ToList();
            }
        }      

        public List<SupplyCategory> GetSupplyCategories(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesCategories.Where(w => w.SupplyID == supplyID).ToList();
            }
        }

        public List<SupplyCategoryVM> GetSupplyCategoriesVM(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesCategories.Where(w => w.SupplyID == supplyID).Include(c => c.Category).Include(c => c.Category.Company).Select(s => new SupplyCategoryVM
                {
                    Category = s.Category.Name,
                    CategoryID = s.CategoryID,
                    Company = s.Category.Company.Name,
                    Cost = s.Cost,
                    CostAfterDiscount = s.CostAfterDiscount,
                    CostAfterTax = s.CostAfterTax,
                    CostTotal = s.CostTotal,
                    CostTotalAfterDiscount = s.CostTotalAfterDiscount,
                    CostTotalAfterTax = s.CostTotalAfterTax,
                    Discount = s.Discount,
                    DiscountValue = s.DiscountValue,
                    DiscountValueTotal = s.DiscountValueTotal,
                    Qty = s.Qty,
                    SupplyID = s.SupplyID,
                    Tax = s.Tax,
                    TaxValue = s.TaxValue,
                    TaxValueTotal = s.TaxValueTotal,
                    Price = s.Price
                }).ToList();
            }
        }
    }
}
