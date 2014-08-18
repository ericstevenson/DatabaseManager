using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Infrastructure.Abstract
{
    public interface IEmailProvider
    {
        Email SendEmail(List<Tuple<string, string>> datesAndNames, string url);
    }
}
