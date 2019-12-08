using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Services
{
    public class SafeServices
    {
        public void AddSafe(Safe safe)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Safes.Add(safe);
                db.SaveChanges();
            }
        }

        public void DeleteSafe(Safe safe)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Safes.Attach(safe);
                db.Safes.Remove(safe);
                db.SaveChanges();
            }
        }

        public void DeleteSafe(DateTime dt)
        {
            using (SalesDB db = new SalesDB())
            {
                var safe = db.Safes.SingleOrDefault(s => s.RegistrationDate == dt);
                if (safe != null)
                {
                    db.Safes.Attach(safe);
                    db.Safes.Remove(safe);
                    db.SaveChanges();
                }
            }
        }

        public int GetSafesNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => w.Statement.Contains(key)).Count();
            }
        }

        public int GetSafesNumer(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
            }
        }

        public decimal? GetCurrentAccount()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Sum(s => s.Amount);
            }
        }

        public decimal? GetTotalIncome(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => w.Amount > 0 && w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Amount);
            }
        }

        public decimal? GetTotalOutgoings(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => w.Amount < 0 && w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Amount);
            }
        }

        public decimal? GetItemSum(string key, DateTime dtFrom, DateTime dtTo, int source)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => w.Source == source && w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Amount);
            }
        }

        public List<string> GetStatementSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data = db.Safes.Where(w => w.CanDelete == true).OrderBy(o => o.Statement).Select(s => new { s.Statement }).Distinct().ToList();
                foreach (var item in data)
                {
                    newData.Add(item.Statement);
                }
                return newData;
            }
        }

        public List<Safe> SearchSafes(string search, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => (w.Statement).Contains(search)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

        public List<Safe> SearchSafes(string search, int page, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Safes.Where(w => (w.Statement).Contains(search) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }
    }
}
