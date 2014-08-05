using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;
using System.Xml;
using System.Xml.Linq;

namespace DatabaseManager.WebUI.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        public const string ALERT_SUCCESS = "alert-success";
        public const string ALERT_DANGER = "alert-danger";

        private ILawsonDatabaseRepository repository;

        public DatabaseController(ILawsonDatabaseRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.LawsonDatabases);
        }

        public ViewResult Edit(int lawsonDatabaseID)
        {
            var db = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == lawsonDatabaseID);
            var tempFields = db.AdditionalFields;
            ViewBag.AdditionalFields = tempFields != null ? XDocument.Parse(tempFields).Descendants("AdditionalFields")
                .Elements()
                .ToDictionary(r => r.Name.ToString().Replace("SpaceDummyChar", " "), r => r.Value.ToString().Replace("SpaceDummyChar", " ")) : new Dictionary<string, string>();

            return View(db);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(LawsonDatabase db)
        {
            if (ModelState.IsValid)
            {
                repository.SaveDatabase(db);
                addAlert("{0} has been saved", new string[] { db.Nickname }, ALERT_SUCCESS);
                return RedirectToAction("List");
            }
            else
            {
                return View(db);
            }
        }

        public ViewResult Create()
        {
            ViewBag.AdditionalFields = new Dictionary<string, string>();
            return View("Edit", new LawsonDatabase());
        }

        [HttpPost]
        public ActionResult Delete(int lawsonDatabaseId)
        {
            LawsonDatabase dbToDelete = repository.DeleteDatabase(lawsonDatabaseId);
            if (dbToDelete != null)
            {
                addAlert("{0} has been deleted", new string[] { dbToDelete.Nickname }, ALERT_SUCCESS);
            }
            return RedirectToAction("List");
        }

        public ViewResult AddColumn(int lawsonDatabaseID)
        {
            LawsonDatabase database = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == lawsonDatabaseID);
            return View(database);
        }

        [HttpPost]
        public ActionResult AddColumn(string save, int lawsonDatabaseID, string fieldName, string fieldValue)
        {
            LawsonDatabase db = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == lawsonDatabaseID);

            if (fieldName == "")
            {
                addAlert("You did not enter a field name. Please enter the field name before continuing.", new string[]{}, ALERT_DANGER);
                return View(db);
            }

            // Replace spaces with unused character (for xml formatting)
            string safeFieldName = fieldName.Replace(" ", "SpaceDummyChar");
            string safeFieldValue = fieldValue.Replace(" ", "SpaceDummyChar");

            XElement elem = new XElement(safeFieldName, safeFieldValue);
            XDocument doc = new XDocument();

            List<LawsonDatabase> databases = repository.LawsonDatabases.ToList();

            if (save == "Save Field to All Databases")
            {
                foreach (var database in databases)
                {
                    if (database.AdditionalFields == null)
                    {
                        doc = createDocumentForEmptyFields(safeFieldName, safeFieldValue);
                    }
                    else
                    {
                        doc = XDocument.Parse(database.AdditionalFields);
                        if (doc.Descendants().FirstOrDefault(m => m.Name == safeFieldName) != null)
                        {
                            addAlert("At least one database already contained the field \"{0}\". These databases were not modified", new string[] { fieldName }, ALERT_DANGER);
                            continue;
                        }
                        doc.Root.Add(elem);
                    } 

                    database.AdditionalFields = doc.ToString();
                    repository.SaveDatabase(database);

                    if (TempData["message"] == null)
                    {
                        addAlert("{0} successfully added", new string[] { fieldName }, ALERT_SUCCESS);
                    }
                }
                return RedirectToAction("List");
            }
            else
            {
                if (db.AdditionalFields == null)
                {
                    doc = createDocumentForEmptyFields(safeFieldName, safeFieldValue);
                }
                else
                {
                    doc = XDocument.Parse(db.AdditionalFields);
                    if (doc.Descendants().FirstOrDefault(m => m.Name == safeFieldName) != null)
                    {
                        addAlert("{0} already exists", new string[] { fieldName }, ALERT_DANGER);
                        return View(db);
                    }
                    doc.Root.Add(elem);
                }

                db.AdditionalFields = doc.ToString();
                repository.SaveDatabase(db);
                addAlert("{0} successfully added to {1}", new string[] { fieldName, db.Nickname }, ALERT_SUCCESS);
                return RedirectToAction("List");
            }
        }


        /// <summary>
        /// returns a skeleton xml document with the new field and value added for AdditionalFields that are empty
        /// </summary>
        /// <returns>The newly formed XML document</returns>
        public XDocument createDocumentForEmptyFields(string safeFieldName, string safeFieldValue)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("AdditionalFields");
            root.Add(new XElement(safeFieldName, safeFieldValue));
            doc.Add(root);
            return doc;
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