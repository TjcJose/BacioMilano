using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Visit
{
    [Serializable]
    public class VisitSerializeObj
    {
        public List<string> ls { get; set; }
        public IVisitPool<VisitData<string>> vistPool { get; set; }
    }
}
