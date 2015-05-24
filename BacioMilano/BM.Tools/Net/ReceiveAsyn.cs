using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using BM.Log;

namespace BM.Net
{
    /// <summary>
    /// 异步接受基类
    /// </summary>
    public abstract class ReceiveAsyn
    {
        /// <summary>
        /// 客户端socket
        /// </summary>
        protected Socket clientSocket;



        /// <summary>
        /// 异步接受基类
        /// </summary>
        public ReceiveAsyn()
        {
            this.InitData();
        }

        /// <summary>
        /// 以读取到的位置,(不包括头数据的长度)
        /// </summary>
        protected long readIndex;

        /// <summary>
        /// 要读取的数据长度,(不包括头数据的长度)
        /// </summary>
        protected long totalDataLen;

        /// <summary>
        /// 接受处理
        /// </summary>
        /// <param name="buffer"></param>
        protected abstract void ReveiveDeal(byte[] buffer);

        /// <summary>
        /// 第一次接受处理
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>要读取的数据长度</returns>
        protected abstract void ReveiveDealFirst(byte[] buffer);



        protected virtual void InitData()
        {
            this.readIndex = 0;
            this.totalDataLen = 0;
        }

        /// <summary>
        /// 接受回调
        /// </summary>
        /// <param name="iar"></param>
        public void EndAccept(IAsyncResult iar)
        {
            if (this.readIndex >= this.totalDataLen - 1 && this.readIndex > 0)
            {
                return;
            }

            SocketState state = (SocketState)iar.AsyncState;
            this.clientSocket = state.clientSocket.SocketClient;
            SocketError errorCode;

            int bytesRead = 0;
            try
            {
                bytesRead = this.clientSocket.EndReceive(iar);

                if (bytesRead > 0)
                {
                    if (this.clientSocket.Available > 0 || this.clientSocket.Connected)
                    {
                        if (this.readIndex == 0)
                        {
                            if (bytesRead == state.buffer.Length)
                            {
                                this.ReveiveDealFirst(state.buffer);
                            }
                            else
                            {
                                byte[] bytes = new byte[bytesRead];
                                Array.Copy(state.buffer, bytes, bytesRead);
                                this.ReveiveDealFirst(bytes);
                            }
                        }
                        else
                        {
                            if (bytesRead == state.buffer.Length)
                            {
                                this.ReveiveDeal(state.buffer);
                            }
                            else
                            {
                                byte[] bytes = new byte[bytesRead];
                                Array.Copy(state.buffer, bytes, bytesRead);
                                this.ReveiveDeal(bytes);
                            }
                        }

                        this.clientSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, out errorCode, EndAccept, state);

                    }
                }
                else
                {
                    this.InitData();

                    if (this.clientSocket == null) return;
                    if (state.clientSocket.OnCloseClient == null)
                    {
                        this.clientSocket.Close();
                    }
                    else
                    {
                        state.clientSocket.OnCloseClient(state.clientSocket);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.Log<ReceiveAsyn>().Error(ex.Message, ex);
                if (this.clientSocket == null) return;
                if (state.clientSocket.OnCloseClient != null)
                {
                    state.clientSocket.OnCloseClient(state.clientSocket);
                }
                else
                {
                    this.clientSocket.Close();
                }
            }
        }
    }
}
