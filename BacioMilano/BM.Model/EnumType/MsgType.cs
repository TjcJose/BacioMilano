using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    public enum MsgType
    {
        /// <summary>
        /// 公告
        /// </summary>
        [Description("公告")]
        Notice = 1,
        /// <summary>
        /// 消息
        /// </summary>
        [Description("消息")]
        Message = 2
    }
}
