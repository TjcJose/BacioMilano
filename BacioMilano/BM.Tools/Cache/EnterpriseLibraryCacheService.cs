using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace BM.Cache
{
    public class EnterpriseLibraryCacheService : ICacheService
    {
        // Fields
        private ICacheManager entCacheManager;

        internal EnterpriseLibraryCacheService(ICacheManager cacheManager)
        {
            this.entCacheManager = cacheManager;
        }

        public void Add(string key, object value, CachingExpirationTypes cachingExpirationType)
        {
           
            if ((key != null) && (value != null))
            {
                TimeSpan span;
                switch (cachingExpirationType)
                {
                    case CachingExpirationTypes.Invariable:
                        span = new TimeSpan(8760, 0, 0);
                        break;

                    case CachingExpirationTypes.Stable:
                        span = new TimeSpan(8, 0, 0);
                        break;

                    case CachingExpirationTypes.RelativelyStable:
                        span = new TimeSpan(2, 0, 0);
                        break;

                    case CachingExpirationTypes.UsualSingleObject:
                        span = new TimeSpan(0, 10, 0);
                        break;

                    case CachingExpirationTypes.UsualObjectCollection:
                        span = new TimeSpan(0, 5, 0);
                        break;

                    case CachingExpirationTypes.SingleObject:
                        span = new TimeSpan(0, 3, 0);
                        break;

                    default:
                        span = new TimeSpan(0, 3, 0);
                        break;
                }
                this.Add(key, value, span);
            }
        }

        public void Add(string key, object value, TimeSpan timeSpan)
        {
            AbsoluteTime time = new AbsoluteTime(timeSpan);
            this.entCacheManager.Add(key, value, CacheItemPriority.Normal, null, new ICacheItemExpiration[] { time });
        }


        public void AddWithFileDependency(string key, object value, string fullFileNameOfFileDependency)
        {
            if ((key != null) && (value != null))
            {
                FileDependency dependency = new FileDependency(fullFileNameOfFileDependency);
                this.entCacheManager.Add(key, value, CacheItemPriority.High, null, new ICacheItemExpiration[] { dependency });
            }
        }


        public void Clear()
        {
            this.entCacheManager.Flush();
        }


        public bool Contains(string key)
        {
            return this.entCacheManager.Contains(key);
        }


        public object Get(string key)
        {
            return this.entCacheManager.GetData(key);
        }


        public void Remove(string key)
        {
            if (key != null)
            {
                this.entCacheManager.Remove(key);
            }
        }


        // Properties
        public int Count
        {
            get
            {
                return this.entCacheManager.Count;
            }
        }


        public object this[string key]
        {
            get
            {
                return this.entCacheManager[key];
            }
            set
            {
                if (this.Contains(key))
                {
                    this.entCacheManager.Remove(key);
                }
                this.entCacheManager.Add(key, value);
            }
        }

     
    }
}
