using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class CategoryServices 
    {
        CategoryVM categoryInfo = new CategoryVM();
        List<CategoryVM> requiredCategories = new List<CategoryVM>();

        public void AddCategory(Category category)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }

        public void DeleteCategory(Category category)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Categories.Attach(category);
                db.Categories.Remove(category);
                db.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public int GetCategoriesNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Categories.Include(i => i.Company).Where(w => (w.Name + w.Company.Name).Contains(key)).Count();
            }
        }

        public DateTime GetFirstDate(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                var supply = db.SuppliesCategories.Include(i => i.Supply).AsEnumerable().Where(w => w.CategoryID == id).FirstOrDefault();
                var sale = db.SalesCategories.Include(i => i.Sale).AsEnumerable().Where(w => w.CategoryID == id).FirstOrDefault();
                if (supply == null && sale == null)
                    return Convert.ToDateTime(DateTime.Now.ToShortDateString());
                else if (supply == null && sale != null)
                    return sale.Sale.Date;
                else if (supply != null && sale == null)
                    return supply.Supply.Date;
                else
                {
                    if (supply.Supply.Date > sale.Sale.Date)
                        return sale.Sale.Date;
                    else
                        return supply.Supply.Date;
                }
            }
        }

        public DateTime GetLastDate(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                var supply = db.SuppliesCategories.Include(i => i.Supply).AsEnumerable().Where(w => w.CategoryID == id).LastOrDefault();
                var sale = db.SalesCategories.Include(i => i.Sale).AsEnumerable().Where(w => w.CategoryID == id).LastOrDefault();
                if (supply == null && sale == null)
                    return Convert.ToDateTime(DateTime.Now.ToShortDateString());
                else if (supply == null && sale != null)
                    return sale.Sale.Date;
                else if (supply != null && sale == null)
                    return supply.Supply.Date;
                else
                {
                    if (supply.Supply.Date < sale.Sale.Date)
                        return sale.Sale.Date;
                    else
                        return supply.Supply.Date;
                }
            }
        }

        public Category GetCategory(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Categories.SingleOrDefault(s => s.ID == id);
            }
        }

        public Category GetCategory(string name, int companyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Categories.SingleOrDefault(s => s.Name == name && s.CompanyID == companyID);
            }
        }

        public CategoryVM GetCategoryInformation(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                var category = db.Categories.Include(i => i.Company).SingleOrDefault(s => s.ID == id);
                categoryInfo.ID = category.ID;
                categoryInfo.CompanyID = category.CompanyID;
                categoryInfo.Company = category.Company.Name;
                categoryInfo.Category = category.Name;
                categoryInfo.QtyStart = category.QtyStart;
                categoryInfo.Qty = category.Qty;
                categoryInfo.Cost = category.Cost;
                categoryInfo.Price = category.Price;
                categoryInfo.RequestLimit = category.RequestLimit;
                categoryInfo.TotalPrice = category.Price * category.Qty;
                categoryInfo.TotalCost = category.Cost * category.Qty;
                return categoryInfo;
            }
        }

        public CategoryVM GetCategoryInformation(int id, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                decimal? supplyQty = db.SuppliesCategories.Where(w => w.CategoryID == id).Include(i => i.Supply).Where(w => w.Supply.Date >= dtFrom && w.Supply.Date <= dtTo).Sum(s => s.Qty);
                decimal? supplyRecallQty = db.SuppliesRecalls.Where(w => w.CategoryID == id).Include(i => i.Supply).Where(w => w.Supply.Date >= dtFrom && w.Supply.Date <= dtTo).Sum(s => s.Qty);
                decimal? supplyCost = db.SuppliesCategories.Where(w => w.CategoryID == id).Include(i => i.Supply).Where(w => w.Supply.Date >= dtFrom && w.Supply.Date <= dtTo).Sum(s => s.CostTotalAfterDiscount);
                decimal? recallCost = db.SuppliesRecalls.Where(w => w.CategoryID == id).Include(i => i.Supply).Where(w => w.Supply.Date >= dtFrom && w.Supply.Date <= dtTo).Sum(s => s.CostTotalAfterDiscount);
                if (supplyQty == null)
                    supplyQty = 0;
                if (supplyRecallQty == null)
                    supplyRecallQty = 0;
                if (supplyCost == null)
                    supplyCost = 0;
                if (recallCost == null)
                    recallCost = 0;
                categoryInfo.SupplyQty = supplyQty - supplyRecallQty;
                categoryInfo.SupplyCost = supplyCost - recallCost;

                decimal? saleQty = db.SalesCategories.Where(w => w.CategoryID == id).Include(i => i.Sale).Where(w => w.Sale.Date >= dtFrom && w.Sale.Date <= dtTo).Sum(s => s.Qty);
                decimal? saleRecallQty = db.SalesRecalls.Where(w => w.CategoryID == id).Include(i => i.Sale).Where(w => w.Sale.Date >= dtFrom && w.Sale.Date <= dtTo).Sum(s => s.Qty);
                decimal? salePrice = db.SalesCategories.Where(w => w.CategoryID == id).Include(i => i.Sale).Where(w => w.Sale.Date >= dtFrom && w.Sale.Date <= dtTo).Sum(s => s.PriceTotalAfterDiscount);
                decimal? recallPrice = db.SalesRecalls.Where(w => w.CategoryID == id).Include(i => i.Sale).Where(w => w.Sale.Date >= dtFrom && w.Sale.Date <= dtTo).Sum(s => s.PriceTotalAfterDiscount);
                if (saleQty == null)
                    saleQty = 0;
                if (saleRecallQty == null)
                    saleRecallQty = 0;
                if (salePrice == null)
                    salePrice = 0;
                if (recallPrice == null)
                    recallPrice = 0;
                categoryInfo.SaleQty = saleQty - saleRecallQty;
                categoryInfo.SalePrice = salePrice - recallPrice;
                return categoryInfo;
            }

        }

        public List<string> GetCategoriesSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data = db.Categories.OrderBy(o => o.Name).Select(s => new { s.Name }).Distinct().ToList();
                foreach (var item in data)
                {
                    newData.Add(item.Name);
                }
                return newData;
            }
        }

        public List<Category> SearchCategories(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Categories.Include(i => i.Company).Where(w => (w.Name + w.Company.Name).Contains(key)).OrderBy(o => o.Company.Name).ThenBy(t => t.Name).Skip((page - 1) * 17).Take(17).Include(i => i.SuppliesCategories).Include(i => i.SalesCategories).ToList();
            }
        }

        public List<CategoryVM> GetRequiredCategories()
        {
            using (SalesDB db = new SalesDB())
            {
                requiredCategories = db.Categories.Include(i => i.Company).Select(s => new CategoryVM
                {
                    Category = s.Name,
                    Company = s.Company.Name,
                    Price=s.Price,
                    Cost = s.Cost,
                    Qty = s.Qty,
                    RequestLimit = s.RequestLimit,
                    ID = s.ID
                }).ToList();
                return requiredCategories;
            }
        }

        public List<CategoryVM> GetRequiredCategories(string key, int page)
        {
            return requiredCategories.OrderBy(o => o.Company).ThenBy(t => t.Category).Where(w => (w.Company + w.Category).Contains(key)).Skip((page - 1) * 17).Take(17).ToList();
        }

        public List<CategoryVM> GetAllRequiredCategories()
        {
            return requiredCategories.ToList();
        }
    }
}
