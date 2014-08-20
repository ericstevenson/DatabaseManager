﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Models
{
    public class EditFinanceViewModel
    {
        public string DatabaseName { get; set; }
        public Finance NewInvoice { get; set; }
    }
}