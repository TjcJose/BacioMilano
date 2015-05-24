using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Net
{
    public static class IPAddressHelper
    { 
        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ushort NetworkOrHostOrder(ushort host)
        {
            //byte[] tmp = BitConverter.GetBytes(param);
            //Array.Reverse(tmp);
            //return BitConverter.ToUInt16(tmp, 0);
            return (ushort)(((host & 0xff) << 8) | ((host >> 8) & 0xff));

        }

        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static uint NetworkOrHostOrder(uint host)
        {
            return (((NetworkOrHostOrder((ushort)host) & 0xffffU) << (0x10)) | (NetworkOrHostOrder((ushort)(host >> (0x10))) & 0xffffU));
        }

        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ulong NetworkOrHostOrder(ulong host)
        {
            return (ulong)(((NetworkOrHostOrder((uint)host) & 0xffffffffUL) << 0x20) | (NetworkOrHostOrder((uint)(host >> 0x20)) & 0xffffffffUL));
        }

        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static short NetworkOrHostOrder(short host)
        {
            return (short)(((host & 0xff) << 8) | ((host >> 8) & 0xff));
        }

        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int NetworkOrHostOrder(int host)
        {
            return (((NetworkOrHostOrder((short)host) & 0xffff) << 0x10) | (NetworkOrHostOrder((short)(host >> 0x10)) & 0xffff));
        }

        /// <summary>
        /// 网络字节序到主机字节序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static long NetworkOrHostOrder(long host)
        {
            return (long)(((NetworkOrHostOrder((int)host) & 0xffffffffL) << 0x20) | (NetworkOrHostOrder((int)(host >> 0x20)) & 0xffffffffL));
        }


    }
}
