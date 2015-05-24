using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Xml;
using System.Data.OracleClient;
using BM.Log;

namespace BM.DA
{
    public  class DbConnectionHelper
    {
        public const string ConfigSection_ConnectionStrings = "connectionStrings";
        public static void Refresh_ConfigSection_ConnectionStrings()
        {
            ConfigurationManager.RefreshSection(ConfigSection_ConnectionStrings);
        }

       public String DbAddress{get;set;}
       public String Database{get;set;}
       public String UserName{get;set;}
       public String Password{get;set;}
       public DatabaseType DbType {get;set;}

       

       public DbConnectionHelper(DatabaseType dbType, string dbAddress, string db, string user, string pwd)
       {
           this.DbType = dbType;
           this.DbAddress = dbAddress;
           this.Database = db;
           this.UserName = user;
           this.Password = pwd;
       }

       public static string GetDatabase_Bak(string dbCreateName)
       {
           return dbCreateName + "_bak";
       }

       public static string GetMasterDb(DatabaseType dbType)
       {
            if (dbType == DatabaseType.SqlServer)
           {
               return "master";
           }
           else if (dbType == DatabaseType.MySql)
           {
               return "mysql";
           }

           return null;
       }

       public enum CreateDbResultEnum
       {
           ConnDisable = 1,
           DbExist = 2,
           Success = 9
       }

       public static CreateDbResultEnum CreateDb_bak(DatabaseType dbType, string dbAddress, string dbCreateName, string user, string pwd)
       {
           if (dbType != DatabaseType.SqlServer && dbType != DatabaseType.MySql)
           {
               throw new ArgumentException("只支持mysql,sqlserver数据库的创建");
           }
           else
           {
               var connHelper = new DbConnectionHelper(dbType, dbAddress, GetMasterDb(dbType), user, pwd);
               if (connHelper.TestConn())
               {
                   var conn = connHelper.GetConnection();

                   if (conn.State != ConnectionState.Open)
                   {
                       conn.Open();
                   }

                   var cmd = conn.CreateCommand();

                   cmd.CommandText = "create database " + GetDatabase_Bak(dbCreateName);
                   try
                   {
                       cmd.ExecuteNonQuery();

                       return CreateDbResultEnum.Success;
                   }
                   catch (Exception ex)
                   {
                       return CreateDbResultEnum.DbExist;
                   }
               }
               else
               {
                   return CreateDbResultEnum.ConnDisable;
               }
           }
       }

       public static CreateDbResultEnum CreateDb(DatabaseType dbType, string dbAddress, string dbCreateName, string user, string pwd)
       {
           if (dbType != DatabaseType.SqlServer && dbType != DatabaseType.MySql)
           {
               throw new ArgumentException("只支持mysql,sqlserver数据库的创建");
           }
           else
           {
               var connHelper = new DbConnectionHelper(dbType, dbAddress, GetMasterDb(dbType), user, pwd);
               if (connHelper.TestConn())
               {
                   var conn = connHelper.GetConnection();

                   if (conn.State != ConnectionState.Open)
                   {
                       conn.Open();
                   }

                   var cmd = conn.CreateCommand();


                   cmd.CommandText = "create database " + dbCreateName;
                   try
                   {
                       cmd.ExecuteNonQuery();
                       return CreateDbResultEnum.Success;
                   }
                   catch (Exception ex)
                   {
                       return CreateDbResultEnum.DbExist;
                   }
               }
               else
               {
                   return CreateDbResultEnum.ConnDisable;
               }
           }
       }
      
       public DbConnectionHelper(DatabaseType dbType, string connectionString)
       {
           init(dbType, connectionString);
       }

