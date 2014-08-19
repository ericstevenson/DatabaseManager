using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using DatabaseManager.WebUI.Models;

namespace DatabaseManager.WebUI.Controllers
{
    public class FinanceController : Controller
    {
        public const string ALERT_SUCCESS = "alert-success";
        public const string ALERT_DANGER = "alert-danger";

        private ILawsonDatabaseRepository repository;
        private IFinanceRepository financeRepository;

        public FinanceController(ILawsonDatabaseRepository repo, IFinanceRepository financeRepo)
        {
            repository = repo;
            financeRepository = financeRepo;
        }

        public PartialViewResult Edit(int LawsonDatabaseID)
        {
            IEnumerable<Finance> finances = financeRepository.Finances.Where(f => f.DatabaseID == LawsonDatabaseID);
            LawsonDatabase database = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == LawsonDatabaseID);
            return PartialView(new EditFinanceViewModel
            {
                Finances = finances,
                DatabaseName = database.Nickname,
                NewInvoice = new Finance { DatabaseID = database.LawsonDatabaseID }
            });
        }

        [HttpPost]
        public ActionResult Edit(EditFinanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                financeRepository.SaveFinance(model.NewInvoice);
                addAlert("{0} has been saved", new string[] { model.DatabaseName ?? "Database" }, ALERT_SUCCESS);
                return RedirectToAction("List", "Database", null);
            }
            return View(model);
        }

        public ViewResult Index()
        {
            return View(repository.LawsonDatabases.ToList());
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