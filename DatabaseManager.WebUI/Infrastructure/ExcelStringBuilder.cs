using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Domain.Entities;
using System.Text;

namespace DatabaseManager.WebUI.Infrastructure
{
    public class ExcelStringBuilder
    {
        public StringBuilder Build(IEnumerable<LawsonDatabase> data)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<table>");
            str.Append("<tr><font size=5>Index</font></tr>");
            str.Append("<tr><b>");
            str.Append("<td>PI</td>");
            str.Append("<td>Nickname</td>");
            str.Append("<td>Database Status</td>");
            str.Append("<td>Name on Server</td>");
            str.Append("<td>Status on Server</td>");
            str.Append("</tr>");
            foreach (var db in data)
            {
                str.Append("<td>" + db.PIName + "</td>");
                str.Append("<td>" + db.Nickname + "</td>");
                str.Append("<td>" + db.DatabaseStatus + "</td>");
                str.Append("<td>" + db.Name + "</td>");
                str.Append("<td>" + db.OnServerStatus + "</td>");
                str.Append("</tr>");
            }
            str.Append("</table>");
            return str;
        }
    }
}