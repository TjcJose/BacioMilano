using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace BM.Net
{
    /// <summary>
    /// 同步服务器tcp socket对象
    /// </summary>
    public class TcpServerSocket
    {
        /// <summary>
        /// 服务器tcp socket对象
        /// </summary>
        protected Socket sockSrv;

        /// <summary>
        /// 同步服务器tcp socket对象
        /// </summary>
        public TcpServerSocket()
        {

        }

        ~TcpServerSocket()
        {
            this.Close();
        }

        /// <summary>
        /// 关闭服务器tcp socket对象
        /// </summary>
        public void Close()
        {
            if (this.sockSrv != null && this.sockSrv.Connected == true)
            {
                this.sockSrv.Shutdown(SocketShutdown.Both);
                this.sockSrv.Close();
            }
        }

        /// <summary>
        /// socket 绑定
        /// </summary>
        /// <param name="localIP">服务器IP</param>
        /// <param name="port">服务器端口</param>
        public void Bind(string localIP, int port)
        {
            this.sockSrv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(localIP);
            IPEndPoint ipEnd = new IPEndPoint(ip, port);
            this.sockSrv.Bind(ipEnd);
        }

        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="queueNum">最大可监听的数量</param>
        public void Listen(int queueNum)
        {
            this.sockSrv.Listen(queueNum);
        }

        /// <summary>
        /// 监听
        /// </summary>
        public void Listen()
        {
            this.sockSrv.Listen(0);
        }

        /// <summary>
        /// 接受获取客户端连接的socket
        /// </summary>
        /// <returns></returns>
        public Socket Accept()
        {
            return this.sockSrv.Accept();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socketClient">客户端socket</param>
        /// <param name="buf">发送的书已经</param>
        /// <returns></returns>
        public int Send(Socket socketClient, byte[] buf)
        {
            return socketClient.Send(buf);
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public int Recv(Socket socketClient, byte[] buf)
        {
            return socketClient.Receive(buf);
        }


    }
}
