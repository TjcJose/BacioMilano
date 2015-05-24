using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using BM.Util;
using System.IO;
using System.Timers;
using System.Reflection;
using BM.Log;

namespace BM.Tools.Web
{
    /// <summary>
    /// Web上传类
    /// </summary>
    public sealed class WebUpload
    {
        public enum WebUploadState
        {
            Init = 1,
            Run,
            Success,
            ErrorType,
            ErrorSizeOutOfLimit,
            ErrorFileEmpty
        }

        private WebUploadState _State;
        /// <summary>
        /// 获取上传状态
        /// </summary>
        public WebUploadState State
        {
            get
            {
                return this._State;
            }
        }

        /// <summary>
        /// 上传文件大小限制，设为0表式不限制
        /// </summary>
        public int LimitUploadDataSize { get; private set; }

        /// <summary>
        /// 边界
        /// </summary>
        private byte[] _boundaryBytes;
        /// <summary>
        /// 头结束边界
        /// </summary>
        private byte[] _endHeaderBytes;
        /// <summary>
        /// 文件结束边界
        /// </summary>
        private byte[] _endFileBytes;
        /// <summary>
        /// 换行字符
        /// </summary>
        private const string lineBreak = "\r\n";

        /// <summary>
        /// 字节数组长度
        /// </summary>
        private const int byteLen = 2048;

        /// <summary>
        /// 多加的字节数组长度
        /// </summary>
        private const int preByteLen = 1024;

        /// <summary>
        /// 预先加载长度
        /// </summary>
        private const int preLoadByteLen = 10240;

        private static readonly Regex regexFile = new Regex("Content-Disposition:\\s*form-data\\s*;\\s*name\\s*=\\s*\"(?<name>[^\\\"\\\']+)\"\\s*;\\s*filename\\s*=\\s*\"(?<filename>[^\\\"\\\']+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly HttpWorkerRequest _workerRequest;

        public static Dictionary<string, WebUploadData> UploadDataInfos = new Dictionary<string, WebUploadData>();

        public static WebUploadData GetUploadDataInfo(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;
            if (UploadDataInfos.ContainsKey(key))
            {
                return UploadDataInfos[key];
            }
            return null;
        }

        public static void CancelUpload(string key)
        {
            UploadDataInfos[key].Cancel = true;
        }

      

        /// <summary>
        /// 存放文件目录
        /// </summary>
        private string _rootPath;

        /// <summary>
        /// 编码格式
        /// </summary>
        private Encoding _encodingUse { get; set; }

        private byte[] _currentBuffer;

        /// <summary>
        /// get总大小
        /// </summary>
        public int TotalSize
        {
            get
            {
                return UploadDataInfos[this.Id].TotalSize;
            }
            private set
            {
                UploadDataInfos[this.Id].TotalSize = value;
            }
        }

        /// <summary>
        /// get已加载数据
        /// </summary>
        public int LoadedSize
        {
            get
            {
                return UploadDataInfos[this.Id].LoadedSize;
            }
            private set
            {
                UploadDataInfos[this.Id].LoadedSize = value;
            }
        }

        /// <summary>
        /// get剩余数据大小
        /// </summary>
        public int RemainSize { get { return this.TotalSize - this.LoadedSize; } }

        private readonly string _Id;
        /// <summary>
        /// get上传唯一键
        /// </summary>
        public string Id
        {
            get {
                return this._Id;
            }
        }



        /// <summary>
        /// Web上传类
        /// </summary>
        /// <param name="context"></param>
        public WebUpload(HttpContextBase context, string id, int limitUploadDataSize)
        {
            this._State = WebUploadState.Init;
            var provider = (IServiceProvider)context;
            this._workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            this.LimitUploadDataSize = limitUploadDataSize;

            var contentType = _workerRequest.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentType);
            var bufferIndex = contentType.IndexOf("boundary=") + "boundary=".Length;
            var boundary = String.Concat("--", contentType.Substring(bufferIndex));
            this._encodingUse = context.Request.ContentEncoding;
            this._boundaryBytes = this._encodingUse.GetBytes(string.Concat(lineBreak, boundary, lineBreak));
            this._endHeaderBytes = this._encodingUse.GetBytes(string.Concat(lineBreak, lineBreak));
            this._endFileBytes = this._encodingUse.GetBytes(string.Concat(lineBreak, boundary, "--", lineBreak));

            this._Id = id;
            UploadDataInfos.Add(this._Id, new WebUploadData { Cancel = false });
            
            this.TotalSize = this._workerRequest.GetTotalEntityBodyLength();
        }


