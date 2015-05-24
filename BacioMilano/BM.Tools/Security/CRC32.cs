using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BM.Security
{
    public class CRC32
    {
        private static CRC32 crc32 = null;

        /// <summary>
        /// 将字符串转化为指纹
        /// </summary>
        /// <param name="sInputString">想要生成指纹的字符串</param>
        /// <returns>长整型的指纹</returns>
        public ulong BuildFingerprint(string sInputString)
        {
            byte[] buffer = Encoding.Default.GetBytes(sInputString);
            return ByteCRC(ref buffer);
        }

        protected ulong[] crc32Table;
        private ulong ulPolynomial = 0xEDB88320;

        /// <summary>
        /// 构造函数
        /// </summary>
        private CRC32()
        {
            this.InitializeTable();
        }

        private void InitializeTable()
        {
            ulong dwCrc;
            crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                dwCrc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((dwCrc & 1) == 1)
                        dwCrc = (dwCrc >> 1) ^ ulPolynomial;
                    else
                        dwCrc >>= 1;
                }
                crc32Table[i] = dwCrc;
            }
        }

        /// <summary>
        /// 将字节数组转化为指纹
        /// </summary>
        /// <param name="buffer">引用的字节数组</param>
        /// <returns>无符号长整型指纹</returns>
        public ulong ByteCRC(ref byte[] buffer)
        {
            ulong ulCRC = 0xffffffff;
            ulong len;
            len = (ulong)buffer.Length;
            for (ulong buffptr = 0; buffptr < len; buffptr++)
            {
                ulong tabPtr = ulCRC & 0xFF;
                tabPtr = tabPtr ^ buffer[buffptr];
                ulCRC = ulCRC >> 8;
                ulCRC = ulCRC ^ crc32Table[tabPtr];
            }
            return ulCRC ^ 0xffffffff;
        }

        /// <summary>
        /// 生成文件效验指纹
        /// </summary>
        /// <param name="sInputFilename">文件名</param>
        /// <returns>长整型的指纹</returns>
        public ulong FileCRC(string sInputFilename)
        {
            FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bInput = new byte[inFile.Length];
            inFile.Read(bInput, 0, bInput.Length);
            inFile.Close();
            return ByteCRC(ref bInput);
        }

        /// <summary>
        /// 获取该类的实例
        /// </summary>
        /// <returns></returns>
        public static CRC32 Instance
        {
            get
            {
                if (crc32 == null)
                    crc32 = new CRC32();
                return crc32;
            }
        }
    }
}
