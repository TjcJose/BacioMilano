using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Security
{
    public class MD5
    {
        private static MD5 md5 = null;

        private MD5()
        {
           
        }



        public string BuildFingerprint(string str)
        {
            byte[] b = System.Text.UTF8Encoding.UTF32.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static MD5 Instance
        {
            get
            {
                if (md5 == null)
                    md5 = new MD5();
                return md5;
            }
        }

       
    }
}
