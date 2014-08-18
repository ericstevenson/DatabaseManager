using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.Domain.Concrete
{
    public class EFPeriodicFinanceRepository : IPeriodicFinanceRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<PeriodicFinance> PeriodicFinances
        {
            get { return context.PeriodicFinances; }
        }
    }
}
