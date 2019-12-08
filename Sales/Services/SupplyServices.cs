using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SupplyServices
    {
        public void AddSupply(Supply supply)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Supplies.Add(supply);
                db.SaveChanges();
            }
        }

        public void UpdateSupply(Supply supply)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(supply).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSupply(Supply supply)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Supplies.Attach(supply);
                db.Supplies.Remove(supply);
                db.SaveChanges();
            }
        }

        public int GetLastSupplyID()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.AsEnumerable().LastOrDefault().ID;
            }
        }

        public int GetSuppliesNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).Count();
            }
        }

        public int GetSuppliesFutureNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                DateTime dt =Convert.ToDateTime( DateTime.Now.ToShortDateString());
                return db.Supplies.Include(i => i.Client).Include(i => i.SupplyFuture).Where(w => w.Future == true && w.SupplyFuture.Change > 0 && w.SupplyFuture.Date <= dt&& (w.ID.ToString() + w.Client.Name).Contains(key)).Count();
            }
        }

        public int GetSuppliesNumer(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
            }
        }

        public decimal? GetSuppliesCost(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Where(w => w.Client.Name.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.CostAfterTax);
            }
        }

        public bool IsLastSupply(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                int clientID = db.Supplies.FirstOrDefault(d => d.ID == id).ClientID;
                return id == db.Supplies.AsEnumerable().Where(s => s.ClientID == clientID).LastOrDefault().ID;
            }
        }

        public Supply GetSupply(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Include(i => i.SupplyFuture).SingleOrDefault(s => s.ID == id);
            }
        }

        public List<Supply> SearchSupplies(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).Include(i => i.SupplyFuture).Include(i => i.SupplyPremiums).Include(i => i.SupplyRecalls).ToList();
            }
        }

        public List<Supply> SearchSuppliesFuture(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                DateTime dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                return db.Supplies.Include(i => i.Client).Include(i => i.SupplyFuture).Where(w => w.Future == true && w.SupplyFuture.Change > 0 && w.SupplyFuture.Date <= dt && (w.ID.ToString() + w.Client.Name).Contains(key)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).Include(i => i.SupplyPremiums).Include(i => i.SupplyRecalls).ToList();
            }
        }

        public List<Supply> SearchSupplies(string key, int page, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Supplies.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }
    }
}
