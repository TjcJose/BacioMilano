using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    /// <summary>
    /// 缓存过期类型
    /// </summary>
    public enum CachingExpirationTypes
    {
        /// <summary>
        /// 恒定的 8760 hour
        /// </summary>
        Invariable,
        /// <summary>
        /// 稳定的 8 hour
        /// </summary>
        Stable,
        /// <summary>
        /// 相对地稳定的 2 hour
        /// </summary>
        RelativelyStable,
        /// <summary>
        /// 一般简单对象 10 minute
        /// </summary>
        UsualSingleObject,
        /// <summary>
        /// 一般集合对象 5 minute
        /// </summary>
        UsualObjectCollection,
        /// <summary>
        /// 简单对象 3 minute
        /// </summary>
        SingleObject,
        /// <summary>
        /// 集合对象
        /// </summary>
        ObjectCollection
    }
}

 