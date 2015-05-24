using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace BM.Cache
{
    public class EnterpriseLibraryCacheServiceProvider : CacheServiceProvider
    {
        // Fields
        private ICacheService defaultCacheService;
        private static object lockObject;

        // Methods
        static EnterpriseLibraryCacheServiceProvider()
        {
            lockObject = new object();
        }

        public EnterpriseLibraryCacheServiceProvider()
        {
        }

        public override ICacheService GetCacheService()
        {
            if (this.defaultCacheService == null)
            {
                lock (lockObject)
                {
                    if (this.defaultCacheService == null)
                    {
                        var cacheManager = CacheFactory.GetCacheManager();
                        this.defaultCacheService = new EnterpriseLibraryCacheService(cacheManager);
                    }
                }
            }
            return this.defaultCacheService;
        }
    }
}
