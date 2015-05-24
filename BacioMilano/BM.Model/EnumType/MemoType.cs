using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        [Description("组")]
        Group = 1,
        [Description("页")]
        Page = 2,
        [Description("对话框")]
        Dialog = 3,
        [Description("链接")]
        Link = 4
    }
}
