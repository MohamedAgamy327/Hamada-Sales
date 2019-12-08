using Sales.Models;
using System.Data.Entity;
using System.Linq;

namespace Sales.Services
{
    public class UserServices
    {
        public void UpdateUser(User user)
        {
            using (SalesDB db = new SalesDB())
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public User GetUser()
        {
            using (SalesDB db = new SalesDB())
            {
                return db.Users.FirstOrDefault();
            }
        }
    }
}

