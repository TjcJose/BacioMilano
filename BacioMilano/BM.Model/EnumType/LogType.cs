using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    public enum LogFun
    {
        None = 0,
        Auth = 1,
        AccessToken = 2,
        Menu_Get = 3,
        Menu_Create = 4,
        Menu_Delete = 5
    }

    public enum LogResult
    {
        Fail = 0,
        Info = 2,
        Success = 1,
       
    }

    public enum LogSys
    {
        Manager = 1,
        Site = 2,
        WebPlat = 3
    }
}
