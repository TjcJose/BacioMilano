using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
    public class ThreadData<K>
    {
        public ThreadData(VisitData<K> visitDataObj, ThreadNum<K> threadNumObj)
        {
            this.VisitDataObj = visitDataObj;
            this.ThreadNumObj = threadNumObj;
        }

        public ThreadData(VisitData<K> visitDataObj, ThreadNum<K> threadNumObj,  IVisitPool<VisitData<K>> visitPool)
        {
            this.VisitDataObj = visitDataObj;
            this.ThreadNumObj = threadNumObj;
            this.VisitPool = visitPool;
        }

        public IVisitPool<VisitData<K>> VisitPool { get; set; }
        public VisitData<K> VisitDataObj { get; set; }
        public ThreadNum<K> ThreadNumObj { get; set; }

    }
}
