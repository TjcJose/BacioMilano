using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BM.DA
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public partial class ModelDAL<T> where T : IModel, new()
    {
        private MappingBase<T> _Mapping;
        public MappingBase<T> Mapping
        {
            get
            {
                return _Mapping;
            }
        }

        public ModelDAL()
        {
            this._Mapping = MappingBase<T>.GetMapping();
        }
    }
}
