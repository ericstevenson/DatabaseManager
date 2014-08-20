using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using DatabaseManager.WebUI.Models;
using DatabaseManager.WebUI.Infrastructure.Abstract;

namespace DatabaseManager.WebUI.Controllers
{
    public class FinanceController : Controller
    {
        public const string ALERT_SUCCESS = "alert-success";
        public const string ALERT_DANGER = "alert-danger";

        private ILawsonDatabaseRepository repository;
        private IFinanceRepository financeRepository;
        private IPaymentProvider paymentProvider;

        public FinanceController(ILawsonDatabaseRepository repo, IFinanceRepository financeRepo, IPaymentProvider payment)
        {
            repository = repo;
            financeRepository = financeRepo;
            paymentProvider = payment;
        }

        public PartialViewResult Edit(int LawsonDatabaseID)
        {
            IEnumerable<Finance> finances = financeRepository.Finances.Where(f => f.DatabaseID == LawsonDatabaseID);
            LawsonDatabase database = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == LawsonDatabaseID);
            return PartialView("_Edit", new EditFinanceViewModel
            {
                DatabaseName = database.Nickname,
                NewInvoice = new Finance { DatabaseID = database.LawsonDatabaseID, Paid = false}
            });
        }

        [HttpPost]
        public ActionResult Edit(EditFinanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewInvoice.PeriodStart != null && model.NewInvoice.PeriodEnd != null && model.NewInvoice.MonthlyRate != null)
                {
                    model.NewInvoice.AmountDue = paymentProvider.CalculatePayment(model.NewInvoice.MonthlyRate.Value, model.NewInvoice.PeriodStart.Value, model.NewInvoice.PeriodEnd.Value);
                }
                financeRepository.SaveFinance(model.NewInvoice);
                addAlert("{0} has been saved", new string[] { model.DatabaseName ?? "Database" }, ALERT_SUCCESS);
                return RedirectToAction("List", "Database", null);
            }
            return View(model);
        }

        public ViewResult List()
        {
            FinanceListViewModel model = new FinanceListViewModel();
            model.FinanceList = new List<Tuple<string, Finance>>();
            foreach (Finance finance in financeRepository.Finances)
            {
                string name = repository.LawsonDatabases.First(d => d.LawsonDatabaseID == finance.DatabaseID).Nickname ?? "";
                model.FinanceList.Add(Tuple.Create<string, Finance>(name, finance));
            }
            return View(model);
        }

        public ViewResult Index()
        {
            return View(repository.LawsonDatabases.ToList());
        }

        public ActionResult EditFinance(int id)
        {
            return View();
        }

        /// <summary>
        /// Simple method for addng an alert to TempData
        /// </summary>
        /// <param name="alert">The string to be added. Should be formatting similarily to string.Format(string)</param>
        /// <param name="args">The arguments for the formatted string</param>
        /// <param name="alertClass">The bootstrap alert class to be added to the html tag</param>
        public void addAlert(string alert, string[] args, string alertClass)
        {
            TempData["message"] = string.Format(alert, args);
            TempData["alert-class"] = alertClass;
        }
    }
}