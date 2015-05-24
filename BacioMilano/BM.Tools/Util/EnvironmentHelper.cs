using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    public static class EnvironmentHelper
    {
        /// <summary>
        /// 是否为64位
        /// </summary>
        /// <returns></returns>
        public static bool Is64()
        {
            return IntPtr.Size == 8;
        }

        /// <summary>
        /// 是否为32位
        /// </summary>
        /// <returns></returns>
        public static bool Is32()
        {
            return IntPtr.Size == 4;
        }
    }
}
