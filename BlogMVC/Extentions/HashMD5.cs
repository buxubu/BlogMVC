using System.Security.Cryptography;
using System.Text;
using System;

namespace BlogMVC.Extentions
{
    public static class Share
    {
        public static string ToMD5(string pass)
        {
            MD5 hash = MD5.Create();

            byte[] bHash = hash.ComputeHash(Encoding.UTF8.GetBytes(pass));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(string.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        public static string GetSailt()
        {
            byte[] salt = new byte[5];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

    }
}
