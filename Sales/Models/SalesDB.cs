using System.Data.Entity;

namespace Sales.Models
{
    public class SalesDB : DbContext
    {
        public SalesDB()
        {
         //   Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supply>()
                .HasOptional(a => a.SupplyFuture)
                .WithRequired(m => m.Supply)
                .WillCascadeOnDelete(true);        
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Safe> Safes { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAccount> ClientsAccounts { get; set; }
        public DbSet<Salesperson> Salespersons { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleCategory> SalesCategories { get; set; }
        public DbSet<SaleRecall> SalesRecalls { get; set; }
        public DbSet<SaleOffer> SalesOffers { get; set; }
        public DbSet<SaleOfferCategory> SalesOffersCategories { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyOffer> SuppliesOffers { get; set; }
        public DbSet<SupplyCategory> SuppliesCategories { get; set; }
        public DbSet<SupplyOfferCategory> SuppliesOffersCategories { get; set; }
        public DbSet<SupplyRecall> SuppliesRecalls { get; set; }
        public DbSet<SupplyFuture> SuppliesFuture { get; set; }
        public DbSet<SupplyPremium> SuppliesPremiums { get; set; }
    }
}
