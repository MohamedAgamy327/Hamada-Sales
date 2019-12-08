using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public  class SupplyOfferCategoryServices
    {
        public void AddSupplyOfferCategory(SupplyOfferCategory supplyOfferCategory)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesOffersCategories.Add(supplyOfferCategory);
                db.SaveChanges();
            }
        }

        public void DeleteSupplyOfferCategories(int supplyOfferID)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesOffersCategories.RemoveRange(db.SuppliesOffersCategories.Where(x => x.SupplyOfferID == supplyOfferID));
                db.SaveChanges();
            }
        }

        public List<SupplyOfferCategoryVM> GetSupplyOfferCategories(int supplyOfferID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesOffersCategories.Where(w => w.SupplyOfferID == supplyOfferID).Include(c => c.Category).Include(c => c.Category.Company).Select(s => new SupplyOfferCategoryVM
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
                    SupplyOfferID = s.SupplyOfferID,
                    Tax = s.Tax,
                    TaxValue = s.TaxValue,
                    TaxValueTotal = s.TaxValueTotal
                }).ToList();
            }
        }
    }
}
