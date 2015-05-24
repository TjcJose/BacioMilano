using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Util
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 添加星期
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="value">添加的星期数</param>
        /// <returns></returns>
        public static DateTime AddWeek(this DateTime dateTime, double value)
        {
            return dateTime.AddDays(value * 7);
        }
    }
}
