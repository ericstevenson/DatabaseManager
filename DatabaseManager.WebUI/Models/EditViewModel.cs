using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Models
{
    public class EditViewModel
    {
        public int LawsonDatabaseID { get; set; }
        public string OnServerStatus { get; set; }
        public string DatabaseStatus { get; set; }
        public string Platform { get; set; }
        public string PIName { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public string StudyTitle { get; set; }
        public string REB { get; set; }
        public string LawsonNumber { get; set; }
        public DateTime? REBExpiry { get; set; }
        public DateTime? LawsonApprovalDate { get; set; }
        public string Researcher { get; set; }
        public string InvoiceContact { get; set; }
        public string InvoiceContactEmail { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}