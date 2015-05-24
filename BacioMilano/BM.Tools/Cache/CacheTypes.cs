using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BM.Cache
{
   public class CacheTypes<T>
    {
       private readonly PropertyInfo[] _PropertyInfos;

       public CacheTypes()
       {
           this._PropertyInfos = typeof(T).GetProperties();
       }

       public PropertyInfo[] GetProperties()
       {
            return this._PropertyInfos;
       }

       public static CacheTypes<T> Instance
       {
           get
           {
               return Util.SingletonHelper<CacheTypes<T>>.Instance;
           }
       }
    }
}
