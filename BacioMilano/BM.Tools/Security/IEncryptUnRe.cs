using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Security
{
    /// <summary>
    /// 解密接口
    /// </summary>
    public interface IEncryptUnRe
    {
        /// <summary>
        /// 不可逆加密
        /// </summary>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        string EncryptString(string originalStr);
    }
}
