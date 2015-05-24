using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BM.Security
{
    /// <summary>
    ///可逆的通用对称加密解密函数集                         
    /// </summary>
    internal class SymmetrySecret
    {
        #region 私有变量

        /// <summary>
        /// 待加密和解密的字符序列变量
        /// </summary>
        private string _CryptText;
        /// <summary>
        /// 加密解密私钥变量
        /// </summary>
        private byte[] _CryptKey;
        /// <summary>
        /// 加密解密初始化向量IV变量
        /// </summary>
        private byte[] _CryptIV;

        #endregion

        #region 属性
        /// <summary>
        /// 待加密或解密的字符序列
        /// </summary>
        public string CryptText
        {
            set
            {
                _CryptText = value;
            }
            get
            {
                return _CryptText;
            }
        }

        /// <summary>
        /// 加密私钥
        /// </summary>
        public byte[] CryptKey
        {
            set
            {
                _CryptKey = value;
            }
            get
            {
                return _CryptKey;
            }
        }

        /// <summary>
        /// 加密的初始化向量IV
        /// </summary>
        public byte[] CryptIV
        {
            set
            {
                _CryptIV = value;
            }
            get
            {
                return _CryptIV;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cryptText">待加密和解密的字符序列变量</param>
        /// <param name="cryptKey">加密解密私钥向量Key变量</param>
        /// <param name="cryptIV">加密解密向量IV变量</param>
        public SymmetrySecret(string cryptText, byte[] cryptKey, byte[] cryptIV)
        {
            this.CryptText = cryptText;
            this.CryptKey = cryptKey;
            this.CryptIV = cryptIV;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cryptText">待加密和解密的字符序列变量</param>
        /// <param name="key">加密解密私钥变量字符串</param>
        /// <param name="IV">加密解密IV变量字符串</param>
        public SymmetrySecret(string cryptText, string key, string IV)
        {
            this.CryptText = cryptText;
            this.CryptKey = Convert.FromBase64String(key);
            this.CryptIV = Convert.FromBase64String(IV);
        }
        public SymmetrySecret(string cryptText)
        {
            this.CryptText = cryptText;
            this.CryptKey = Convert.FromBase64String("0t3q4fHJnyAVndj66+gBmmj6FkemW3xt/7uOgMHoKLg=");
            this.CryptIV = Convert.FromBase64String("DZVKpnvwxRhErICEoMTTOw==");
        }
        
        #endregion

        #region Encrypt 加密函数
        /// <summary>
        /// 加密函数,用于对字符串进行加密。需要提供相应的密钥和IV。
        /// </summary>
        /// <returns></returns>
        public string Encrypt()
        {
            string strEnText = CryptText;
            byte[] EnKey = CryptKey;
            byte[] EnIV = CryptIV;

            byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(strEnText);

            //此处也可以创建其他的解密类实例，但注意不同(长度)的加密类要求不同的密钥Key和初始化向量IV
            RijndaelManaged RMCrypto = new RijndaelManaged();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, RMCrypto.CreateEncryptor(EnKey, EnIV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region Decrypt 解密函数
        /// <summary>
        /// 解密函数，用于经过加密的字符序列进行加密。需要提供相应的密钥和IV。
        /// </summary>
        /// <returns></returns>
        public string Decrypt()
        {
            string strDeText = CryptText;
            byte[] DeKey = CryptKey;
            byte[] DeIV = CryptIV;

            byte[] inputByteArray = Convert.FromBase64String(strDeText);

            //此处也可以创建其他的解密类实例，但注意不同的加密类要求不同(长度)的密钥Key和初始化向量IV
            RijndaelManaged RMCrypto = new RijndaelManaged();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, RMCrypto.CreateDecryptor(DeKey, DeIV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion
    }
}

