using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BM.DA
{
    public class OperationSqlServer : OperationBase
    {
        public override IDbDataAdapter GetDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
