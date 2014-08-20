using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager.WebUI.Infrastructure.Abstract
{
    public interface IPaymentProvider
    {
        double CalculatePayment(double rate, DateTime start, DateTime end);
    }
}
