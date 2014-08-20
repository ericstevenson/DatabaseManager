using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.WebUI.Infrastructure.Abstract;

namespace DatabaseManager.WebUI.Infrastructure.Concrete
{
    public class PaymentCalculator : IPaymentProvider
    {
        public const int DAYS_IN_MONTH = 30;
        public double CalculatePayment(double ratePerMonth, DateTime start, DateTime end)
        {
            double months = Math.Ceiling((end - start).TotalDays/30);
            return months * ratePerMonth;
        }
    }
}