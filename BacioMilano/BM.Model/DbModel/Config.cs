using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.DbModel
{
    public static class Config
    {
        /// <summary>
        /// FP 数据库连接配置键
        /// </summary>
        public const string ConnectionString = "Ad_ConnectionString";
        private static CultureInfo _Culture = new CultureInfo("zh-CN");
        public static CultureInfo Culture
        {
            get
            {
                return _Culture;
            }
        }

    }
}
