using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BM.Util
{
    /// <summary>
    /// byte扩展
    /// </summary>
    public static class ByteEx
    {
        /// <summary>
        /// 查询某字节数组中的一段字节数组的位置
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int IndexOfValues(this byte[] array, byte[] value, int startIndex)
        {
            if (array == null)
                return -1;
            var index = 0;
            var start = Array.IndexOf(array, value[0], startIndex);

            if (start == -1)
            {
                return -1;
            }

            while ((start + index) < array.Length)
            {
                if (array[start + index] == value[index])
                {
                    index++;
                    if (index == value.Length)
                    {
                        return start;
                    }
                }
                else
                {
                    start = Array.IndexOf(array, value[0], start + index);

                    if (start != -1)
                    {
                        index = 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            return -1;
        }

        public static byte[] ChangeToSize(this byte[] array, int size)
        {
            if (array.Length < size)
            {
                return array.AppendBuffer(size - array.Length);
            }
            if (array.Length == size)
            {
                return array;
            }
            else
            {
                return array.SubBuffer(0, size);
            }

        }

        /// <summary>
        /// 返回新数组，长度为原数组长度加size,并复制原数组内容到新数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] AppendBuffer(this byte[] array, int size)
        {
            if (array == null || array.Length == 0)
            {
                if (size <= 0)
                    return new byte[0];
                else
                    return new byte[size];
            }

            var t = new byte[size + array.Length];
            Buffer.BlockCopy(array, 0, t, 0, array.Length);
            return t;
        }

        /// <summary>
        /// 返回新数组，长度为原数组长度加size,并复制原数组内容加buffer内容到新数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] AppendBuffer(this byte[] array, byte[] buffer, int size)
        {
            if (size < 0)
                size = 0;

            if (size > buffer.Length)
                size = buffer.Length;

            var dst = array.AppendBuffer(size);

            if (dst == null)
                return new byte[0]; ;

            Buffer.BlockCopy(buffer, 0, dst, array.Length, size);
            return dst;
        }

        /// <summary>
        /// 截取某段Array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBuffer(this byte[] array, int startIndex, int length)
        {
            if (length <= 0)
                return new byte[0];

            if (array == null)
                return new byte[0];

            int size = array.Length - startIndex;
            if (size < length)
            {
                length = size;
            }

            byte[] arr = new byte[length];
            Buffer.BlockCopy(array, startIndex, arr, 0, length);
            return arr;
        }

        /// <summary>
        /// 截取某段Array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBuffer(this byte[] array, int startIndex)
        {

            if (array == null)
                return new byte[0];

            int size = array.Length - startIndex;
            if (size < 0)
                return new byte[0];

            byte[] arr = new byte[size];
            Buffer.BlockCopy(array, startIndex, arr, 0, size);
            return arr;
        }

        public static byte[] ToBytes(this int n)
        {
            return System.BitConverter.GetBytes(n);
            //byte[] b = new byte[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    b[i] = (byte)(n >> (24 - i * 8));
            //}
            //return b;
        }

        public static int ToInt(this byte[] b)
        {
            return ((((b[0] << 24) | (b[1] << 16)) | (b[2] << 8)) | b[3]);

            return (((int)b[0]) << 24) + (((int)b[1]) << 16)
                    + (((int)b[2]) << 8) + b[3];
        }

        public static long ToLong(this byte[] b)
        {
            int num3 = (((b[0] << 24) | (b[1] << 16)) | (b[2] << 8)) | b[3];
            int num4 = (((b[4] << 24) | (b[5] << 16)) | (b[6] << 8)) | b[7];
            return (((long)((ulong)num4)) | (num3 << 32));

        }

        public static int NetworkToHostInt(this byte[] b)
        {
            return IPAddress.NetworkToHostOrder(System.BitConverter.ToInt32(b, 0));
            //return (((int)b[0]) << 24) + (((int)b[1]) << 16)
            //        + (((int)b[2]) << 8) + b[3];
        }

        public static float NetworkToHostFloat(this byte[] b)
        {
            return BitConverter.ToSingle(b.Reverse().ToArray(), 0);  
        }

       
    }
}
