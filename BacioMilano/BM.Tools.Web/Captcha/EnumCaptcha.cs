using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Tools.Web.Captcha
{
    /// <summary>
    /// 级别
    /// </summary>
    public enum Level
    {
        None = 0,
        Low,
        Medium,
        High,
        Extreme
    }

    /// <summary>
    /// 验证码样式
    /// </summary>
    public enum Style
    {
        Default = 1,
        Small = 2
    }

}
