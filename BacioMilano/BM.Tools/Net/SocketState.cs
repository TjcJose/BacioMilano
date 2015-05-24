using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Net
{
    /// <summary>
    /// socket 异步读取参数类
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// 读取的数据
        /// </summary>
        public byte[] buffer;

        /// <summary>
        /// tcp socket 客户端异步操作类
        /// </summary>
        public TcpClientSocketAsyn clientSocket;

        /// <summary>
        /// socket 异步读取参数类
        /// </summary>
        /// <param name="clientSocket">tcp socket 客户端异步操作类</param>
        /// <param name="buffer"> 读取的数据</param>
        public SocketState(TcpClientSocketAsyn clientSocket, byte[] buffer)
        {
            this.clientSocket = clientSocket;
            this.buffer = buffer;
        }

        /// <summary>
        /// socket 异步读取参数类
        /// </summary>
        /// <param name="clientSocket">tcp socket 客户端异步操作类</param>
        public SocketState(TcpClientSocketAsyn clientSocket)
        {
            this.clientSocket = clientSocket;
            this.buffer = new byte[1024];
        }
    }
}
