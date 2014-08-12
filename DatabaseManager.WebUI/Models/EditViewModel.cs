using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.WebUI.Models
{
    public class EditViewModel
    {
        [Display(Name = "Lawson Database ID")]
        public int LawsonDatabaseID { get; set; }

        [Display(Name = "Status on Server")]
        public string OnServerStatus { get; set; }

        [Display(Name = "Database Status")]
        public string DatabaseStatus { get; set; }

        public string Platform { get; set; }

        [Display(Name = "Principal Investigator")]
        public string PIName { get; set; }

        public string Nickname { get; set; }

        public string Name { get; set; }

        public string Developer { get; set; }

        [Display(Name = "Study Title")]
        public string StudyTitle { get; set; }

        public string REB { get; set; }

        [Display(Name = "Lawson Number")]
        public string LawsonNumber { get; set; }

        [Display(Name = "REB Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? REBExpiry { get; set; }

        [Display(Name = "Lawson Approval Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LawsonApprovalDate { get; set; }

        public string Researcher { get; set; }

        [Display(Name = "Invoice Contact")]
        public string InvoiceContact { get; set; }

        [Display(Name = "Invoice Contact Email")]
        public string InvoiceContactEmail { get; set; }

        public string AdditionalFields { get; set; }

        public Dictionary<string, string> AdditionalFieldsDictionary { get; set; }
    }
}