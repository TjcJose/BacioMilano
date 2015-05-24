using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace BM.Util
{
    public static class ByteHelper
    {
        /// <summary>
        /// 结构转换成字节
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object structObj)
        {
            if (structObj.GetType() == typeof(byte[]))
            {
                return (byte[])structObj;
            }
            else
            {
                int size = Marshal.SizeOf(structObj);
                IntPtr buffer = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(structObj, buffer, true);
                    byte[] bytes = new byte[size];
                    Marshal.Copy(buffer, bytes, 0, size);
                    return bytes;
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        /// <summary>
        /// 字节转换成结构
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="strcutType"></param>
        /// <returns></returns>
        public static object BytesToStruct(byte[] bytes, Type structType)
        {
            if (structType == typeof(byte[]))
            {
                return bytes;
            }
            else
            {
                int size = Marshal.SizeOf(structType);
                IntPtr buffer = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.Copy(bytes, 0, buffer, size);
                    return Marshal.PtrToStructure(buffer, structType);
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

        }

        /// <summary>
        /// 字节转换成结构
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="strcutType"></param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] bytes)
        {
            Type t = typeof(T);
            int size = Marshal.SizeOf(t);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, t);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        /// <summary>
        /// 字节转换成结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] bytes, ref long start) where T : new()
        {
            T t = new T();
            var tempByte = new byte[Marshal.SizeOf(t)];
            Array.Copy(bytes, start, tempByte, 0, tempByte.Length);
            t = ByteHelper.BytesToStruct<T>(tempByte);
            start += tempByte.Length;
            return t;
        }

        /// <summary>
        /// 字节转换成结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="size"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] bytes, int size, ref long start)
        {
            var tempByte = new byte[size];
            Array.Copy(bytes, start, tempByte, 0, size);
            var t = ByteHelper.BytesToStruct<T>(tempByte);
            start += size;
            return t;
        }

        /// <summary>
        /// 结构数组转换成字节
        /// </summary>
        /// <param name="structsObj"></param>
        /// <returns></returns>
        public static byte[] StructsToBytes(params object[] structsObj)
        {
            List<byte> ls = new List<byte>();

            foreach (object obj in structsObj)
            {
                ls.AddRange(StructToBytes(obj));
            }

            return ls.ToArray();
        }


        /// <summary>
        /// 字节对象数组合并
        /// </summary>
        /// <param name="structsObj"></param>
        /// <returns></returns>
        public static byte[] MergeData(params byte[][] structsObj)
        {
            List<byte> ls = new List<byte>();

            foreach (byte[] obj in structsObj)
            {
                ls.AddRange(obj);
            }

            return ls.ToArray();
        }


        /// <summary>
        /// 结构数组转换成字节
        /// </summary>
        /// <param name="len"></param>
        /// <param name="structsObj"></param>
        /// <returns></returns>
        public static byte[] StructsToBytes(int len, params object[] structsObj)
        {
            List<byte> ls = new List<byte>(len);

            foreach (object obj in structsObj)
            {
                ls.AddRange(StructToBytes(obj));
            }

            return ls.ToArray();
        }

        /// <summary>
        /// 字节对象数组合并
        /// </summary>
        /// <param name="len"></param>
        /// <param name="structsObj"></param>
        /// <returns></returns>
        public static byte[] MergeData(int len, params byte[][] structsObj)
        {
            List<byte> ls = new List<byte>(len);
            
            foreach (byte[] obj in structsObj)
            {
                ls.AddRange(obj);
            }

            return ls.ToArray();
        }

        /// <summary>
        /// 获取结构大小
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static int GetStructSize(object structObj)
        {
            return Marshal.SizeOf(structObj);
        }

        /// <summary>
        /// 获取结构大小
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static uint GetStructSize(params object[] structsObj)
        {
            uint count = 0;
            foreach (object structObj in structsObj)
            {
                count = (uint)Marshal.SizeOf(structObj) + count;
            }
            return count;
        }

        /// <summary>
        /// 转换数组端列表成数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ls"></param>
        /// <returns></returns>
        public static T[] GetBytesFromArraySegment<T>(IEnumerable<ArraySegment<T>> ls)
        {
            List<T> result = new List<T>();
            foreach (var arr in ls)
            {
                result.AddRange(arr.Array);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <param name="stream">流对象</param>
        /// <returns></returns>
        public static byte[] GetByStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var b = new byte[stream.Length];
            stream.Read(b, 0, b.Length);
            return b;
        }

        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <param name="count">读取数量</param>
        /// <returns></returns>
        public static byte[] ReadBytes(Stream stream, int count)
        {
            var bs = new byte[count];
            var offset = 0;
            for (int n = -1; n != 0 && count > 0; count -= n, offset += n) n = stream.Read(bs, offset, count);
            if (offset != bs.Length) Array.Resize(ref bs, offset);
            return bs;
        }

        /// <summary>
        /// 数据写入
        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <returns></returns>
        public static long Write(Stream inputStream, Stream outputStream)
        {
            long count = 0;
            int size;

            byte[] b = new byte[1024 * 1024];
            while ((size = inputStream.Read(b, 0, b.Length)) > 0)
            {
                outputStream.Write(b, 0, size);
                count += size;
            }

            return count;
        }


      

    }

}
