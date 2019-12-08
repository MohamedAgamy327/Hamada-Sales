using Sales.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Sales.Services
{
    public class SaleOfferCategoryServices
    {
        public void AddSaleOfferCategory(SaleOfferCategory saleOfferCategory)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesOffersCategories.Add(saleOfferCategory);
                db.SaveChanges();
            }
        }

        public void DeleteSaleOfferCategories(int saleOfferID)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesOffersCategories.RemoveRange(db.SalesOffersCategories.Where(x => x.SaleOfferID == saleOfferID));
                db.SaveChanges();
            }
        }

        public List<SaleOfferCategoryVM> GetSaleOfferCategories(int saleOfferID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesOffersCategories.Where(w => w.SaleOfferID == saleOfferID).Include(c => c.Category).Include(c => c.Category.Company).Select(s => new SaleOfferCategoryVM
                {
                    Category = s.Category.Name,
                    CategoryID = s.CategoryID,
                    Company = s.Category.Company.Name,
                    PriceAfterDiscount = s.PriceAfterDiscount,
                    PriceTotalAfterDiscount = s.PriceTotalAfterDiscount,
                    Discount = s.Discount,
                    DiscountValue = s.DiscountValue,
                    DiscountValueTotal = s.DiscountValueTotal,
                    Qty = s.Qty,
                    SaleOfferID = s.SaleOfferID,
                    Price = s.Price,
                    PriceTotal = s.PriceTotal
                }).ToList();
            }
        }
    }
}
