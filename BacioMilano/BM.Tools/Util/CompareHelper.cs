using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 比较代理
    /// </summary>
    /// <typeparam name="T">比较类型</typeparam>
    /// <param name="objA">比较参照物</param>
    /// <param name="objB">要比较的对象</param>
    /// <returns>是否通过</returns>
    public delegate bool CompareGenericMethod<T>(T objA, T objB);

    /// <summary>
    /// 比较判断类
    /// </summary>
    /// <typeparam name="T">比较判断类型</typeparam>
    public sealed class CompareHelper<T>
    {
        /// <summary>
        /// 要比较的对象
        /// </summary>
        private T compare;

        /// <summary>
        /// 比较判断类
        /// </summary>
        /// <param name="compare">比较判断对象</param>
        /// <param name="compareMethod">比较代理方法</param>
        public CompareHelper(T compare, CompareGenericMethod<T> compareMethod)
        {
            this.compare = compare;
            this.CompareMethod = compareMethod;
        }

        /// <summary>
        /// 比较判断类
        /// </summary>
        /// <param name="compareMethod">比较代理方法</param>
        public CompareHelper(CompareGenericMethod<T> compareMethod)
        {
            this.CompareMethod = compareMethod;
        }

        /// <summary>
        /// 比较判断方法
        /// </summary>
        /// <param name="compareY">比较参照物</param>
        /// <returns>是否通过</returns>
        public bool Compare(T compareY)
        {
            if (CompareMethod != null)
            {
                return CompareMethod(compareY, this.compare);
            }
            else
            {
                throw new Exception("比较代理方法为空");
            }
        }

        /// <summary>
        /// 比较代理对象
        /// </summary>
        public CompareGenericMethod<T> CompareMethod = null;

        /// <summary>
        /// 设置要比较的对象
        /// </summary>
        /// <param name="compare">要比较的对象</param>
        public void SetCompareObj(T compare)
        {
            this.compare = compare;
        }
    }
}
