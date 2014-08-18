using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.Domain.Entities
{
    public class PeriodicFinance
    {
        public int Id { get; set; }
        public int DatabaseID { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double MonthlyRate { get; set; }
    }
}
