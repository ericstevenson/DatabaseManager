using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseManager.WebUI.Infrastructure.Authentication
{
    public class PasswordManager
    {
        HashComputer hashComputer = new HashComputer();

        public Tuple<string, string> GeneratePasswordHash(string plainTextPasword)
        {
            string salt = SaltGenerator.GetSalt();
            string hashedPassword = hashComputer.GetPasswordHashAndSalt(plainTextPasword + salt);
            return new Tuple<string, string>(salt, hashedPassword);
        }

        public bool IsPasswordMatch(string password, string salt, string hash)
        {
            string finalString = password + salt;
            return hash == hashComputer.GetPasswordHashAndSalt(finalString);
        }
    }
}