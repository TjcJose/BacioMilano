using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
     [Serializable]
    public class VisitPoolStack<T> : IVisitPool<T>
    {
        protected Stack<T> pool;
        public VisitPoolStack()
        {
            this.pool = new Stack<T>();
        }

        #region IVisitPool<K> Members

        public int Count
        {
            get { return this.pool.Count(); }
        }

        public void Input(T visitData)
        {
            this.pool.Push(visitData);
        }

        public void Clear()
        {
            this.pool.Clear();
        }



        public T Remove()
        {
            return this.pool.Pop();
        }

        #endregion
    }
}
