using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.Domain.Abstract
{
    public interface ILawsonDatabaseRepository
    {
        IEnumerable<LawsonDatabase> LawsonDatabases { get; }

        void SaveDatabase(LawsonDatabase db);

        LawsonDatabase DeleteDatabase(int lawsonDatabaseId);
    }
}
