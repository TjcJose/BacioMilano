using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 过滤匹配
    /// </summary>
    public abstract class Filter
    {
        public Filter filter { get; set; }


        public bool Check(string str)
        {
            if (this.IsMatch(str))
            {
                if (this.filter != null)
                {
                    return this.filter.Check(str);
                }
                return true;
            }
            return false;
        }

        protected abstract bool IsMatch(string str);
    }
}
