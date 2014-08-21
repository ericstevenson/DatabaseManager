using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.Domain.Concrete
{
    public class EFFinanceRepository : IFinanceRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Finance> Finances
        {
            get { return context.Finances; }
        }

        public int SaveFinance(Finance finance)
        {
            if (finance.Id == 0)
            {
                context.Finances.Add(finance);
            }
            else
            {
                context.Entry(finance).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
            return finance.Id;
        }


        public Finance DeleteFinance(int id)
        {
            Finance finance = context.Finances.Find(id);
            if (finance != null)
            {
                context.Entry(finance).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return finance;
        }
    }
}
