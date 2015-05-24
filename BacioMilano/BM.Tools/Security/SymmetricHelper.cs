using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BM.Security
{
    /// <summary>
    /// 对称加密算法帮助类
    /// </summary>
    public static class SymmetricHelper
    {
        private const int SALT_BYTES_LENGTH = 16;

        /// <summary>
        /// 生成随机加密向量和密钥
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        public static void GenerateKey(AlgorithmType type, out byte[] iv, out byte[] key)
        {
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create(type.ToString());
            algorithm.GenerateKey();
            algorithm.GenerateIV();

            iv = algorithm.IV;
            key = algorithm.Key;
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="source">被加密的数据</param>
        /// <returns>加密后的数据</returns>
        public static byte[] Encrypt(AlgorithmType type, byte[] iv, byte[] key, byte[] source)
        {
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create(type.ToString());

            using (MemoryStream outStream = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(outStream, algorithm.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    stream.Write(source, 0, source.Length);
                    stream.FlushFinalBlock();

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="source">被解密的数据</param>
        /// <returns>解密后的数据</returns>
        public static byte[] Decrypt(AlgorithmType type, byte[] iv, byte[] key, byte[] source)
        {
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create(type.ToString());

            using (MemoryStream outStream = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(outStream, algorithm.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    stream.Write(source, 0, source.Length);
                    stream.FlushFinalBlock();

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="s">被解密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(AlgorithmType type, byte[] iv, byte[] key, string s)
        {
            byte[] source = Encoding.Unicode.GetBytes(s);
            return Convert.ToBase64String(Encrypt(type, iv, key, source));
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="s">被解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptString(AlgorithmType type, byte[] iv, byte[] key, string s)
        {
            byte[] source = Convert.FromBase64String(s);
            return Encoding.Unicode.GetString(Decrypt(type, iv, key, source));
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <remarks>
        /// 加入随机干扰数据，使得同一数据每次加密结果均不相同。
        /// </remarks>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="source">被加密的数据</param>
        /// <returns>加密后的数据</returns>
        public static byte[] EncryptSalt(AlgorithmType type, byte[] iv, byte[] key, byte[] source)
        {
            byte[] salts = new byte[SALT_BYTES_LENGTH];
            new RNGCryptoServiceProvider().GetBytes(salts);

            byte[] newSource = new byte[salts.Length + source.Length];
            Buffer.BlockCopy(salts, 0, newSource, 0, salts.Length);
            Buffer.BlockCopy(source, 0, newSource, salts.Length, source.Length);

            return Encrypt(type, iv, key, newSource);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="source">解密后的数据</param>
        /// <returns>解密后的数据</returns>
        public static byte[] DecryptSalt(AlgorithmType type, byte[] iv, byte[] key, byte[] source)
        {
            byte[] dec = Decrypt(type, iv, key, source);

            byte[] result = new byte[dec.Length - SALT_BYTES_LENGTH];
            Buffer.BlockCopy(dec, SALT_BYTES_LENGTH, result, 0, result.Length);

            return result;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <remarks>
        /// 加入随机干扰数据，使得同一字符串每次加密结果均不相同。
        /// </remarks>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="s">被解密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptStringSalt(AlgorithmType type, byte[] iv, byte[] key, string s)
        {
            byte[] source = Encoding.Unicode.GetBytes(s);
            return Convert.ToBase64String(EncryptSalt(type, iv, key, source));
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="type">加密算法类型</param>
        /// <param name="iv">加密向量</param>
        /// <param name="key">加密键</param>
        /// <param name="s">被解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptStringSalt(AlgorithmType type, byte[] iv, byte[] key, string s)
        {
            byte[] source = Convert.FromBase64String(s);
            return Encoding.Unicode.GetString(DecryptSalt(type, iv, key, source));
        }
    }
}
