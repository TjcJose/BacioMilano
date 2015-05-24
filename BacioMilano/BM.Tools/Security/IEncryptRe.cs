using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Security
{
    /// <summary>
    /// 可逆加密接口
    /// </summary>
    public interface IEncryptRe
    {
        /// <summary>
        /// 可逆加密
        /// </summary>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        string EncryptString(string originalStr);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        string DecryptString(string encryptStr);
    }
}
