using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.WebUI.Infrastructure.Abstract;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.WebUI.Controllers
{
    public class EmailController : Controller
    {
        private IEmailProvider emailSender;
        private IEmailRepository repository;
        private ILawsonDatabaseRepository dbRepository;

        public EmailController(IEmailProvider emailer, IEmailRepository repo, ILawsonDatabaseRepository dbRepo)
        {
            dbRepository = dbRepo;
            emailSender = emailer;
            repository = repo;
        }

        public void SendEmail(string from = null)
        {
            if (from == Resources.Passwords.QUERY_KEY)
            {
                var context = System.Web.HttpContext.Current;
                string url = string.Format("{0}://{1}{2}{3}",
                                       context.Request.Url.Scheme,
                                       context.Request.Url.Host,
                                       context.Request.Url.Port == 80
                                           ? string.Empty
                                           : ":" + context.Request.Url.Port,
                                       context.Request.ApplicationPath);
                IEnumerable<LawsonDatabase> databases = dbRepository.LawsonDatabases.Where(d => d.REBExpiry != null && (d.REBExpiry.Value.Date == DateTime.Today.AddDays(1) || Math.Abs((d.REBExpiry.Value.Date - DateTime.Today).Days) == 30));
                List<Tuple<string, string>> dateAndName = new List<Tuple<string, string>>();
                foreach (var entry in databases)
                {
                    dateAndName.Add(Tuple.Create(entry.Nickname, entry.REBExpiry.Value.ToShortDateString()));
                }
                if (dateAndName.Count == 0)
                {
                    return;
                }
                repository.SaveEmail(emailSender.SendEmail(dateAndName, url));
            }
            else RedirectToAction("List", "Database");
        }
    }
}