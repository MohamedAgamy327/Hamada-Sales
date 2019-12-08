using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SupplyFutureServices
    {
        public void AddSupplyFuture(SupplyFuture supplyFuture)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesFuture.Add(supplyFuture);
                db.SaveChanges();
            }
        }

        public void UpdateSupplyFuture(SupplyFuture supplyFuture)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(supplyFuture).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSupplyFuture(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                var supplyFuture = db.SuppliesFuture.SingleOrDefault(s => s.SupplyID == supplyID);
                if (supplyFuture != null)
                {
                    db.SuppliesFuture.Attach(supplyFuture);
                    db.SuppliesFuture.Remove(supplyFuture);
                    db.SaveChanges();
                }
            }
        }

        public int GetSuppliesFutureNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                DateTime dtNow = DateTime.Now.Date;
                DateTime dt5Days = DateTime.Now.Date.AddDays(5);
                return db.SuppliesFuture.Include(i => i.Supply).Include(i => i.Supply.Client).Where(s => s.Change > 0 && s.Date <= dt5Days && s.Date >= dtNow && (s.SupplyID.ToString() + s.Supply.Client.Name).Contains(key)).Count();
            }
        }

        public SupplyFuture GetSupplyFuture(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesFuture.SingleOrDefault(s => s.SupplyID == supplyID);
            }
        }

        public SupplyFuture GetSupplyFuture()
        {
            using (SalesDB db = new SalesDB())
            {
                DateTime dtNow = DateTime.Now.Date;
                DateTime dt5Days = DateTime.Now.Date.AddDays(5);
                return db.SuppliesFuture.FirstOrDefault(s => s.Date <= dt5Days && s.Date >= dtNow);
            }
        }

        public List<string> GetPlacesSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data1 = db.SuppliesFuture.OrderBy(o => o.Place).Select(s => new { s.Place }).Distinct().ToList();
                var data2 = db.SuppliesPremiums.OrderBy(o => o.Place).Select(s => new { s.Place }).Distinct().ToList();
                foreach (var item in data1)
                {
                    newData.Add(item.Place);
                }
                foreach (var item in data2)
                {
                    newData.Add(item.Place);
                }
                return newData;
            }
        }

        public List<SupplyFuture> SearchSuppliesFuture(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                DateTime dtNow = DateTime.Now.Date;
                DateTime dt5Days = DateTime.Now.Date.AddDays(5);
                return db.SuppliesFuture.Include(i => i.Supply).Include(i => i.Supply.Client).Where(s =>s.Change > 0 &&  s.Date <= dt5Days && s.Date >= dtNow && (s.SupplyID.ToString() + s.Supply.Client.Name).Contains(key)).OrderByDescending(o => o.Date).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

    }
}
