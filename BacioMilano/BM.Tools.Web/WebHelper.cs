using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BM.Tools.Web
{
    public static class WebHelper
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {

            return (file != null && file.ContentLength > 0) ? true : false;

        }

        /// <summary>
        /// 读取指定URL地址的HTML，用来以后发送网页用
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebRequest(string url)
        {
            //读取stream并且对于中文页面防止乱码
            StreamReader reader = new StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }
    }
}
