using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Models
{
    public class EditViewModel
    {
        public LawsonDatabase Database { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}