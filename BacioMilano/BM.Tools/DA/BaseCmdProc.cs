using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.DA
{
    public abstract class BaseCmdProc
    { 
        public abstract string GetProcName();
        public abstract object[] GetObjects();
    }
}
