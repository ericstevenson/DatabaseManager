using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace EmailServices
{
    public class EmailService
    {
        public static void Main()
        {
            const string queryString = Resources.Passwords.QUERY_KEY;
            const string url = "http://localhost:50270/Email/SendEmail?from=" + queryString;
           // Uri uri = new Uri(url + "/Email/SendEmail?from=" + queryString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return;
        }
    }
}
