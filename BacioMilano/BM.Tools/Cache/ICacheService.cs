using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public interface ICacheService
    {
        // Methods
        void Add(string key, object value, CachingExpirationTypes cachingExpirationType);
        void Add(string key, object value, TimeSpan timeSpan);
        void AddWithFileDependency(string key, object value, string fullFileNameOfFileDependency);
        void Clear();
        bool Contains(string cacheKey);
        object Get(string cacheKey);
        void Remove(string cacheKey);
        object this[string key] {get;set;}
        // Properties
        int Count { get; }
    }
}
