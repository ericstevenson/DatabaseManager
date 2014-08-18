using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.Domain.Concrete
{
    public class EFLawsonDatabaseRepository : ILawsonDatabaseRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<LawsonDatabase> LawsonDatabases
        {
            get { return context.LawsonDatabases; }
        }

        public int SaveDatabase(LawsonDatabase db)
        {
            if (db.LawsonDatabaseID == 0)
            {
                context.LawsonDatabases.Add(db);
            }
            else
            {
                context.Entry(db).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
            return db.LawsonDatabaseID;
        }

        public LawsonDatabase DeleteDatabase(int lawsonDatabaseId)
        {
            LawsonDatabase db = context.LawsonDatabases.Find(lawsonDatabaseId);
            if (db != null)
            {
                context.Entry(db).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return db;
        }
    }
}
