using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BM.Visit
{
    /// <summary>
    /// 数据访问器基类
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public abstract class VisitBase<K, T>
    {
        /// <summary>
        /// 数据访问器基类构造函数
        /// </summary>
        /// <param name="startAddress">开始地址值</param>
        /// <param name="name">访问器名称</param>
        /// <param name="depth">访问深度,0为不限深度</param>
        /// <param name="visitArithmetic">访问算法</param>
        /// <param name="funGetDataChildren">得到子数据的代理</param>
        /// <param name="funGetData">取得相关数据</param>
        public VisitBase(K startAddress, string name, int depth, VisitArithmetic visitArithmetic, Func<VisitData<K>, T, IEnumerable<VisitData<K>>> funGetDataChildren, Func<VisitData<K>, T> funGetData)
        {
            this.Name = name;
            this.Depth = depth;
            if (visitArithmetic == VisitArithmetic.Depth)
            {
                this.visitPool = new VisitPoolStack<VisitData<K>>();
            }
            else
            {
                this.visitPool = new VisitPoolQueue<VisitData<K>>();
            }
            this.FunGetDataChildren = funGetDataChildren;
            this.FunGetData = funGetData;
            this.State = VisitState.Stop;
            this.StartAddress = startAddress;
        }

        protected IVisitPool<VisitData<K>> visitPool;
        protected List<K> listUse;

      

        public Action PauseCallBefore;
        public Action PauseCallAfter;
        public virtual void Pause()
        {
            if (this.PauseCallBefore != null) this.PauseCallBefore();
            this.State = VisitState.Pause;
        }

        public Action ResumeCallBefore;
        public Action ResumeCallAfter;
        protected virtual void Resume()
        {
            if (this.ResumeCallBefore != null) this.ResumeCallBefore();
            if (!this.LoadData())
            {
                var root = new VisitData<K>(this.StartAddress, 0, this.StartAddress);
                this.listUse = new List<K>();
                this.visitPool.Clear();
                this.listUse.Add(root.Key);
                this.visitPool.Input(root);

            }
            this.State = VisitState.Run;
            if (this.ResumeCallAfter != null) this.ResumeCallAfter();

        }

        public Action FinishCallBefore;
        public Action FinishCallAfter;
        protected virtual void Finish()
        {
            if (this.FinishCallBefore != null) this.FinishCallBefore();           
            this.ClearData();
            this.State = VisitState.Finish;
            if (this.FinishCallAfter != null) this.FinishCallAfter();
        }

        public Action StopCallBefore;
        public Action StopCallAfter;
        public virtual void Stop()
        {
            if (this.StopCallBefore != null) this.StopCallBefore();
            this.ClearData();
            this.State = VisitState.Stop;
            if (this.StopCallAfter != null) this.StopCallAfter();
        }

        public Action RunCallBefore;
        public Action RunCallAfter;

        /// <summary>
        /// get 数据访问活动，返回数据解析成功或失败
        /// </summary>
        public Action<VisitData<K>, T> ActionDataVisiting;
        /// <summary>
        /// get 数据访问活动结束后的处理
        /// </summary>
        public Action<VisitData<K>, T> ActionDataVisited;

        protected ThreadNum<K> threadNumObj;

        /// <summary>
        ///  多线程从给定节点访问所有数据方法代理
        /// </summary>
        /// <param name="threadNumObj"></param>
        public virtual void Run(ThreadNum<K> threadNumObj)
        {
            this.threadNumObj = threadNumObj;
            if (this.RunCallBefore != null) this.RunCallBefore();
            this.Resume();


            if (this.visitPool.Count > 0)
            {
                do
                {
                    if (this.visitPool.Count > 0)
                    {
                        var visitData = this.visitPool.Remove();
                        var data = new ThreadData<K>(visitData, threadNumObj, this.visitPool);
                        threadNumObj.Increase(this._VisitThread, data);
                    }

                    while (this.visitPool.Count == 0 && this.listUse.Count > 0 && this.threadNumObj.CurrentNum > 0)
                    {
                        Thread.Sleep(100);
                    }
                }
                while (this.State == VisitState.Run && this.listUse.Count > 0);
            }

            while (this.State == VisitState.Pause)
            {
                if (threadNumObj.CurrentNum == 0)
                {
                    this.SaveData();
                    if (this.PauseCallAfter != null) this.PauseCallAfter();
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            this.State = VisitState.Stop;

            if (this.RunCallAfter != null) this.RunCallAfter();

            if (this.visitPool.Count == 0 && this.listUse.Count == 0)
            {
                this.Finish();
            }
        }

        protected virtual void _VisitThread(object threadDataObject)
        {
            var threadData = (ThreadData<K>)threadDataObject;
            T data = this.FunGetData(threadData.VisitDataObj);
            if (data != null)
            {
                if (this.ActionDataVisiting != null)
                {
                    this.ActionDataVisiting(threadData.VisitDataObj, data);
                }

                if (threadData.VisitDataObj.Depth < this.Depth - 1)
                {
                    var children = FunGetDataChildren(threadData.VisitDataObj, data);
                    if (children != null && children.Count() > 0)
                    {
                        foreach (VisitData<K> child in children)
                        {
                            threadData.VisitPool.Input(child);
                            this.listUse.Add(child.Key);
                        }
                    }
                }

                if (this.ActionDataVisited != null)
                {
                    this.ActionDataVisited(threadData.VisitDataObj, data);
                }
            }
            threadData.ThreadNumObj.Decrease(threadData);
            this.listUse.Remove(threadData.VisitDataObj.Key);
        }


        /// <summary>
        /// get 取得数据
        /// </summary>
        public Func<VisitData<K>, T> FunGetData { get; protected set; }

        /// <summary>
        /// get 得到子数据的代理
        /// </summary>
        public Func<VisitData<K>, T, IEnumerable<VisitData<K>>> FunGetDataChildren { get; protected set; }
        /// <summary>
        /// get 访问深度,0为不限深度
        /// </summary>
        public int Depth { get; protected set; }

        /// <summary>
        /// get 访问器名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// get 访问器 开始K
        /// </summary>
        public K StartAddress { get; protected set; }

        /// <summary>
        /// get set 状态
        /// </summary>
        public VisitState State{get;protected set;}

        protected abstract bool LoadData();

        protected abstract void SaveData();

        protected abstract void ClearData();
       
    }
}
