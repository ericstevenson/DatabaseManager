using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<User> Users
        {
            get { return context.Users; }
        }

        public void AddUser(User user)
        {
            context.Users.Add(user);
        }

        public User GetUser(string userName)
        {
            return context.Users.SingleOrDefault(u => u.Username.Equals(userName, StringComparison.Ordinal));
        }

        public void SaveUser(User user)
        {
            if (user.UserID == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
