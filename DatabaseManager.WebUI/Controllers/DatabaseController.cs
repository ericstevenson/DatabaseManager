﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseManager.Domain.Entities;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Concrete;
using DatabaseManager.WebUI.Infrastructure;
using DatabaseManager.WebUI.Models;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace DatabaseManager.WebUI.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        public const string ALERT_SUCCESS = "alert-success";
        public const string ALERT_DANGER = "alert-danger";
        public const string GLYPHICON_WARNING = "glyphicon glyphicon-warning-sign red";
        public const string BORDER_RED = "border-red";
        public const string DEFAULT_SORTING_ORDER = "Nickname";

        private ILawsonDatabaseRepository repository;

        public DatabaseController(ILawsonDatabaseRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string sortingOrder = DEFAULT_SORTING_ORDER, bool welcome = false)
        {
            var databases = repository.LawsonDatabases;
            ViewBag.SortingOrder = sortingOrder;
            ViewBag.REBExpiry = new Dictionary<int, string>();
            ViewBag.REBExpiredAlert = false;

            foreach (var db in databases)
            {
                if (db.REBExpiry != null)
                {

                    double difference = Math.Abs(((db.REBExpiry ?? DateTime.MinValue) - DateTime.Now).TotalDays);
                    if (db.REBExpiry < DateTime.Now && welcome)
                    {
                        ViewBag.REBExpiredAlert = true;
                    }
                    if (difference < 30 || db.REBExpiry < DateTime.Now)
                    {
                        ViewBag.REBExpiry[db.LawsonDatabaseID] = GLYPHICON_WARNING;
                    }
                    else
                    {
                        ViewBag.REBExpiry[db.LawsonDatabaseID] = "";
                    }
                }
                else
                {
                    ViewBag.REBExpiry[db.LawsonDatabaseID] = "";
                }
            }
            if (ViewBag.REBExpiredAlert)
            {
                ViewBag.ExpiredDbs = databases.Where(d => d.REBExpiry < DateTime.Now);
            }
            return View(databases);
        }

        public ViewResult Edit(int lawsonDatabaseID)
        {
            var db = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == lawsonDatabaseID);
            var tempFields = db.AdditionalFields;
            ViewBag.AdditionalFields = tempFields != null ? XDocument.Parse(tempFields).Descendants("AdditionalFields")
                .Elements()
                .ToDictionary(r => toSafeFieldName(r.Name.ToString()), r => toSafeFieldName(r.Value.ToString())) : new Dictionary<string, string>();

            return View(db);
        }

        public ViewResult EditTemp(int lawsonDatabaseID)
        {
            var db = repository.LawsonDatabases.FirstOrDefault(d => d.LawsonDatabaseID == lawsonDatabaseID);
            string tempFields = db.AdditionalFields;
            var x = tempFields != null ? XDocument.Parse(tempFields).Descendants("AdditionalFields")
                .Elements()
                .ToDictionary(r => rawName(r.Name.ToString()), r => rawName(r.Value.ToString())) : new Dictionary<string, string>();

            return View(new EditViewModel
            {
                LawsonApprovalDate = db.LawsonApprovalDate,
                LawsonNumber = db.LawsonNumber,
                DatabaseStatus = db.DatabaseStatus,
                Developer = db.Developer,
                InvoiceContact = db.InvoiceContact,
                InvoiceContactEmail = db.InvoiceContactEmail,
                StudyTitle = db.StudyTitle,
                OnServerStatus = db.OnServerStatus,
                Nickname = db.Nickname,
                REB = db.REB,
                REBExpiry = db.REBExpiry,
                Researcher = db.Researcher,
                PIName = db.PIName,
                Platform = db.Platform,
                LawsonDatabaseID = db.LawsonDatabaseID,
                Name = db.Name,
                AdditionalFields = x
            });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditTemp(EditViewModel model)
        {
            
            LawsonDatabase db = repository.LawsonDatabases.FirstOrDefault(m => m.LawsonDatabaseID == model.LawsonDatabaseID);
            db.LawsonApprovalDate = model.LawsonApprovalDate;
            db.LawsonNumber = model.LawsonNumber;
            db.DatabaseStatus = model.DatabaseStatus;
            db.Developer = model.Developer;
            db.InvoiceContact = model.InvoiceContact;
            db.InvoiceContactEmail = model.InvoiceContactEmail;
            db.StudyTitle = model.StudyTitle;
            db.OnServerStatus = model.OnServerStatus;
            db.Nickname = model.Nickname;
            db.REB = model.REB;
            db.REBExpiry = model.REBExpiry;
            db.Researcher = model.Researcher;
            db.PIName = model.PIName;
            db.Platform = model.Platform;
            db.LawsonDatabaseID = model.LawsonDatabaseID;
            db.Name = model.Name;

            Dictionary<string, string> additionalFields = model.AdditionalFields;
            XDocument doc = new XDocument();
            XElement root = new XElement("AdditionalFields");
            foreach (var kvb in model.AdditionalFields)
            {
                root.Add(new XElement(toSafeFieldName(kvb.Key), toSafeFieldName(kvb.Value)));
            }
            doc.Add(root);
            db.AdditionalFields = doc.ToString();

            if (ModelState.IsValid)
            {
                repository.SaveDatabase(db);
                addAlert("{0} has been saved", new string[] { db.Nickname }, ALERT_SUCCESS);
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
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
                addAlert("You did not enter a field name. Please enter the field name before continuing.", new string[] { }, ALERT_DANGER);
                return View(db);
            }

            // Replace spaces with unused character (for xml formatting)
            string safeFieldName = toSafeFieldName(fieldName);
            string safeFieldValue = toSafeFieldName(fieldValue);

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
                        if (doc.Descendants().FirstOrDefault(m => String.Equals(m.Name.ToString(), safeFieldName, StringComparison.OrdinalIgnoreCase)) != null)
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
                    if (doc.Descendants().FirstOrDefault(m => String.Equals(m.Name.ToString(), safeFieldName, StringComparison.OrdinalIgnoreCase)) != null)
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
        /// Exports database information to a file that can be opened in excel
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            StringBuilder str = new ExcelStringBuilder().Build(repository.LawsonDatabases.OrderBy(d => d.DatabaseStatus));
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=LawsonServerDatabases" + DateTime.Now.ToShortDateString() + ".xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            byte[] temp = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            return File(temp, "application/vnd.ms-excel");
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

        public string toSafeFieldName(string message)
        {
            return message.Replace(" ", "SpaceDummyChar");
        }

        public string rawName(string message)
        {
            return message.Replace("SpaceDummyChar", " ");
        }
    }
}