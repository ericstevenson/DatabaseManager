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
        private ILawsonDatabaseRepository repository;
        private IPeriodicFinanceRepository financeRepository;

        public FinanceController(ILawsonDatabaseRepository repo, IPeriodicFinanceRepository financeRepo)
        {
            repository = repo;
            financeRepository = financeRepo;
        }

        public ViewResult Edit(int LawsonDatabaseID)
        {
            IEnumerable<PeriodicFinance> finances = financeRepository.PeriodicFinances.Where(f => f.DatabaseID == LawsonDatabaseID);
            LawsonDatabase database = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == LawsonDatabaseID);
            return View(new EditFinanceViewModel
            {
                Finances = finances.ToList(),
                Database = database
            });
        }
    }
}