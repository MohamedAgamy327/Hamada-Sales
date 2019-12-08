using Sales.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class ClientAccountServices
    {
        

        public void AddAccount(ClientAccount account)
        {
            using (SalesDB db = new SalesDB())
            {
                db.ClientsAccounts.Add(account);
                db.SaveChanges();
            }
        }

        public void DeleteAccount(ClientAccount account)
        {
            using (SalesDB db = new SalesDB())
            {
                db.ClientsAccounts.Attach(account);
                db.ClientsAccounts.Remove(account);
                db.SaveChanges();
            }
        }

        public void DeleteAccount(DateTime dt)
        {
            using (SalesDB db = new SalesDB())
            {
                db.ClientsAccounts.RemoveRange(db.ClientsAccounts.Where(x => x.RegistrationDate == dt));
                db.SaveChanges();
            }
        }

        public int GetClientsAccountsNumer(string key)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(key)).Count();
            }
        }

        public int GetClientsAccountsNumer(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
            }
        }

        public decimal? GetTotalDebit(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Debit);
            }
        }

        public decimal? GetTotalCredit(string key, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Sum(s => s.Credit);
            }
        }      

        public decimal? GetClientAccount(int clientID)
        {
            using (SalesDB db=new SalesDB())
            {
                decimal? _discount = db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات")).Sum(d => d.Credit);
                if (_discount == null)
                    _discount = 0;
                decimal? _withoutCurrentAccount = db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات")).Sum(d => d.Credit) - db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات")).Sum(d => d.Debit);
                if (_withoutCurrentAccount == null)
                    _withoutCurrentAccount = 0;
                return _withoutCurrentAccount - _discount;
            }        
        }

        public string IsDiscount(int clientID)
        {
            using (SalesDB db = new SalesDB())
            {
                var _clientAccount = db.ClientsAccounts.FirstOrDefault(f => f.ClientID == clientID && f.Statement.Contains("خصومات"));
                if (_clientAccount == null)
                    return "Collapsed";
                else
                    return "Visible";
            }

        }

        public DateTime GetFirstDate(int clientID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Where(w => w.ClientID == clientID).Min(d => d.Date).Date;
            }
        }

        public DateTime GetLastDate(int clientID)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Where(w => w.ClientID == clientID).Max(d => d.Date).Date;
            }
        }

        public ClientAccountVM GetAccountInfo(int clientID, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                ClientServices clientServ = new ClientServices();
                var client = clientServ.GetClient(clientID);
                ClientAccountVM accountVM = new ClientAccountVM();
                accountVM.WithoutTotalCredit = db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات") && w.Date >= dtFrom && w.Date <= dtTo).Sum(d => d.Credit);
                if (accountVM.WithoutTotalCredit == null)
                    accountVM.WithoutTotalCredit = 0;
                accountVM.WithoutTotalDebit = db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات") && w.Date >= dtFrom && w.Date <= dtTo).Sum(d => d.Debit);
                if (accountVM.WithoutTotalDebit == null)
                    accountVM.WithoutTotalDebit = 0;
                accountVM.WithoutDuringAccount = accountVM.WithoutTotalCredit - accountVM.WithoutTotalDebit;
                accountVM.WithoutCurrentAccount = client.AccountStart + db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات")).Sum(d => d.Credit) - db.ClientsAccounts.Where(w => w.ClientID == clientID && !w.Statement.Contains("خصومات")).Sum(d => d.Debit);
                accountVM.WithoutOldAccount = accountVM.WithoutCurrentAccount - accountVM.WithoutDuringAccount;

                accountVM.DiscountTotalCredit = db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات") && w.Date >= dtFrom && w.Date <= dtTo).Sum(d => d.Credit);
                if (accountVM.DiscountTotalCredit == null)
                    accountVM.DiscountTotalCredit = 0;
                accountVM.DiscountTotalDebit = db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات") && w.Date >= dtFrom && w.Date <= dtTo).Sum(d => d.Debit);
                if (accountVM.DiscountTotalDebit == null)
                    accountVM.DiscountTotalDebit = 0;
                accountVM.DiscountDuringAccount = accountVM.DiscountTotalCredit - accountVM.DiscountTotalDebit;
                accountVM.DiscountCurrentAccount = db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات")).Sum(d => d.Credit) - db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات")).Sum(d => d.Debit);
                accountVM.DiscountOldAccount = accountVM.DiscountCurrentAccount - accountVM.DiscountDuringAccount;
                decimal? _discount = db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Statement.Contains("خصومات")).Sum(d => d.Credit);
                if (_discount == null)
                    _discount = 0;
                accountVM.CurrentAccount = accountVM.WithoutCurrentAccount - _discount;
                return accountVM;
            }
        }

        public List<ClientAccount> GetClientAccounts(int clientID, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Where(w => w.ClientID == clientID && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).ToList();
            }
        }

        public List<ClientAccount> SearchClientsAccounts(string search, int page)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(search)).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }

        public List<ClientAccount> SearchClientsAccounts(string search, int page, DateTime dtFrom, DateTime dtTo)
        {
            using (SalesDB db = new SalesDB())
            {
                return db.ClientsAccounts.Include(i => i.Client).Where(w => (w.Statement + w.Client.Name).Contains(search) && w.Date >= dtFrom && w.Date <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((page - 1) * 17).Take(17).ToList();
            }
        }
    }
}
