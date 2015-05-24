using BM.DA;
using BM.Model.DbModel;
using BM.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    public static class DbHelper
    {
        public static IDbConnection GetConnection()
        {
            return SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping.CreateConnection();
        }
    }
}
