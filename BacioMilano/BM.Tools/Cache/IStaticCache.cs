using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Cache
{
    public interface IStaticCache
    {
        void Set(string key, object value, DateTime absoluteExpiration);
        void Set(string key, object value, TimeSpan slidingExpiration);
        T Get<T>(string key);
        void Delete(string key);
    }
}
