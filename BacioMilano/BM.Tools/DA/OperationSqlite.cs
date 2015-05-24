using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.SQLite;
using System.Data;
using System.Reflection;

namespace BM.DA
{
    public class OperationSqlite : OperationBase
    {
        public override IDbDataAdapter GetDataAdapter()
        {
            IDbDataAdapter obj = (IDbDataAdapter)Activator.CreateInstance(type_SQLiteDataAdapter);
            return obj;
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection obj = (IDbConnection)Activator.CreateInstance(type_SQLiteConnection, new object[] { connectionString });
            return obj;
        }

        public IDbDataParameter CreateDataParameter(string paramName, DbType dbType)
        {
            IDbDataParameter obj = (IDbDataParameter)Activator.CreateInstance(type_SQLiteParameter, new object[] { paramName, dbType });
            return obj;
        }

        public static OperationSqlite Instance
        {
            get
            {
                return BM.Util.SingletonHelper<OperationSqlite>.Instance;
            }
        }


        static OperationSqlite()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "System.Data.SQLite.dll");
            if (System.Environment.Is64BitOperatingSystem)
            {
                //BM.Log.LogManager.Log("OperationSqlite").Info("x64:" + path);
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x64");
                path = System.IO.Path.Combine(path, "System.Data.SQLite.dll");
            }
            //else
            //{
            //    BM.Log.LogManager.Log("OperationSqlite").Info("x32:" + path);
            //}
            sqliteAssembly = Assembly.LoadFile(path);
            type_SQLiteDataAdapter = sqliteAssembly.GetType("System.Data.SQLite.SQLiteDataAdapter");
            type_SQLiteConnection = sqliteAssembly.GetType("System.Data.SQLite.SQLiteConnection");
            type_SQLiteParameter = sqliteAssembly.GetType("System.Data.SQLite.SQLiteParameter");
        }

        private static Type type_SQLiteParameter;
        private static Type type_SQLiteDataAdapter;
        private static Type type_SQLiteConnection;
        private static Assembly sqliteAssembly;
      
    }
}
