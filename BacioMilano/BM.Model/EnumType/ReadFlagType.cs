using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    public enum ReadFlagType
    {
        [Description("未读")]
        UnRead = 0,

        [Description("已读")]
        Read = 1
    }
}
