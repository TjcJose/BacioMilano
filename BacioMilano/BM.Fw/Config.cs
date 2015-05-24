using BM.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    [Serializable]
    public class Config:IConfigInfo
    {
        public Config()
        {
            Title = "";
            PageSize = 50;
        }

        public string Title
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// BM.WebPlat
        /// </summary>
        public string UrlWeiXinAuthSite
        {
            get;
            set;
        }


        public string UploadFolder
        {
            get;
            set;
        }

        /// <summary>
        /// 发送邮件服务器地址
        /// </summary>
        public string EmailSmtp { get; set; }

        /// <summary>
        /// 发送邮件服务器端口
        /// </summary>
        public int EmailSmtp_Port { get; set; }

        /// <summary>
        /// 发信人地址 
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// 发送邮件用户名
        /// </summary>
        public string EmailUserName { get; set; }


        /// <summary>
        /// 发送邮件密码
        /// </summary>
        public string EmailUserPassword { get; set; }

    }
}
