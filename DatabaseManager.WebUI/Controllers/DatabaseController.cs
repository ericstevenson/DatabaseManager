using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;

namespace DatabaseManager.WebUI.Controllers
{
    public class DatabaseController : Controller
    {
        private IDatabaseRepository repository;

        public DatabaseController(IDatabaseRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Databases);
        }
	}
}