       private void init(DatabaseType dbType, string connectionString)
       {
           this.DbType = dbType;
           var dic = getConnDic(connectionString);
           if (DatabaseType.SqlServer == DbType)
           {
               DbAddress = getDicValue(dic, "server");
               UserName = getDicValue(dic, "uid");
               Password = getDicValue(dic, "pwd");
               Database = getDicValue(dic, "database");
           }
           else if (DatabaseType.MySql == DbType)
           {
               DbAddress = getDicValue(dic, "server");
               UserName = getDicValue(dic, "userid");
               if (UserName == "")
               {
                   string t = getDicValue(dic, "uid");
                   if (t != "")
                       UserName = t;
               }
               Password = getDicValue(dic, "password");
               if (Password == "")
               {
                   string t = getDicValue(dic, "pwd");
                   if (t != "")
                       Password = t;
               }
               Database = getDicValue(dic, "database");
           }
           else if (DatabaseType.Sqlite == DbType)
           {
               DbAddress = getDicValue(dic, "data source");
           }
           else if (DatabaseType.Oracle == DbType)
           {
               DbAddress = getDicValue(dic, "data source");
               UserName = getDicValue(dic, "user id");
               Password = getDicValue(dic, "password");
           }
           else
           {
               DbAddress = getDicValue(dic, "data source");
               Password = getDicValue(dic, "jet oledb:database password");
           }
       }

       public bool TestConn()
       {
           if (String.IsNullOrEmpty(this.Database) || String.IsNullOrEmpty(this.UserName) || String.IsNullOrEmpty(this.Password))
           {
               return false;
           }
           bool result = true;
           IDbConnection conn = null;
           try
           {
               conn = GetConnection(1);
               if (result == true)
               {
                   conn.Open();
                   if (conn.State == ConnectionState.Open)
                       result = true;
                   else
                       result = false;
               }
           }
           catch (Exception ex)
           {
               LogManager.Log("DbConnPart").Error(ex.Message, ex);
               result = false;
           }
           finally
           {
               if (conn != null && conn.State == ConnectionState.Open)
               {
                   conn.Close();
               }
           }

           return result;
       }

       public IDbConnection GetConnection(int connectionTimeout)
       {
           string connStr = this.ToConnectionString(1);

           IDbConnection conn = null;

           if (this.DbType == DatabaseType.SqlServer)
           {
               conn = new SqlConnection(connStr);
           }
           else if (this.DbType == DatabaseType.MySql)
           {
               conn = new MySqlConnection(connStr);
           }
           else if (this.DbType == DatabaseType.Sqlite)
           {
               conn = OperationSqlite.Instance.CreateConnection(connStr);
           }
           else if (this.DbType == DatabaseType.Oracle)
           {
               conn = new OracleConnection(connStr);
           }
           else
           {
               conn = new OleDbConnection(connStr);
           }

           return conn;

       }

       public IDbConnection GetConnectionFromConnStr(string connStr)
       {
           IDbConnection conn = null;

           if (this.DbType == DatabaseType.SqlServer)
           {
               conn = new SqlConnection(connStr);
           }
           else if (this.DbType == DatabaseType.MySql)
           {
               conn = new MySqlConnection(connStr);
           }
           else if (this.DbType == DatabaseType.Sqlite)
           {
               conn = OperationSqlite.Instance.CreateConnection(connStr);
           }
           else if (this.DbType == DatabaseType.Oracle)
           {
               conn = new OracleConnection(connStr);
           }
           else
           {
               conn = new OleDbConnection(connStr);
           }

           return conn;

       }

       public IDbConnection GetConnection()
       {
           string connStr = this.ToConnectionString();
           return GetConnectionFromConnStr(connStr);
          
       }

       public IDbConnection GetConnectionBak()
       {
           string connStr = this.ToConnectionStringBak();
           return GetConnectionFromConnStr(connStr);
       }

       public string GetProviderName()
       {
           if (this.DbType == DatabaseType.SqlServer)
           {
               return "System.Data.SqlClient";
           }
           else if (this.DbType == DatabaseType.MySql)
           {
               return  "MySql.Data.MySqlClient";
           }
           else if (this.DbType == DatabaseType.Sqlite)
           {
              return  "System.Data.SQLite";
           }
           else if (this.DbType == DatabaseType.Oracle)
           {
               return "System.Data.OracleClient";
           }
           else
           {
               return "System.Data.OleDb";
           }
       }

      
       private Dictionary<string, string> getConnDic(string connectionString)
       {
           connectionString = connectionString.ToLower();
           var arr = connectionString.Split(';');
           Dictionary<string, string> dic = new Dictionary<string, string>();
           foreach (string s in arr)
           {
               var p = s.Split('=');
               if (p.Length == 2)
               {
                   if (dic.ContainsKey(p[0]) == false)
                       dic.Add(p[0], p[1]);
               }
           }
           return dic;
       }

