using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
    [Serializable]
   public class VisitPoolQueue<T>:IVisitPool<T>
    {
        protected Queue<T> pool;

        public  VisitPoolQueue()
        {
            this.pool =new Queue<T>();
        }

        #region IVisitPool<K> Members

        public int Count
        {
            get { return this.pool.Count(); }
        }

        public void Input(T visitData)
        {
            this.pool.Enqueue(visitData);
        }

        public T Remove()
        {
            return this.pool.Dequeue();
        }


        public void Clear()
        {
            this.pool.Clear();
        }

        #endregion
    }
}
