using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BM.Util;

namespace BM.IO
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 读入文件到字节数组
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static byte[] ReadToByte(string filePath)
        {
            List<byte> ls = new List<byte>();
            byte[] block = new byte[1024];
            using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                int length = 1;
                while (length > 0)
                {
                    length = stream.Read(block, 0, 1024);
                    if (length > 0)
                    {
                        ls.AddRange(block.SubBuffer(0, length));
                    }
                }
                return ls.ToArray();
            }
        }

        public static string ReadToString(string filePath)
        {
            using (System.IO.StreamReader sr = new StreamReader(filePath))
            {
               return sr.ReadToEnd();
            }
        }

      
    }
}
