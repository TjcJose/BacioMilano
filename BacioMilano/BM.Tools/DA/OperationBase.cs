using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.OleDb;
//using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace BM.DA
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public abstract class OperationBase
    {
        /// <summary>
        /// 执行数库操作并且不返回值
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameterNames">参数名列表</param>
        /// <param name="parameterValues">参数值列表</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string commandText, List<string> parameterNames, List<object> parameterValues, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, parameterNames, parameterValues);
                cmd.CommandText = GetSqlFormat(commandText);
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteNonQuery();
            }
        }



        public int ExecuteNonQuery(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, proc);
                cmd.CommandText = proc.GetProcName();
                cmd.CommandType = CommandType.StoredProcedure;
                var r = cmd.ExecuteNonQuery();
                ChangeParameterValues(cmd.Parameters, proc);
                return r;
            }
        }


        /// <summary>
        /// 执行数库操作返回值第一行第一列
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameterNames">参数名列表</param>
        /// <param name="parameterValues">参数值列表</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象</param>
        /// <returns>第一行第一列的值</returns>
        public object ExecuteScalar(string commandText, List<string> parameterNames, List<object> parameterValues, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, parameterNames, parameterValues);
                cmd.CommandText = GetSqlFormat(commandText);
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteScalar();
            }
        }



        public object ExecuteScalar(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, proc);
                cmd.CommandText = proc.GetProcName();
                cmd.CommandType = CommandType.StoredProcedure;
                ChangeParameterValues(cmd.Parameters, proc);
                return cmd.ExecuteScalar();
            }
        }


        /// <summary>
        /// 执行数库操作并且不返回值
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameterNames">参数名列表</param>
        /// <param name="parameterValues">参数值列表</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象</param>
        /// <returns>IDataReader对象</returns>
        public IDataReader ExecuteReader(string commandText, List<string> parameterNames, List<object> parameterValues, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, parameterNames, parameterValues);
                cmd.CommandText = GetSqlFormat(commandText);
                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteReader();
            }
        }



        public IDataReader ExecuteReader(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, proc);
                cmd.CommandText = proc.GetProcName();
                cmd.CommandType = CommandType.Text;
                ChangeParameterValues(cmd.Parameters, proc);
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 执行数库操作并且不返回值
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameterNames">参数名列表</param>
        /// <param name="parameterValues">参数值列表</param>
        /// <param name="da">IDbDataAdapter对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象</param>
        /// <returns>DataSet对象</returns>
        public DataSet ExecuteDataSet(string commandText, List<string> parameterNames, List<object> parameterValues, IDbDataAdapter da, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                    conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, parameterNames, parameterValues);
                cmd.CommandText = GetSqlFormat(commandText);
                cmd.CommandType = CommandType.Text;


                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }


        public DataSet ExecuteDataSet(BaseCmdProc proc, IDbDataAdapter da, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, proc);
                cmd.CommandText = proc.GetProcName();
                cmd.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                ChangeParameterValues(cmd.Parameters, proc);

                return ds;
            }
        }

        #region GetExectueCommand
        /// <summary>
        /// 得到待执行的命令
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameterNames">要添加的参数名列表</param>
        /// <param name="parameterValues">要添加的参数值列表</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象</param>
        /// <returns>待执行的命令</returns>
        public IDbCommand GetExectueCommand(string commandText, List<string> parameterNames, List<object> parameterValues, IDbConnection conn, IDbTransaction trans)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            using (IDbCommand cmd = conn.CreateCommand())
            {

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                SetIDbCommandParameter(cmd, parameterNames, parameterValues);
                cmd.CommandText = GetSqlFormat(commandText);
                cmd.CommandType = CommandType.Text;
                return cmd;
            }
        }
        #endregion

        #region SetIDbCommandParameter
        /// <summary>
        /// IDbCommand 参数赋值
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <param name="parameterNames">要添加的参数名列表</param>
        /// <param name="parameterValues">要添加的参数值列表</param>
        protected virtual void SetIDbCommandParameter(IDbCommand cmd, List<string> parameterNames, List<object> parameterValues)
        {
            if (parameterNames != null && parameterValues != null)
            {
                for (int i = 0; i < parameterNames.Count; i++)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = parameterNames[i];
                    parameter.Value = parameterValues[i];
                    parameter.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// IDbCommand 参数赋值
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterValue">参数值</param>
        /// <param name="parameterDirection">参数类型</param>
        protected virtual void SetIDbCommandParameter(ref IDbCommand cmd, string parameterName, object parameterValue, ParameterDirection parameterDirection)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Direction = parameterDirection;
            cmd.Parameters.Add(parameter);
        }

        /// <summary>
        /// IDbCommand 参数赋值
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <param name="proc">要添加的参数值列表</param>
        protected virtual void SetIDbCommandParameter(IDbCommand cmd, BaseCmdProc proc)
        {
            var atts = proc.GetType().GetProperties();
            foreach (var att in atts)
            {
                IDbDataParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = att.Name;
                try
                {
                    parameter.Value = att.GetValue(proc, null);
                }
                catch { };
                cmd.Parameters.Add(parameter);
            }
        }

        #endregion

        #region ChangeParameterValues
        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <param name="parameterValues"></param>
        protected virtual void ChangeParameterValues(IDataParameterCollection commandParameters, BaseCmdProc proc)
        {
            var atts = proc.GetType().GetProperties();
            foreach (IDbDataParameter param in commandParameters)
            {
                foreach (var att in atts)
                {
                    if (att.Name == param.ParameterName)
                    {
                        att.SetValue(proc, param.Value, null);
                        break;
                    }
                }
            }
        }
        #endregion


        protected virtual string GetSqlFormat(string sql)
        {
            return sql;
        }


        #region public static GetOperation

        /// <summary>
        /// 得到 Mapping 对象
        /// </summary>
        /// <returns>Mapping对象</returns>
        public static OperationBase GetOperation(string dataAccessName)
        {
            if (operation == null)
            {
               operation = DbConnectionHelper.GetOperation(dataAccessName);
            }
            return operation;
        }

        public static IDbConnection GetConnection(string dataAccessName)
        {
            return DbConnectionHelper.GetConnection(dataAccessName);
        }

        public static DatabaseType GetDatabaseType(string dataAccessName)
        {
            return DbConnectionHelper.GetDatabaseType(dataAccessName);
        }

        public static IDbDataAdapter GetDataAdapter(string dataAccessName)
        {
            return DbConnectionHelper.GetDataAdapter(dataAccessName);
        }

        public static string[] AddPropertyFields(string[] originProperties, params string[] addProperties)
        {
            List<string> li = new List<string>();
            if (originProperties != null)
            {
                foreach (string s in originProperties)
                {
                    li.Add(s.ToLower());
                }
            }
            foreach (string s in addProperties)
            {
                if (!li.Contains(s.ToLower()))
                {
                    li.Add(s.ToLower());
                }
            }
            return li.ToArray();
        }

        public static string GetSelectTableFields(string fields, string tableName)
        {
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            string[] arr = fields.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(String.Format(@"{0}.{1}", tableName, arr[i]));
                if (i < arr.Length - 1)
                {
                    sb.Append(',');
                }
            }
            return sb.ToString();
        }

        private static OperationBase operation;
        #endregion

        public abstract IDbConnection CreateConnection(string connectionString);

        public abstract IDbDataAdapter GetDataAdapter();


    }
}
