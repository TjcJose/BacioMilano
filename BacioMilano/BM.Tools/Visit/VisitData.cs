using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
    /// <summary>
    /// 访问数据
    /// </summary>
    /// <typeparam name="K"></typeparam>
    [Serializable]
    public class VisitData<K>
    {
        public VisitData(K key, int depth, K fatherKey)
        {
            this.Key = key;
            this.Depth = depth;
            this.FatherKey = fatherKey;
        }

        /// <summary>
        /// 键
        /// </summary>
        public K Key { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// 父键
        /// </summary>
        public K FatherKey { get; set; }
    }
}
