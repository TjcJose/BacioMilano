using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BM.Util;

namespace BM.Security
{
    public class EncryptRe : IEncryptRe
    {
        private static SymmetrySecret secret;
        static EncryptRe()
        {
            secret = new SymmetrySecret("");
        }
        public string EncryptString(string originalStr)
        {
            secret.CryptText = originalStr;
            return secret.Encrypt();
        }

        public string DecryptString(string encryptStr)
        {
            secret.CryptText = encryptStr;
            return secret.Decrypt();
        }

        public static EncryptRe Instance
        {
            get
            {
                return SingletonHelper<EncryptRe>.Instance;
            }
        }
    }
}
