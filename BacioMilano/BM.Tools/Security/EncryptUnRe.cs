using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using BM.Util;


namespace BM.Security
{
    public class EncryptUnRe : IEncryptUnRe
    {
        private static MD5CryptoServiceProvider md5csp;

        static EncryptUnRe()
        {
            md5csp = new MD5CryptoServiceProvider();
        }
        #region IEncryptUnRe Members

        public string EncryptString(string originalStr)
        {
            byte[] MD5Source = System.Text.Encoding.UTF8.GetBytes(originalStr);
            byte[] MD5Out = md5csp.ComputeHash(MD5Source);
            return Convert.ToBase64String(MD5Out);
        }

        #endregion

        public static EncryptRe Instance
        {
            get
            {
                return SingletonHelper<EncryptRe>.Instance;
            }
        }

    }
}
