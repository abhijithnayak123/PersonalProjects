using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MGI.Common.Util
{
    public static class PasswordHelper
    {
        public static string CreateSalt(int size)
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }       

        public static bool MeetsPasswordRequirements(string newPassword)
        {
            return newPassword.Length >= 8 && newPassword.Length <= 20 && Regex.IsMatch(newPassword, @"\d") && Regex.IsMatch(newPassword, @"[A-Z]");
        }
    }
}
