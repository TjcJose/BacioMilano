﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 结果
    /// </summary>
    [Serializable]
    public class ResultU
    {
        /// <summary>
        /// 消息提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 信息代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息结果
        /// </summary>
        public bool IsOK{get;set;}


        /// <summary>
        /// 扩展消息
        /// </summary>
        public string Ext { get; set; }


        public ResultU()
        {
            
        }

        public ResultU(bool isOK, int code = 0, string msg = "")
        {
            this.IsOK = isOK;
            this.Code = code;
            this.Msg = msg;
        }
    }

    /// <summary>
    /// 结果
    /// </summary>
    [Serializable]
    public class ResultU<T>:ResultU
    {
        public ResultU()
        {

        }

        public ResultU(bool isOK = true, int code = 0, string msg = "")
        {
            this.IsOK = isOK;
            this.Code = code;
            this.Msg = msg;
        }

        public ResultU(T data, bool isOK = true, int code = 0, string msg = "")
        {
            this.IsOK = isOK;
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        public ResultU(bool isOK, T data, string msg = "", int code = 0)
        {
            this.IsOK = isOK;
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }


      

        /// <summary>
        /// 扩展消息
        /// </summary>
        public string Ext { get; set; }

       
        /// <summary>
        /// 结果对象
        /// </summary>
        public T Data { get; set; }
    }
}
