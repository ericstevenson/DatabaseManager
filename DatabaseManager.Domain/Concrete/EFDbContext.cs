using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<LawsonDatabase> LawsonDatabases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}
