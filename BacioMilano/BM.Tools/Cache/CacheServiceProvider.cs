using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    /// <summary>
    /// 服务缓存提供器接口
    /// </summary>
    public abstract class CacheServiceProvider
    {
       public abstract ICacheService GetCacheService();
       
       protected CacheServiceProvider()
       {
       }
    }
}
