using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SaleRecallServices
    {
        public void AddSaleRecall(SaleRecall saleRecall)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesRecalls.Add(saleRecall);
                db.SaveChanges();
            }
        }

        public void DeleteSaleRecall(DateTime dt)
        {
            using (SalesDB db = new SalesDB())
            {
                var saleRecall = db.SalesRecalls.SingleOrDefault(s => s.RegistrationDate == dt);
                db.SalesRecalls.Attach(saleRecall);
                db.SalesRecalls.Remove(saleRecall);
                db.SaveChanges();
            }
        }

        public decimal? GetSaleRecallsSum(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesRecalls.Where(w => w.SaleID == saleID).Sum(s => s.Qty);
            }

        }

        public decimal? GetRecallCategoryQty(int saleID, int categoryID)
        {
            using (SalesDB db = new SalesDB())
            {
                var recallCategory = db.SalesRecalls.Where(w => w.SaleID == saleID && w.CategoryID == categoryID).Sum(s => s.Qty);
                if (recallCategory == null)
                    return 0;
                else
                    return recallCategory;
            }
        }

        public List<SaleRecallVM> GetSaleCategoriesVM(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesCategories.Where(w => w.SaleID == saleID).Include(c => c.Category).Include(c => c.Category.Company).OrderBy(o => o.Category.Name).Select(s => new SaleRecallVM
                {
                    Category = s.Category.Name + " " + s.Category.Company.Name,
                    CategoryID = s.CategoryID,
                    Cost = s.Cost,
                    PriceAfterDiscount = s.PriceAfterDiscount,
                    SaleID = s.SaleID,
                    Date = DateTime.Now
                }).ToList();
            }
        }

        public List<SaleRecallVM> GetSaleRecallsVM(int saleID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesRecalls.Where(w => w.SaleID == saleID).Include(c => c.Category).Include(c => c.Category.Company).OrderBy(o => o.RegistrationDate).Select(s => new SaleRecallVM
                {
                    Category = s.Category.Name + " " + s.Category.Company.Name,
                    CategoryID = s.CategoryID,
                    Cost = s.Cost,
                    CostTotal = s.CostTotal,
                    PriceAfterDiscount = s.PriceAfterDiscount,
                    PriceTotalAfterDiscount = s.PriceTotalAfterDiscount,
                    Date = s.Date,
                    Qty = s.Qty,
                    RegistrationDate = s.RegistrationDate,
                    SaleID = s.SaleID,
                }).ToList();
            }
        }
    }
}
