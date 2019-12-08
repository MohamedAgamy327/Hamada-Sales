using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Sales.Services
{
    public class SaleServices
    {
        public void AddSale(Sale sale)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Sales.Add(sale);
                db.SaveChanges();
            }
        }

        public void UpdateSale(Sale sale)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSale(Sale sale)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Sales.Attach(sale);
                db.Sales.Remove(sale);
                db.SaveChanges();
            }
        }

        public int GetLastSaleID()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.AsEnumerable().LastOrDefault().ID;
            }
        }
       
        public int GetSalesNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Include(i => i.Salesperson).Where(w => (w.ID.ToString() + w.Client.Name + w.Salesperson.Name).Contains(key)).Count();
            }
        }

        public int GetSalesNumer(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Include(i => i.Salesperson).Where(w => (w.ID.ToString() + w.Client.Name + w.Salesperson.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
            }
        }

        public decimal? GetTotalTransportCost(int salespersonID, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Where(w => w.SalespersonID == salespersonID && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.TransportCost);
            }
        }

        public decimal? GetTotalBillPrice(int salespersonID, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Where(w => w.SalespersonID == salespersonID && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.PriceAfterDiscount);
            }
        }

        public decimal? GetSalesCost(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Where(w => w.Client.Name.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Cost);
            }
        }

        public decimal? GetSalesPrice(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Where(w => w.Client.Name.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.PriceAfterDiscount);
            }
        }

        public bool IsLastSale(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                int clientID = db.Sales.FirstOrDefault(d => d.ID == id).ClientID;
                return id == db.Sales.AsEnumerable().Where(s => s.ClientID == clientID).LastOrDefault().ID;
            }
        }

        public DateTime GetFirstDate(int salespersonID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Where(w => w.SalespersonID == salespersonID).Min(d => d.Date).Date;
            }
        }

        public DateTime GetLastDate(int salespersonID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Where(w => w.SalespersonID == salespersonID).Max(d => d.Date).Date;
            }
        }

        public Sale GetSale(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Include(i => i.Salesperson).SingleOrDefault(s => s.ID == id);
            }
        }

        public List<Sale> SearchSales(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Include(i => i.Salesperson).Where(w => (w.ID.ToString() + w.Client.Name + w.Salesperson.Name).Contains(key)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).Include(i=>i.SaleRecalls).ToList();
            }
        }

        public List<Sale> SearchSales(string key, int page, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i => i.Client).Include(i => i.Salesperson).Where(w => (w.ID.ToString() + w.Client.Name + w.Salesperson.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

        public List<Sale> GetSalespersonAccounts(int salespersonID, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Sales.Include(i=>i.Client).Where(w => w.SalespersonID == salespersonID && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).ToList();
            }
        }

    }
}
