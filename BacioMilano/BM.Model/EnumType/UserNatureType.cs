using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    /// <summary>
    /// 性质
    /// </summary>
    public enum UserNatureType
    {
        /// <summary>
        /// 个人
        /// </summary>
        [Description("个人")]
        Personal = 1,
        /// <summary>
        /// 公司
        /// </summary>
        [Description("公司")]
        Company = 2
    }
}
