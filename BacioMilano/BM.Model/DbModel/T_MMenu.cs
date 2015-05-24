using BM.Model.BModel;
using BM.Model.EnumType;
using BM.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.DbModel
{
    public partial class T_MMenu
    {
        public IList<T_MMenu> Sons { get; set; }

        public string MenuTypeName
        {
            get
            {
                return EnumHelper.GetDescription<MenuType>(this.MenuType.Value);
            }
        }

        public bool HasSon
        {
            get
            {
                return (Sons == null || Sons.Count == 0) ? false : true;
            }
        }
    }
}