       private string getDicValue(Dictionary<string, string> dic, string key)
       {
           if (dic.ContainsKey(key))
               return dic[key];
           return "";
       }

       public string ToConnectionString(int connectionTimeOut)
       {
           if (DatabaseType.SqlServer == DbType)
           {
               return String.Format(@"server={0};uid={1};pwd={2};database={3};Connection Timeout={4}", DbAddress, UserName, Password, Database,connectionTimeOut);
           }
           else if (DatabaseType.MySql == DbType)
           {
               return String.Format(@"server={0};UserId={1};Password={2};Database={3};Connection Timeout={4}", DbAddress, UserName, Password, Database, connectionTimeOut);
           }
           else if (DatabaseType.Sqlite == DbType)
           {
               return String.Format(@"Data Source={0};Timeout={1}", DbAddress, connectionTimeOut);
           }
           else if (DatabaseType.Oracle == DbType)
           {//Data Source={0}/{1};Persist Security Info=True;User ID={2};Password={3};Unicode=True
               return String.Format(@"Data Source={0}/{1};Persist Security Info=True;User ID={2};Password={3};Unicode=True", DbAddress, Database, UserName, Password);
           }
           else
           {
               if (String.IsNullOrEmpty(Password))
               {
                   return String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Connect Timeout={1}", Database, connectionTimeOut);
               }
               else
               {
                   return String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};Persist Security Info=true;Connect Timeout={2}", Database, Password, connectionTimeOut);
               }
           }
       }

