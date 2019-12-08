using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Services
{
    public class SpendingServices
    {
        public void AddSpending(Spending spending)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Spendings.Add(spending);
                db.SaveChanges();
            }
        }

        public void DeleteSpending(Spending spending)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Spendings.Attach(spending);
                db.Spendings.Remove(spending);
                db.SaveChanges();
            }
        }

        public int GetSpendingsNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Spendings.Where(w => w.Statement.Contains(key)).Count();
            }
        }

        public int GetSpendingsNumer(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Spendings.Where(w => w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
            }
        }

        public decimal? GetTotalAmount(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Spendings.Where(w => w.Statement.Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Amount);
            }
        }

        public List<string> GetStatementSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data = db.Spendings.OrderBy(o => o.Statement).Select(s => new { s.Statement }).Distinct().ToList();
                foreach (var item in data)
                {
                    newData.Add(item.Statement);
                }
                return newData;
            }
        }

        public List<Spending> SearchSpendings(string search, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Spendings.Where(w => (w.Statement).Contains(search)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

        public List<Spending> SearchSpendings(string search, int page, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Spendings.Where(w => (w.Statement).Contains(search) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }
    }
}
