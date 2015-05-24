using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.BModel
{
    [Serializable]
    public class Right
    {
        /// <summary>
        /// OperationId
        /// </summary>
        public Int32 OperationId { get; set; }
        /// <summary>
        /// FunctionId
        /// </summary>
        public Int32 FunctionId { get; set; } 
    }
}
