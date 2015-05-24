using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OracleClient;

namespace BM.DA
{
    public class OperationOracle : OperationBase
    {
        private string FormatParameterName(string parameterName)
        {
            if(parameterName[0] != '?')
            {
                return "?" + parameterName;
            }
            return parameterName;
        }

        private string GetNotFormatParameterName(string parameterName)
        {
            if (parameterName[0] == '?')
            {
                return parameterName.Substring(1);
            }
            return parameterName;
        }

        #region SetIDbCommandParameter
        /// <summary>
        /// IDbCommand 参数赋值
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <param name="parameterNames">要添加的参数名列表</param>
        /// <param name="parameterValues">要添加的参数值列表</param>
        protected override void SetIDbCommandParameter(IDbCommand cmd, List<string> parameterNames, List<object> parameterValues)
        {
            if (parameterNames != null && parameterValues != null)
            {
                for (int i = 0; i < parameterNames.Count; i++)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = parameterNames[i];
                    if (parameterValues[i].GetType() == typeof(DateTime))
                    {
                        parameter.Value = parameterValues[i].ToString();
                    }
                    else
                    {
                        parameter.Value = parameterValues[i];
                    }
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
        protected override void SetIDbCommandParameter(ref IDbCommand cmd, string parameterName, object parameterValue, ParameterDirection parameterDirection)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = FormatParameterName(parameterName);
            if (parameterValue.GetType() == typeof(DateTime))
            {
                parameter.Value = parameterValue.ToString();
            }
            else
            {
                parameter.Value = parameterValue;
            }
            parameter.Direction = parameterDirection;
            cmd.Parameters.Add(parameter);
        }

        /// <summary>
        /// IDbCommand 参数赋值
        /// </summary>
        /// <param name="cmd">IDbCommand</param>
        /// <param name="proc">要添加的参数值列表</param>
        protected override void SetIDbCommandParameter(IDbCommand cmd, BaseCmdProc proc)
        {
            var atts = proc.GetType().GetProperties();
            foreach (var att in atts)
            {
                IDbDataParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = FormatParameterName(att.Name);
                try
                {
                    object objValue = att.GetValue(proc, null);
                    if (objValue.GetType() == typeof(DateTime))
                    {
                        parameter.Value = objValue.ToString();
                    }
                    else
                    {
                        parameter.Value = objValue;
                    }
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
        protected override void ChangeParameterValues(IDataParameterCollection commandParameters, BaseCmdProc proc)
        {
            var atts = proc.GetType().GetProperties();
            foreach (IDbDataParameter param in commandParameters)
            {
                foreach (var att in atts)
                {
                    if (att.Name == GetNotFormatParameterName(param.ParameterName))
                    {
                        object objValue = att.GetValue(proc, null);
                        if (objValue.GetType() == typeof(DateTime))
                        {
                            att.SetValue(proc, DateTime.Parse(param.Value.ToString()), null);
                        }
                        else
                        {
                            att.SetValue(proc, param.Value, null);
                        }

                        break;
                    }
                }
            }
        }
        #endregion


        protected override string GetSqlFormat(string sql)
        {
            return sql.Replace('@', ':');
        }

        public override IDbDataAdapter GetDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }
    }
}
