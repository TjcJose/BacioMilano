using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Net
{
   /// <summary>
   /// 读取回调
   /// </summary>
   /// <param name="state">回调参数</param>
   public delegate void ReceiveCallHandler(SocketState state);

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <param name="client">tcp socket 异步客户端类</param>
   public delegate void CloseClientHandler(TcpClientSocketAsyn client);
}
