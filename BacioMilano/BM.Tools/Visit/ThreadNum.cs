using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BM.Visit
{
    public class ThreadNum<K>
    {
        private object lockObj = new object();

        public ThreadNum(int maxNum)
        {
            this.MaxNum = maxNum;
            this.CurrentNum = 0;
        }

        public Action<ThreadData<K>> ActionIncrease;
        public Action<ThreadData<K>> ActionDecrease;

        /// <summary>
        /// get MaxNum
        /// </summary>
        public int MaxNum { get; protected set; }

        /// <summary>
        /// get CurrentNum
        /// </summary>
        public int CurrentNum { get; protected set; }

        public void Increase(Action<object> action, ThreadData<K> data)
        {
            lock (lockObj)
            {
                if (this.CurrentNum <= this.MaxNum)
                {
                    this.CurrentNum++;
                    WaitCallback async = new WaitCallback(action);
                    ThreadPool.QueueUserWorkItem(async, data);
                    if (this.ActionIncrease != null)
                    {
                        this.ActionIncrease(data);
                    }
                }
                else
                {
                    action(data);
                }
            }
        }

        public void Decrease(ThreadData<K> data)
        {
            lock (lockObj)
            {
                if (this.CurrentNum > 0)
                {
                    this.CurrentNum--;
                    if (this.ActionDecrease != null)
                    {
                        this.ActionDecrease(data);
                    }
                }
            }
        }
    }
}
