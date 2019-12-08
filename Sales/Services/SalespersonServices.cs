using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SalespersonServices
    {
        public void AddSalesperson(Salesperson salesperson)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Salespersons.Add(salesperson);
                db.SaveChanges();
            }
        }

        public void DeleteSalesperson(Salesperson salesperson)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Salespersons.Attach(salesperson);
                db.Salespersons.Remove(salesperson);
                db.SaveChanges();
            }
        }

        public int GetSalespersonsNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Salespersons.Where(w => w.Name.Contains(key)).Count();
            }
        }

        public Salesperson GetSalesperson(string name)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Salespersons.SingleOrDefault(s => s.Name == name);
            }
        }

        public List<string> GetSalespersonsSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data = db.Salespersons.OrderBy(o => o.Name).Select(s => new { s.Name }).ToList();
                foreach (var item in data)
                {
                    newData.Add(item.Name);
                }
                return newData;
            }
        }

        public List<Salesperson> GetSalespersons()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Salespersons.OrderBy(o => o.Name).ToList();
            }
        }
      
        public List<Salesperson> SearchSalespersons(string search, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Salespersons.Where(w => (w.Name).Contains(search)).OrderBy(o => o.Name).Skip((page - 1) * 17).Take(17).Include(c => c.Sales).ToList();
            }
        }
    }
}
