using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.WebUI.Infrastructure.Abstract;
using DatabaseManager.WebUI.Infrastructure.Authentication;
using System.Web.Security;

namespace DatabaseManager.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        private PasswordManager passwordManager;

        public FormsAuthProvider()
        {
            passwordManager = new PasswordManager();
        }

        public bool Authenticate(string userName, string password, string salt, string hash)
        {
            bool result = passwordManager.IsPasswordMatch(password, salt, hash);

            if (result)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
            }
            return result;
        }
    }
}