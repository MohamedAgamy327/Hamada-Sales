using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SupplyRecallServices
    {
        public void AddSupplyRecall(SupplyRecall saleRecall)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesRecalls.Add(saleRecall);
                db.SaveChanges();
            }
        }

        public void DeleteSupplyRecall(DateTime dt)
        {
            using (SalesDB db = new SalesDB())
            {
                var supplyRecall = db.SuppliesRecalls.SingleOrDefault(s => s.RegistrationDate == dt);
                db.SuppliesRecalls.Attach(supplyRecall);
                db.SuppliesRecalls.Remove(supplyRecall);
                db.SaveChanges();
            }
        }

        public decimal? GetSupplyRecallsSum(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesRecalls.Where(w => w.SupplyID == supplyID).Sum(s => s.Qty);
            }
        }

        public decimal? GetRecallCategoryQty(int supplyID, int categoryID)
        {
            using (SalesDB db = new SalesDB())
            {
                var recallCategory = db.SuppliesRecalls.Where(w => w.SupplyID == supplyID && w.CategoryID == categoryID).Sum(s => s.Qty);
                if (recallCategory == null)
                    return 0;
                else
                    return recallCategory;
            }
        }

        public List<SupplyRecallVM> GetSupplyCategoriesVM(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesCategories.Where(w => w.SupplyID == supplyID).Include(c => c.Category).Include(c => c.Category.Company).OrderBy(o => o.Category.Name).Select(s => new SupplyRecallVM
                {
                    Category = s.Category.Name + " " + s.Category.Company.Name,
                    CategoryID = s.CategoryID,
                    SupplyID = s.SupplyID,
                    Date = DateTime.Now,
                    CostAfterDiscount = s.CostAfterDiscount,
                    CostAfterTax = s.CostAfterTax,
                    DiscountValue = s.DiscountValue
                }).ToList();
            }
        }

        public List<SupplyRecallVM> GetSupplyRecallsVM(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesRecalls.Where(w => w.SupplyID == supplyID).Include(c => c.Category).Include(c => c.Category.Company).OrderBy(o => o.RegistrationDate).Select(s => new SupplyRecallVM
                {
                    Category = s.Category.Name + " " + s.Category.Company.Name,
                    CategoryID = s.CategoryID,
                    CostAfterDiscount = s.CostAfterDiscount,
                    CostAfterTax = s.CostAfterTax,
                    CostTotalAfterDiscount = s.CostTotalAfterDiscount,
                    CostTotalAfterTax = s.CostTotalAfterTax,
                    DiscountValue = s.DiscountValue,
                    DiscountValueTotal = s.DiscountValueTotal,
                    Date = s.Date,
                    Qty = s.Qty,
                    RegistrationDate = s.RegistrationDate,
                    SupplyID = s.SupplyID,
                }).ToList();
            }
        }
    }
}
