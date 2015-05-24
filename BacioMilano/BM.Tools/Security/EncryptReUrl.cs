using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BM.Util;

namespace BM.Security
{
    public class EncryptReUrl : IEncryptRe
    {
       private static SymmetrySecret secret;
       static EncryptReUrl()
       {
           secret = new SymmetrySecret("");
       }

        public string EncryptString(string originalStr)
        {
            secret.CryptText = originalStr;
            string s = secret.Encrypt().Replace("=", "a123456a").Replace("?", "b123456b").Replace("/", "c123456c").Replace("&", "d123456d").Replace(@"\", "e123456e").Replace("+", "f123456f").Replace(",", "e99099e");
            return SwapString(s);
        }

        public string DecryptString(string encryptStr)
        {
            encryptStr = SwapString(encryptStr);
            secret.CryptText = encryptStr.Replace("a123456a", "=").Replace("b123456b", "?").Replace("c123456c", "/").Replace("d123456d", "&").Replace("e123456e", @"\").Replace("f123456f", "+").Replace("e99099e", ",");
            return secret.Decrypt();
        }

        public static EncryptReUrl Instance
        {
            get
            {
                return SingletonHelper<EncryptReUrl>.Instance;
            }
        }

        private static string SwapString(string s)
        {

            int level = 5;
            level = s.Length % 11;
            level = (level + 7) % 13;

            if (level == 0 || level == 1 || level == 2 || level == 3 || level == 4)
            {
                level = 5;
            }

            return SwapString(s, level);
        }

        private static string SwapString(string s, int level)
        {
            if (s.Length == 1 || s.Length == 0 || level == 0)
            {
                return s;
            }

            string s1;
            string s2;
            string s3;

            int mid = s.Length / 2;
            if (s.Length % 2 == 0)
            {
                s1 = s.Substring(mid);
                s2 = "";
                s3 = s.Substring(0, mid);
            }
            else
            {
                s1 = s.Substring(mid + 1);
                s2 = s[mid].ToString();
                s3 = s.Substring(0, mid);
            }

            if (level == 0)
            {
                return s1 + s2 + s3;
            }
            else
            {
                level--;
                return SwapString(s1, level) + s2 + SwapString(s3, level);
            }

        }
    }
}
