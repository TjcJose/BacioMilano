using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
    /// <summary>
    /// 访问状态
    /// </summary>
    public enum VisitState
    {
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 1,
        /// <summary>
        /// 运行
        /// </summary>
        Run = 2,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause = 3,
        /// <summary>
        /// 完成
        /// </summary>
        Finish = 4
    }
}
