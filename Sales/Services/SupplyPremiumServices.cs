using Sales.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Services
{
    public class SupplyPremiumServices
    {
        public void AddPremium(SupplyPremium premium)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesPremiums.Add(premium);
                db.SaveChanges();
            }
        }

        public void DeletePremium(SupplyPremium premium)
        {
            using (SalesDB db = new SalesDB())
            {
                db.SuppliesPremiums.Attach(premium);
                db.SuppliesPremiums.Remove(premium);
                db.SaveChanges();
            }
        }

        public List<SupplyPremium> GetPremiums(int supplyID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.SuppliesPremiums.Where(w => w.SupplyID == supplyID).ToList();
            }
        }
    }
}
