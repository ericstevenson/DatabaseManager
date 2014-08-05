using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace DatabaseManager.WebUI.Infrastructure.Authentication
{
    public class SaltGenerator
    {
        private static RNGCryptoServiceProvider cryptoServiceProvider = null;
        private const int SALT_SIZE = 24;

        static SaltGenerator()
        {
            cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public static string GetSalt()
        {
            byte[] saltBytes = new byte[SALT_SIZE];
            cryptoServiceProvider.GetNonZeroBytes(saltBytes);
            return Encoding.Default.GetString(saltBytes);
        }
    }
}