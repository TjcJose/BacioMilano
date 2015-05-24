using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using BM.Log;

namespace BM.Net
{
    /// <summary>
    /// 异步Tcp Server Socket
    /// </summary>
    public class TcpServerSocketAsyn
    {
        private bool isRun;
        private string localIP;
        private int port;
        private Encoding netEncoding;

        private ManualResetEvent acceptDone = new ManualResetEvent(false);
        private Dictionary<int, TcpClientSocketAsyn> clientSockets;

        protected Socket sockSrv;

        /// <summary>
        /// 接受回调
        /// </summary>
        public ReceiveCallHandler OnReceiveCallback;
        
      
        /// <summary>
        /// 异步TcpServerSocketAsyn
        /// </summary>
        /// <param name="localIP">服务器IP</param>
        /// <param name="port">服务器端口</param>
        /// <param name="netEncoding">编码格式</param>
        public TcpServerSocketAsyn(string localIP, int port, Encoding netEncoding)
        {
            this.isRun = false;
            this.localIP = localIP;
            this.port = port;
            this.netEncoding = netEncoding;
            this.sockSrv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.clientSockets = new Dictionary<int, TcpClientSocketAsyn>();
        }

        ~TcpServerSocketAsyn()
        {
            this.Close();
        }

        public void Start()
        {
            this.StartY();
        }

        /// <summary>
        /// 开始服务器段监听
        /// </summary>
        /// <returns></returns>
        public bool StartY()
        {
            if (this.isRun == true)
                return true;

            this.isRun = true;

            if (this.bindAndListen() == false)
                return false;

            while (this.isRun)
            {
                this.acceptDone.Reset();
                this.sockSrv.BeginAccept(new AsyncCallback(acceptCallback), this.sockSrv);
                this.acceptDone.WaitOne();
            }

            return true;
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        public void Pause()
        {
            this.isRun = false;
        }

        /// <summary>
        /// 关闭服务器socket
        /// </summary>
        public void Close()
        {
            if (this.sockSrv != null)
            {
                this.closeAllClient();
                try
                {
                    this.sockSrv.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    LogManager.Log<TcpServerSocketAsyn>().Info(ex.Message, ex);
                }
                try
                {
                    this.sockSrv.Close();
                }
                catch (Exception ex)
                {
                    LogManager.Log<TcpServerSocketAsyn>().Info(ex.Message, ex);
                }
                this.sockSrv = null;
            }
        }

        /// <summary>
        /// 关闭客户端连接的socket请求
        /// </summary>
        /// <param name="client">异步客户端socket的对象</param>
        public void CloseClient(TcpClientSocketAsyn client)
        {
            lock (this.clientSockets)
            {
                this.clientSockets.Remove(client.Id);
                client.Close();
            }
        }

       

        private bool bindAndListen()
        {
            try
            {
                this.sockSrv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(this.localIP);
                IPEndPoint ipEnd = new IPEndPoint(ip, this.port);
                this.sockSrv.Bind(ipEnd);
                this.sockSrv.Listen(10000);
                return true;
            }
            catch(Exception ex)
            {
                LogManager.Log<TcpServerSocketAsyn>().Error(ex.Message, ex);
                return false;
            }
        }

        private void acceptCallback(IAsyncResult ar)
        {
            this.acceptDone.Set();

            try
            {
                TcpClientSocketAsyn client = null;
                Socket socketClient = ((Socket)ar.AsyncState).EndAccept(ar);
                client = new TcpClientSocketAsyn(socketClient, this.netEncoding, this.CloseClient);
                this.clientSockets.Add(client.Id, client);

                client.OnReceiveCallback = this.OnReceiveCallback;
                if (client.SocketClient.Connected)
                {
                    client.ReceiveAsyn();
                }
            }
            catch(Exception ex)
            {

                LogManager.Log<TcpServerSocketAsyn>().Info(ex.Message, ex);
            }
        }

        private void closeAllClient()
        {
            lock (this.clientSockets)
            {
                foreach (TcpClientSocketAsyn client in this.clientSockets.Values)
                {
                    client.Close();
                }
                this.clientSockets.Clear();
            }
        }

    }
}
