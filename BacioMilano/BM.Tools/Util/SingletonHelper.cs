using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 单例模式帮助类
    /// </summary>
    /// <typeparam name="T">要得到的类型</typeparam>
    /// <remarks>泛型来完成单例模式的重用</remarks>
    public class SingletonHelper<T> where T : new()
    {
        /// <summary>
        /// 单例模式提供者
        /// </summary>
        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        /// <summary>
        /// 单例模式提供工厂
        /// </summary>
        /// <param name="className">T的类型名称</param>
        /// <returns>T 类型的对象</returns>
        public static T Factory(string className)
        {
            return CreateInstance(className);
        }

        private class SingletonCreator
        {
            internal static readonly T instance = new T();
            internal static Dictionary<string, T> InstanceDictionary = new Dictionary<string, T>();
        }

        /// <summary>
        /// 得到T类的单件实例
        /// </summary>
        /// <param name="className">T类型的className</param>
        /// <returns>T类型的单件实例</returns>
        private static T CreateInstance(string className)
        {
            if (!SingletonCreator.InstanceDictionary.ContainsKey(className))
            {
                T t = (T)Activator.CreateInstance(Type.GetType(className, true));
                SingletonCreator.InstanceDictionary.Add(className, t);
            }
            return SingletonCreator.InstanceDictionary[className];
        }
    }
}
