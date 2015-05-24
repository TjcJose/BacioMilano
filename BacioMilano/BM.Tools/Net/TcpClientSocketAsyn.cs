using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using BM.Util;
using BM.Log;

namespace BM.Net
{
    /// <summary>
    /// tcp socket 异步对象
    /// </summary>
    public class TcpClientSocketAsyn
    {
        private byte[] headBeginSign;
        /// <summary>
        /// 客户端的socket
        /// </summary>
        protected Socket socketClient;

        /// <summary>
        /// 编码格式
        /// </summary>
        public Encoding netEncoding;

        /// <summary>
        /// 接受数据的回调
        /// </summary>
        public ReceiveCallHandler OnReceiveCallback;

        /// <summary>
        /// 关闭socket的回调
        /// </summary>
        public CloseClientHandler OnCloseClient = null;

        /// <summary>
        /// tcp socket 异步对象
        /// </summary>
        /// <param name="socketClient">客户端的socket</param>
        /// <param name="netEncoding">编码格式</param>
        /// <param name="OnCloseClient">关闭socket的回调</param>
        /// <param name="clientTimeout">连接等待时间</param>
        public TcpClientSocketAsyn(Socket socketClient, Encoding netEncoding, CloseClientHandler OnCloseClient, int clientTimeout)
        {
            this.socketClient = socketClient;
            this.netEncoding = netEncoding;
            this.OnCloseClient = OnCloseClient;
            this.headBeginSign = this.netEncoding.GetBytes(T_DataHead.UseBeginSign);
            socketClient.SendTimeout = clientTimeout;
        }

        /// <summary>
        /// tcp socket 异步对象
        /// </summary>
        /// <param name="socketClient">客户端的socket</param>
        /// <param name="netEncoding">编码格式</param>
        /// <param name="OnCloseClient">关闭socket的回调</param>
        public TcpClientSocketAsyn(Socket socketClient, Encoding netEncoding, CloseClientHandler OnCloseClient)
        {
            this.socketClient = socketClient;
            this.netEncoding = netEncoding;
            this.OnCloseClient = OnCloseClient;
            this.headBeginSign = this.netEncoding.GetBytes(T_DataHead.UseBeginSign);
            socketClient.SendTimeout = 30;
        }

        /// <summary>
        /// tcp socket 异步对象
        /// </summary>
        /// <param name="serverIP">服务器ip</param>
        /// <param name="port">服务器端口</param>
        /// <param name="netEncoding">编码格式</param>
        /// <param name="clientTimeout">连接等待时间</param>
        public TcpClientSocketAsyn(string serverIP, int port, Encoding netEncoding, int clientTimeout)
        {
            try
            {
                this.socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socketClient.SendTimeout = clientTimeout;
                this.socketClient.Connect(serverIP, port);
                this.netEncoding = netEncoding;
                this.headBeginSign = this.netEncoding.GetBytes(T_DataHead.UseBeginSign);
            }
            catch(Exception ex)
            {
                this.socketClient = null;
            }
        }

        /// <summary>
        /// tcp socket 异步对象
        /// </summary>
        /// <param name="serverIP">服务器ip</param>
        /// <param name="port">服务器端口</param>
        /// <param name="netEncoding">编码格式</param>
        public TcpClientSocketAsyn(string serverIP, int port, Encoding netEncoding)
        {
            try
            {
                this.socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socketClient.SendTimeout = 30;
                this.socketClient.Connect(serverIP, port);
                this.netEncoding = netEncoding;
                this.headBeginSign = this.netEncoding.GetBytes(T_DataHead.UseBeginSign);
            }
            catch(Exception ex)
            {
                LogManager.Log<TcpClientSocketAsyn>().Error(ex.Message, ex);
                this.socketClient = null;
            }
        }

        /// <summary>
        /// 关闭socket连接
        /// </summary>
        public void Close()
        {
            if (this.socketClient != null)
            {
                try
                {
                    this.socketClient.Shutdown(SocketShutdown.Both);
                    this.socketClient.Close();
                    this.socketClient = null;
                }
                catch (Exception ex)
                {
                    LogManager.Log<TcpClientSocketAsyn>().Error(ex.Message, ex);
                    this.socketClient = null;
                }
            }
        }

        /// <summary>
        /// 客户端的socket
        /// </summary>
        public Socket SocketClient
        {
            get
            {
                return this.socketClient;
            }
        }

        /// <summary>
        /// tcp socket 异步对象的Id
        /// </summary>
        public int Id
        {
            get
            {
                return this.socketClient.Handle.ToInt32();
            }
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        public void ReceiveAsyn()
        {
            if (OnReceiveCallback != null)
            {
                SocketState state = new SocketState(this);
                this.OnReceiveCallback(state);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        public void Send(int data)
        {
            this.socketClient.Send(System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)));
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        public void Send(long data)
        {
            this.socketClient.Send(System.BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)));
        }


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        public void Send(byte[] data)
        {
            this.socketClient.Send(data);
        }

        public byte[] Receive(int size)
        {
            if (socketClient == null || socketClient.Connected == false)
                return new byte[0];
            byte[] data = new byte[size];
            size = this.socketClient.Receive(data);
            return data.SubBuffer(0, size);
        }

        public byte[] ReceiveData(ref T_DataHead head)
        {
            byte[] data = this.Receive(1024);
            if (data == null || data.Length == 0)
                return null;

            int startIndex = data.IndexOfValues(headBeginSign, 0);
            if (startIndex >= 0)
            {
                byte[] b = new byte[T_DataHead.HeadLen];
                Array.Copy(data, startIndex, b, 0, T_DataHead.HeadLen);
                head = ByteHelper.BytesToStruct<T_DataHead>(b);
                if (head.iBeginSign != T_DataHead.UseBeginSign)
                {
                    return null;
                }

                int len = data.Length - startIndex - T_DataHead.HeadLen;
                if (len < head.iLen)
                {
                    byte[] b2 = this.Receive((int)head.iLen - len);
                    if (b2 != null)
                    {
                        data = data.SubBuffer(startIndex + T_DataHead.HeadLen).AppendBuffer(b2, b2.Length);
                    }
                }
                else
                {
                    return data.SubBuffer(startIndex + T_DataHead.HeadLen);
                }

                if (data.Length == head.iLen)
                {
                    return data;
                }
            }

            return null;
        }

        public void SendStream(Stream stream)
        {
            SendStream(stream, 0);
        }

        public void SendStream(Stream stream, long index)
        {
            int size = 1024;
            byte[] data = new byte[size];


            using (NetworkStream ns = new NetworkStream(this.socketClient))
            {
                stream.Position = index;
                int count = stream.Read(data, 0, size);
                while (count > 0)
                {
                    try
                    {
                        ns.Write(data, 0, count);
                        count = stream.Read(data, 0, size);
                    }
                    catch
                    {
                        return;
                    }
                }
            }

        }
    }
}
