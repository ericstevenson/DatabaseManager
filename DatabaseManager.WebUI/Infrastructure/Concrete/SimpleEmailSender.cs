using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.WebUI.Infrastructure.Abstract;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using DatabaseManager.Domain.Entities;

namespace DatabaseManager.WebUI.Infrastructure.Concrete
{
    public class SimpleEmailSender : IEmailProvider
    {
        public Email SendEmail(List<Tuple<string, string>> dbs, string url)
        {
            var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var fromAddress = new MailAddress("estevenson608@gmail.com", "Eric");
            var toAddress = new MailAddress("estevenson608@gmail.com", "To Name");
            string fromPassword = section.Network.Password;
            string subject = "Lawson Informatics Database Administration - Action Required";
            string body = "<div>The following database(s) require attention: <br /></div>";
            body += "<table><tbody><tr><th>Name</th><th>REB Expiry Date</th></tr>";
            foreach (var db in dbs)
            {
                body += "<tr><td>" + db.Item1 + "</td><td>" + db.Item2 + "</td></tr>";
            }
            body += @"</tbody></table><br />Please click <a href=" + url + @">here</a> to address these issues.";

            var smtp = new SmtpClient
            {
                Host = section.Network.Host,
                Port = section.Network.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            return new Email
            {
                To = toAddress.Address,
                From = fromAddress.Address,
                Subject = subject,
                Message = body,
                Sent = DateTime.Now
            };
        }
    }
}