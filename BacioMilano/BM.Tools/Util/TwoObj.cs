using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 两个对象的泛型
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public  class TwoObj<K,V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public TwoObj(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public TwoObj()
        {
        }

        public  const string Field_Key = "Key";
        public const string Field_Value = "Value";
    }

    public static class TwoObj
    {
        public  const string Field_Key = "Key";
        public const string Field_Value = "Value";
    }
}
