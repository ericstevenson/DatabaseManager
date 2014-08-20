using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Models
{
    public class FinanceListViewModel
    {
        public List<Tuple<string, Finance>> FinanceList { get; set; }
    }
}