       private string ToConnectionString(string database)
       {
           if (DatabaseType.SqlServer == DbType)
           {
               return String.Format(@"server={0};uid={1};pwd={2};database={3}", DbAddress, UserName, Password, database);
           }
           else if (DatabaseType.MySql == DbType)
           {
               return String.Format(@"server={0};UserId={1};Password={2};Database={3}", DbAddress, UserName, Password, database);
           }
           else if (DatabaseType.Sqlite == DbType)
           {
               return String.Format(@"Data Source={0}", DbAddress);
           }
           else
           {
               if (String.IsNullOrEmpty(Password))
               {
                   return String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", database);
               }
               else
               {
                   return String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1};Persist Security Info=true;", database, Password);
               }
           }
       }

       public string ToConnectionString()
       {
           return ToConnectionString(Database);
       }

       public string ToConnectionStringBak()
       {
           return ToConnectionString(GetDatabase_Bak(Database));
       }


        #region static select type

       /// <summary>
       /// 得到 Mapping 对象
       /// </summary>
       /// <returns>Mapping对象</returns>
       public static OperationBase GetOperation(string dataAccessName)
       {
           string providerName = ConfigurationManager.ConnectionStrings[dataAccessName].ProviderName;
           if (providerName == "System.Data.SqlClient")
           {
               return new OperationSqlServer();
           }
           else if (providerName == "MySql.Data.MySqlClient")
           {
               return new OperationMySql();
           }
           else if (providerName == "System.Data.SQLite")
           {
               return new OperationSqlite();
           }
           else if(providerName == "System.Data.OracleClient")
           {
               return new OperationOracle();
           }
           else// if (providerName == "System.Data.OleDb")
           {
               return new OperationOleDb();
           }
       }

       /// <summary>
       /// 得到DatabaseType 
       /// </summary>
       /// <returns>DatabaseType</returns>
       public static DatabaseType GetDatabaseType(string dataAccessName)
       {
           string providerName = ConfigurationManager.ConnectionStrings[dataAccessName].ProviderName;
           return GetDatabaseTypeByProviderName(providerName);
       }

       /// <summary>
       /// 得到DatabaseType 
       /// </summary>
       /// <returns>DatabaseType</returns>
       public static DatabaseType GetDatabaseTypeByProviderName(string providerName)
       {
           if (providerName == "System.Data.SqlClient")
           {
               return DatabaseType.SqlServer;
           }
           else if (providerName == "MySql.Data.MySqlClient")
           {
               return DatabaseType.MySql;
           }
           else if (providerName == "System.Data.SQLite")
           {
               return DatabaseType.Sqlite;
           }
           else if (providerName == "System.Data.OracleClient")
           {
               return DatabaseType.Oracle;
           }
           else// if (providerName == "System.Data.OleDb")
           {
               return DatabaseType.OleDb;
           }
       }


       public static IDbConnection GetConnection(string dataAccessName)
       {
           var dbtype = GetDatabaseType(dataAccessName);
           var connConfig = ConfigurationManager.ConnectionStrings[dataAccessName];

           if (dbtype == DatabaseType.SqlServer)
           {
               return new SqlConnection(connConfig.ConnectionString);
           }
           else if (dbtype == DatabaseType.Sqlite)
           {
               return OperationSqlite.Instance.CreateConnection(connConfig.ConnectionString);
           }
           else if (dbtype == DatabaseType.MySql)
           {
               return new MySqlConnection(connConfig.ConnectionString);
           }
           else if (dbtype == DatabaseType.Oracle)
           {
               return new OracleConnection (connConfig.ConnectionString);
           }
           else// if (providerName == "System.Data.OleDb")
           {
               return new OleDbConnection(connConfig.ConnectionString);
           }
       }

      

       public static IDbDataAdapter GetDataAdapter(string dataAccessName)
       {
           var dbtype = GetDatabaseType(dataAccessName);

           if (dbtype == DatabaseType.SqlServer)
           {
               return new SqlDataAdapter();
           }
           else if (dbtype == DatabaseType.Sqlite)
           {
               return OperationSqlite.Instance.GetDataAdapter();
           }
           else if (dbtype == DatabaseType.MySql)
           {
               return new MySqlDataAdapter();
           }
           else if (dbtype == DatabaseType.Oracle)
           {
               return new OracleDataAdapter();
           }
           else// if (providerName == "System.Data.OleDb")
           {
               return new OleDbDataAdapter();
           }
       }

       


        #endregion

        #region static config
       public static DbConnectionHelper LoadFromAppConfig(string configPath, string dataAccessName)
       {
           DbConnectionHelper helper = null;
           XmlDocument doc = new XmlDocument();
           doc.Load(configPath);
           string p = String.Format(@"configuration/connectionStrings/add[@name='{0}']", dataAccessName);
           XmlNode node = doc.SelectSingleNode(p);
           if (node != null)
           {
               string connStr = node.Attributes["connectionString"].Value;
               var dbType = DbConnectionHelper.GetDatabaseType(node.Attributes["providerName"].Value);
               helper = new DbConnectionHelper(dbType, connStr);
           }
           return helper;
       }

       public static DbConnectionHelper LoadFromDataAccessName(string dataAccessName)
       {
           string connStr =ConfigurationManager.ConnectionStrings[dataAccessName].ConnectionString;
           LogManager.Log<DbConnectionHelper>().Info(connStr);
           var dbType = GetDatabaseType(dataAccessName);
           return new DbConnectionHelper(dbType, connStr);
       }

       public static void SaveAppConfig(string strFileName, string connectionString, string connectionStringBak, string providerName, string dataAccessName, string dataAccessNameBak)
       {
           XmlDocument doc = new XmlDocument();
           doc.Load(strFileName);
           string p = String.Format(@"configuration/connectionStrings/add[@name='{0}']", dataAccessName);
           XmlNode node = doc.SelectSingleNode(p);
           if (node != null)
           {
               node.Attributes["connectionString"].Value = connectionString;
               node.Attributes["providerName"].Value = providerName;
           }
           p = String.Format(@"configuration/connectionStrings/add[@name='{0}']", dataAccessNameBak);
           node = doc.SelectSingleNode(p);
           if (node != null)
           {
               node.Attributes["connectionString"].Value = connectionStringBak;
               node.Attributes["providerName"].Value = providerName;
           }
           //保存上面的修改
           doc.Save(strFileName);
       }

       #endregion
    }
}
