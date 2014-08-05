using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }

        void AddUser(User user);

        User GetUser(string userName);

        void SaveUser(User user);
    }
}
