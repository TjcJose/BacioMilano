using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace BM.Net
{
    /// <summary>
    /// tcp client socket 同步操作类
    /// </summary>
    public class TcpClientSocket
    {
        /// <summary>
        /// 连接的socket
        /// </summary>
        protected Socket sockSrv;

        /// <summary>
        /// tcp client socket 同步操作类
        /// </summary>
        public TcpClientSocket()
        {
        }

        ~TcpClientSocket()
        {
            this.Close();
        }

        /// <summary>
        /// 关闭socket
        /// </summary>
        public void Close()
        {
            if (this.sockSrv != null)
            {
                this.sockSrv.Shutdown(SocketShutdown.Both);
                this.sockSrv.Close();
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="port">服务器IP端口</param>
        public void Connect(string serverIp, int port)
        {
            this.sockSrv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.sockSrv.Connect(serverIp, port);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buf">发送的数据</param>
        /// <returns>发送数据的数量</returns>
        public int Send(byte[] buf)
        {
            return this.sockSrv.Send(buf);
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="buf">接受数据的byte[]</param>
        /// <returns>接受的数据数量</returns>
        public int Recv(byte[] buf)
        {
            return this.sockSrv.Receive(buf);
        }

    }
}
