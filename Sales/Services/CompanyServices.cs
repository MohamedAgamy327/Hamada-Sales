using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class CompanyServices
    {
        public void AddCompany(Company company)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Companies.Add(company);
                db.SaveChanges();
            }
        }

        public void DeleteCompany(Company company)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Companies.Attach(company);
                db.Companies.Remove(company);
                db.SaveChanges();
            }
        }

        public int GetCompaniesNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Companies.Where(w => w.Name.Contains(key)).Count();
            }
        }

        public Company GetCompany(string name)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Companies.SingleOrDefault(s => s.Name == name);
            }
        }

        public List<string> GetCompaniesSuggetions()
        {
            using (SalesDB db = new SalesDB())
            {
                List<string> newData = new List<string>();
                var data = db.Companies.OrderBy(o => o.Name).Select(s => new { s.Name }).ToList();
                foreach (var item in data)
                {
                    newData.Add(item.Name);
                }
                return newData;
            }
        }

        public List<Company> GetCompanies()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Companies.OrderBy(o => o.Name).ToList();
            }
        }
    
        public List<Company> SearchCompanies(string search, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Companies.Where(w => (w.Name).Contains(search)).OrderBy(o => o.Name).Skip((page - 1) * 17).Take(17).Include(c => c.Categories).ToList();
            }
        }
    }
}
