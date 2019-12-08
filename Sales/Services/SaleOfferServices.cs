using Sales.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class SaleOfferServices
    {
        public void AddSaleOffer(SaleOffer saleOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesOffers.Add(saleOffer);
                db.SaveChanges();
            }
        }

        public void UpdateSaleOffer(SaleOffer saleOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(saleOffer).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSaleOffer(SaleOffer saleOffer)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SalesOffers.Attach(saleOffer);
                db.SalesOffers.Remove(saleOffer);
                db.SaveChanges();
            }
        }

        public int GetSaleOffersNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesOffers.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).Count();
            }
        }

        public int GetLastSaleOfferID()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesOffers.AsEnumerable().LastOrDefault().ID;
            }
        }

        public SaleOffer GetSaleOffer(int id)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesOffers.Include(i => i.Client).SingleOrDefault(s => s.ID == id);
            }
        }

        public List<SaleOffer> SearchSaleOffers(string key, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SalesOffers.Include(i => i.Client).Where(w => (w.ID.ToString() + w.Client.Name).Contains(key)).OrderByDescending(o => o.Date).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

    }
}
