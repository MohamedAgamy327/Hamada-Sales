using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SupplyOfferServices
    {
        public void AddSupplyOffer(SupplyOffer supplyOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesOffers.Add(supplyOffer);
                db.SaveChanges();
            }
        }

        public void UpdateSupplyOffer(SupplyOffer supplyOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(supplyOffer).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSupplyOffer(SupplyOffer supplyOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesOffers.Attach(supplyOffer);
                db.SuppliesOffers.Remove(supplyOffer);
                db.SaveChanges();
            }
        }

        public int GetLastSupplyOfferID()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesOffers.AsEnumerable().LastOrDefault().ID;
            }
        }

        public int GetSuppliesOffersNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesOffers.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).Count();
            }
        }

        public SupplyOffer GetSupplyOffer(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesOffers.Include(i => i.Client).SingleOrDefault(s => s.ID == id);
            }
        }

        public List<SupplyOffer> SearchSuppliesOffers(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesOffers.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).OrderByDescending(o => o.Date).Skip((page - 1) * 17).Take(17).ToList();
            }
        }
    }
}
