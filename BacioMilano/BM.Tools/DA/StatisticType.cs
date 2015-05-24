using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.DA
{
    /// <summary>
    /// 统计枚举
    /// </summary>
   public enum StatisticType
    {
      /// <summary>
      /// 平均
      /// </summary>
       Avg = 1,
       /// <summary>
       /// 总和
       /// </summary>
       Sum = 2,
       /// <summary>
       /// 最小
       /// </summary>
       Min = 3, 
       /// <summary>
       /// 最大
       /// </summary>
       Max = 4,
       /// <summary>
       /// 总数
       /// </summary>
       Count = 5
    }
}
