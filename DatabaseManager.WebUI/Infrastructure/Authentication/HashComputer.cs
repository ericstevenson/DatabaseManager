using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;

namespace DatabaseManager.WebUI.Infrastructure.Authentication
{
    public class HashComputer
    {
        public string GetPasswordHashAndSalt(string message)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte [] dataBytes = Encoding.Default.GetBytes(message);
            byte [] resultBytes = sha.ComputeHash(dataBytes);
            return Encoding.Default.GetString(resultBytes);
        }
    }
}