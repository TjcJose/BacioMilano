using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    public class CacheServiceProviderHelper<T> where T :  CacheServiceProvider, new()
    {
        public static CacheServiceProvider Instance
        {
            get
            {
                return Util.SingletonHelper<T>.Instance;
            }
        }
    }
}
