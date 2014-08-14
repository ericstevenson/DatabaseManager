using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.Domain.Concrete
{
    public class EFEmailRepository : IEmailRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Email> Emails
        {
            get { return context.Emails; }
        }

        public void SaveEmail(Email email)
        {
            if (email.Id == 0)
            {
                context.Emails.Add(email);
            }
            else
            {
                context.Entry(email).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
