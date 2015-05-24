using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    /// <summary>
    /// 缓存辅助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CacheCallHelper<T, W> where W : CacheServiceProvider, new()
    {
        private static object lockObj = new object();
        /// <summary>
        /// 运行方法，加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="f"></param>
        /// <param name="cachingExpirationType"></param>
        /// <param name="lockObj">锁对象，为null时，没有锁</param>
        /// <returns></returns>
        public static T CacheFunRun(string key, Func<T> f, CachingExpirationTypes cachingExpirationType, object lockObj)
        {
            var ser = CacheServiceProviderHelper<W>.Instance.GetCacheService();
            Func<T> tempF = delegate
            {
                if (ser.Contains(key))
                {
                    return (T)ser.Get(key);
                }
                else
                {
                    var val = f();
                    ser.Add(key, val, cachingExpirationType);
                    return (T)val;
                }
            };
            if (lockObj != null)
            {
                lock (lockObj)
                {
                    return tempF();
                }

            }
            else
            {
                return tempF();
            }

        }

    }
}
