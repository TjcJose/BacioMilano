using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BM.Util;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using BM.Log;



namespace BM.Net
{
    /// <summary>
    /// 数据包异步接受实现
    /// </summary>
    public abstract class ReceiveAsynDataPack : ReceiveAsyn
    {
        /// <summary>
        /// 当前还没处理的数据
        /// </summary>
        private byte[] currentBuffer;

        private byte[] headBeginSign;
        /// <summary>
        /// 字符编码
        /// </summary>
        protected Encoding netEncoding;

        /// <summary>
        /// 数据包异步接受构造函数
        /// </summary>
        /// <param name="netEncoding">字符编码</param>
        public ReceiveAsynDataPack(Encoding netEncoding)
            : base()
        {
            this.netEncoding = netEncoding;
            this.headBeginSign = this.netEncoding.GetBytes(T_DataHead.UseBeginSign);
            this.currentBuffer = null;
        }

        /// <summary>
        /// 数据头
        /// </summary>
        protected T_DataHead head;

        /// <summary>
        /// 读取完成后的处理
        /// </summary>
        /// <returns></returns>
        protected abstract void ReveiveFinish();
        
        /// <summary>
        /// 接受处理
        /// </summary>
        /// <param name="buffer">处理的数据</param>
        protected override void ReveiveDeal(byte[] buffer)
        {
            this.Read(buffer);
        }

        /// <summary>
        /// 第一次接受处理
        /// </summary>
        /// <param name="buffer">处理的数据</param>
        protected override void ReveiveDealFirst(byte[] buffer)
        {


            if (currentBuffer == null)
            {
                currentBuffer = buffer;
            }
            else
            {
                currentBuffer = currentBuffer.AppendBuffer(buffer, buffer.Length);
            }
            
            int len = currentBuffer.Length - T_DataHead.HeadLen;
            if (len >= 0)
            {
                int startIndex = currentBuffer.IndexOfValues(headBeginSign, 0);
                if (startIndex >= 0)
                {
                    byte[] b = new byte[T_DataHead.HeadLen];
                    Array.Copy(currentBuffer, startIndex, b, 0, T_DataHead.HeadLen);
                    this.head = ByteHelper.BytesToStruct<T_DataHead>(b);
                    if (this.head.iBeginSign != T_DataHead.UseBeginSign)
                    {
                        currentBuffer = currentBuffer.SubBuffer(currentBuffer.Length - T_DataHead.HeadLen + 1);
                        return;
                    }

                    this.totalDataLen = this.head.iLen;
                    if (len > 0)
                    {
                        b = new byte[len - startIndex];
                        len = currentBuffer.Length - T_DataHead.HeadLen - startIndex;
                        startIndex = T_DataHead.HeadLen + startIndex;
                        if (currentBuffer.Length - startIndex >= len && len > 0)
                        {
                            Array.Copy(currentBuffer, startIndex, b, 0, len);
                            this.Read(b);
                        }
                    }
                    else if (len == 0)
                    {
                        this.ReveiveFinish();
                    }
                }
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        protected abstract void ReadBuffer(byte[] buffer);

        protected void Read(byte[] buffer)
        {
            if (this.readIndex + buffer.Length > this.head.iLen)
            {
                byte[] b = new byte[this.head.iLen - this.readIndex];
                Array.Copy(buffer, b, b.Length);

                int len = buffer.Length - b.Length;
                currentBuffer = new byte[len];
                Array.Copy(buffer, b.Length, currentBuffer, 0, len);

                buffer = b;
            }
            else
            {
                currentBuffer = null;
            }

            this.ReadBuffer(buffer);

            this.readIndex += buffer.Length;

            if (this.totalDataLen == this.readIndex && this.totalDataLen > 0)
            {
                this.ReveiveFinish();
                this.InitData();
                if (this.currentBuffer != null && this.currentBuffer.Length >= T_DataHead.HeadLen)
                {
                    byte[] useBuffer = new byte[currentBuffer.Length];
                    Array.Copy(currentBuffer, useBuffer, currentBuffer.Length);
                    this.currentBuffer = null;
                    this.ReveiveDealFirst(useBuffer);
                }
            }
        }
    }
}
