using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Models
{
    public class EditFinanceViewModel
    {
        public List<PeriodicFinance> Finances { get; set; }
        public LawsonDatabase Database { get; set; }
    }
}