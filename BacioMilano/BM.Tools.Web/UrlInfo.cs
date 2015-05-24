using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BM.Tools.Web
{
    public static class UrlInfo
    {
        /// <value>
        /// 站点url路径
        /// </value>
        public static String UrlBase
        {
            get
            {
                var url = ConfigurationManager.AppSettings["UrlBase"];
                if (null != url)
                {
                    return url;
                }
                else
                {
                    string s = HttpContext.Current.Request.Url.AbsoluteUri;
                    int i = s.IndexOf('/');
                    return s.Substring(0, i) + "//" + UrlSuffix;
                }
            }
        }

        public static string FullPath(string host, string absoluteUrl)
        {
            if (absoluteUrl.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                return absoluteUrl;
            }
            return host.TrimEnd('/') + "/" + absoluteUrl.TrimStart('/');
        }

        public static string FullPath(string absoluteUrl)
        {
            return FullPath(UrlBase, absoluteUrl);
        }

        /// <summary>
        /// 除去http://的站点url
        /// </summary>
        public static string UrlSuffix
        {
            get
            {
                return HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
            }
        }
    }
}