        /// <summary>
        /// 初始化加载数据（加载上传文件数据流前边的数据）
        /// </summary>
        /// <returns></returns>
        private bool PreLoadData(string rootPath)
        {
            if (!this._workerRequest.HasEntityBody())
            {
                return false;
            }


            if (!_workerRequest.HasEntityBody())
            {
                return false;
            }

            this._rootPath = rootPath;
            this.LoadedSize = this._workerRequest.GetPreloadedEntityBodyLength();

            this._currentBuffer = this._workerRequest.GetPreloadedEntityBody();
            if (this._currentBuffer == null) // IE normally does not preload  
            {
                this._currentBuffer = new byte[preLoadByteLen];
                this.LoadedSize = _workerRequest.ReadEntityBody(this._currentBuffer, this._currentBuffer.Length);
            }

            return true;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceivePartData(ref bool isFirst)
        {
            byte[] newbuffer;
            if (isFirst)
            {//第一次开始，多接受preByteLen个字节，以后查询才可查询分割符，但数据流加载只加载 byteLen 个
                newbuffer = new byte[byteLen + preByteLen];
                isFirst = false;
            }
            else
            {
                if (RemainSize - byteLen < byteLen)
                {//最后一次
                    newbuffer = new byte[RemainSize];
                }
                else
                {//中间
                    newbuffer = new byte[byteLen];
                }
            }
           
            var received = this._workerRequest.ReadEntityBody(newbuffer, newbuffer.Length);
            if (received > 0)
            {
                this.LoadedSize += received;
            }
            else if(this.TotalSize > this.LoadedSize && received == 0)
            {
                throw new HttpRequestValidationException("接收数据失败,可能是客户端已取消请求。");
            }


            this._currentBuffer = this._currentBuffer.AppendBuffer(newbuffer, received);
        }

        /// <summary>
        /// 解析上传数据，获取上传文件名正则匹配信息
        /// 循环匹配，如果没有找到匹配数据，加载没读取数据继续匹配
        /// </summary>
        /// <returns></returns>
        private Match MatchUploadFile()
        {
            bool isFirst = true;
            do
            {
                //循环匹配
                string text = this._encodingUse.GetString(this._currentBuffer);
                var ms = regexFile.Matches(text);
                for (int i = 0; i < ms.Count; i++)
                {
                    if (!String.IsNullOrEmpty(ms[i].Value))
                    {
                        return ms[i];
                    }
                }

                if (this.RemainSize == 0)
                    return null;

                this.ReceivePartData(ref isFirst);

                
                while (_currentBuffer.Length < byteLen)
                {//处理接受数据不足的问题
                    this.ReceivePartData(ref isFirst);
                }

                this._currentBuffer = this._currentBuffer.SubBuffer(byteLen);

            } while (this.TotalSize > LoadedSize);

            return null;
        }

        /// <summary>
        /// 获取上传的服务器文件名全路径
        /// </summary>
        /// <param name="funGetFileName">文件名称处理函数代理
        /// 此代理参数为从流中获取的上传文件名称。此代理作用是给要保存的文件赋个文件名
        /// 如果为空，已从流中获取的上传文件名称给要保存的文件赋名
        /// </param>
        /// <returns>上传文件服务器保存路径，如果返回null,表示找到没有上传文件</returns>
        private string GetUploadFilePath(Func<string, string> funGetFileName)
        {
            Match m = this.MatchUploadFile();

            if (m == null)
            {
                return null;
            }

            var mv = this._encodingUse.GetBytes(m.Value);
            int startindex = this._currentBuffer.IndexOfValues(mv, 0) + mv.Length;
            this._currentBuffer = this._currentBuffer.SubBuffer(startindex);

            var fileName = m.Groups["filename"].Value;
            fileName = Path.GetFileName(fileName); // IE captures full user path; chop it 

            if (funGetFileName != null)
            {
                fileName = funGetFileName(fileName);
            }
            return Path.Combine(this._rootPath, fileName);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveFile(string filePath)
        {
            #region 找到文件数据开始的位置
            int startindex = _currentBuffer.IndexOfValues(this._endHeaderBytes, 0) + this._endHeaderBytes.Length;
            this._currentBuffer = this._currentBuffer.SubBuffer(startindex);
            #endregion
           
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    bool isFirst = true;
                    while (_workerRequest.IsClientConnected() && UploadDataInfos[this.Id].Cancel == false)
                    {
                        #region 接受数据

                        if (this.RemainSize > 0)
                        {
                            //还有数据没接收，接收数据
                            this.ReceivePartData(ref isFirst);
                        }
                        #endregion

                        #region 将数据写入文件流

                        //查询数据分割标记
                        int i = _currentBuffer.IndexOfValues(this._boundaryBytes, 0);
                        if (i == -1)
                        {
                            while (_currentBuffer.Length < byteLen)
                            {//处理接受数据不足的问题
                                this.ReceivePartData(ref isFirst);
                            }

                            var b = _currentBuffer.SubBuffer(0, byteLen);
                            stream.Write(b, 0, b.Length);
                            _currentBuffer = _currentBuffer.SubBuffer(byteLen);
                            if (this.TotalSize == LoadedSize)
                            {
                                // 查到上传数据末尾，加载余下可加载数据到流中
                                startindex = _currentBuffer.IndexOfValues(this._endFileBytes, 0);
                                _currentBuffer = _currentBuffer.SubBuffer(0, startindex);
                                stream.Write(_currentBuffer, 0, _currentBuffer.Length);
                                break;
                            }
                        }
                        else
                        {
                            // 写入部分可写入的数据
                            stream.Write(_currentBuffer, 0, i);
                            _currentBuffer = _currentBuffer.SubBuffer(i);
                            break;
                        }

                        if (_currentBuffer == null)
                            break;

                        #endregion

                        if (this.RemainSize == 0)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper<WebUpload>.GetLogger().Error(ex.Message, ex);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            if (UploadDataInfos[this.Id].Cancel == false)
            {
                UploadDataInfos[this.Id].CanDel = true;
            }
        }

        /// <summary>
        /// 上传文件
        /// 思路：
        /// 1. 先加载部分数据
        /// 2. 循环解析上传文件流
        ///    1）解析上传数据，获取上传文件名信息
        ///      A）循环获取数据流，直到获取了上传文件Match
        ///      B）获取上传文件名信息
        ///    2）找到文件数据开始的位置
        ///    3）创建文件，写入上传数据到文件
        ///      A）接受数据，
        ///         但第一次开始，多接受 preByteLen 字节，以后查询才可查询分割符，但数据流加载只加载 byteLen 个
        ///         最后接收，如果剩余字符小于byteLen，全部接收
        ///      B）将数据写入文件流
        ///         一）查询数据分割标记
        ///         二）没找到分割标记，写入所以刚接受数据，
        ///                如果文件已读完，查询文件数据结束标记，写入结束前的数据。
        ///             找到分割标记，写入分割标记前的数据。
        /// </summary>
        /// <param name="rootPath">上传目录</param>
        /// <param name="funGetFileName">文件名称处理函数代理
        /// 此代理参数为从流中获取的上传文件名称。此代理作用是给要保存的文件赋个文件名
        /// 如果为空，已从流中获取的上传文件名称给要保存的文件赋名
        /// </param>
        /// <param name="funGetFileName">移动文件到新地方或删除临时文件得方法代理
        /// 参数filepath:上传到的位置
        /// 参数WebUploadData:上传当前信息
        /// </param>
        public void Update(string rootPath, Func<string, string> funGetFileName, Action<string, WebUploadData> methodMoveFile, Action<List<string>> methodDealFilePaths, Func<string, bool> checkFileType)
        {
            if (LimitUploadDataSize > 0 && LimitUploadDataSize < this.TotalSize)
            {
                this._State = WebUploadState.ErrorSizeOutOfLimit;
                return;
            }

            this._State = WebUploadState.Run;

            //初始化加载数据（加载上传文件数据流前边的数据）
            if (!this.PreLoadData(rootPath))
            {
                return;
            }

            List<string> ls = new List<string>();
            //循环解析上传文件流
            do
            {
                //解析上传数据，获取上传文件名信息
                var filePath = this.GetUploadFilePath(funGetFileName);

                if (String.IsNullOrEmpty(filePath))
                {
                    this._State = WebUploadState.ErrorFileEmpty;
                    return;
                }

                if (checkFileType != null && checkFileType(filePath) == false)
                {
                    this._State = WebUploadState.ErrorType;
                    return;
                }
               
                this.SaveFile(filePath);
                ls.Add(filePath);

                if (this.TotalSize == this.LoadedSize)
                {
                    break;
                }
            }
            while (this._currentBuffer != null && this._currentBuffer.Length > 0 && UploadDataInfos[this.Id].Cancel == false);

            if (methodMoveFile != null)
            {//移动文件
                foreach (string f in ls)
                {
                    methodMoveFile(f, UploadDataInfos[this.Id]);
                }
            }

            if (methodDealFilePaths != null)
            {
                methodDealFilePaths(ls);
            }

            this._State = WebUploadState.Success;
        }
    }
}
