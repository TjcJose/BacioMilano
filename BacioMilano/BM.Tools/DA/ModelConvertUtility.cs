using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using BM.Cache;
using BM.Util;
using BM.Log;

namespace BM.DA
{
    /// <summary>
    /// 实体转化工具
    /// </summary>
    public class ModelConvertUtility
    {
        #region ConvertDataTableToModelList 得到实体类型列表
        /// <summary>
        /// 得到实体类型列表
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="dt">DataTable对象</param>
        /// <param name="exDataRowToModelDelegate">实体赋值扩展</param>
        /// <returns>实体类型列表</returns>
        public static List<T> ConvertDataTableToModelList<T>(DataTable dt, ExDataRowToModelDelegate<T> exDataRowToModelDelegate) where T : new()
        {
            List<T> list = new List<T>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T model = ConvertDataRowToModel<T>(row, exDataRowToModelDelegate);
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 得到实体类型列表
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="dt">DataTable对象</param>
        /// <returns>实体类型列表</returns>
        public static List<T> ConvertDataTableToModelList<T>(DataTable dt) where T : new()
        {
            return ConvertDataTableToModelList<T>(dt, null);
        }

        #endregion

        #region ConvertDataRowToModel 模型赋值
        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="row">数据读取器</param>
        /// <param name="exDataRowToModelDelegate">实体赋值扩展</param>
        public static void ConvertDataRowToModel<T>(ref T model, DataRow row, ExDataRowToModelDelegate<T> exDataRowToModelDelegate)
        {
            ColumnUseReaderToModel<T>(ref model, new DataRowUseReader(row));
            if (exDataRowToModelDelegate != null)
            {
                exDataRowToModelDelegate(model, row);
            }
        }

        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="row">数据读取器</param>
        /// <param name="exDataRowToModelDelegate">实体赋值扩展</param>
        /// <returns>模型实体</returns>
        public static T ConvertDataRowToModel<T>(DataRow row, ExDataRowToModelDelegate<T> exDataRowToModelDelegate) where T : new()
        {
            T model = new T();
            ColumnUseReaderToModel<T>(ref model, new DataRowUseReader(row));
            if (exDataRowToModelDelegate != null)
            {
                exDataRowToModelDelegate(model, row);
            }
            return model;
        }

        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="row">数据读取器</param>
        public static void ConvertDataRowToModel<T>(ref T model, DataRow row)
        {
            ColumnUseReaderToModel<T>(ref model, new DataRowUseReader(row));
        }

        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="row">数据读取器</param>
        /// <returns>模型实体</returns>
        public static T ConvertDataRowToModel<T>(DataRow row) where T : new()
        {
            T model = new T();
            ColumnUseReaderToModel<T>(ref model, new DataRowUseReader(row));
            return model;
        }

        #endregion

        #region ConvertIDataReaderToModel 模型赋值
        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="reader">数据读取器</param>
        /// <param name="exIDataReaderToModelDelegate">实体赋值扩展</param>
        public static void ConvertIDataReaderToModel<T>(ref T model, IDataReader reader, ExIDataReaderToModelDelegate<T> exIDataReaderToModelDelegate)
        {
            ColumnUseReaderToModel<T>(ref model, new DataReaderUse(reader));
            if (exIDataReaderToModelDelegate != null)
            {
                exIDataReaderToModelDelegate(model, reader);
            }
        } 

        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="reader">数据读取器</param>
        /// <param name="exIDataReaderToModelDelegate">实体赋值扩展</param>
        /// <returns>模型实体</returns>
        public static T ConvertIDataReaderToModel<T>(IDataReader reader, ExIDataReaderToModelDelegate<T> exIDataReaderToModelDelegate) where T : new()
        {
            T model = new T();
            ColumnUseReaderToModel<T>(ref model, new DataReaderUse(reader));
            if (exIDataReaderToModelDelegate != null)
            {
                exIDataReaderToModelDelegate(model, reader);
            }
            return model;
        }

     
        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="reader">数据读取器</param>
        /// <returns>模型实体</returns>
        public static T ConvertIDataReaderToModel<T>(IDataReader reader) where T : new()
        {
            T model = new T();
            ColumnUseReaderToModel<T>(ref model, new DataReaderUse(reader));
            return model;
        }



        /// <summary>
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="reader">数据读取器</param>
        public static void ConvertIDataReaderToModel<T>(ref T model, IDataReader reader)
        {
            ColumnUseReaderToModel<T>(ref model, new DataReaderUse(reader));
        }




        #endregion

        #region ColumnUseReaderToModel 模型赋值
       

        /// <summary
        /// 模型赋值
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">模型实例</param>
        /// <param name="reader">数据读取器</param>
        internal static void ColumnUseReaderToModel<T>(ref T model, IColumnUseReader reader)
        {
           
            for (int i = 0; i < reader.Length; i++)
            {
                try
                {
                    string name = reader.GetName(i).Replace('[', ' ').Replace(']',' ').Trim();
                    if (CacheTypes<T>.Instance.GetProperties().Where(p => p.Name == name).Count() == 0)
                    {
                        continue;
                    }

                    PropertyInfo info = CacheTypes<T>.Instance.GetProperties().Where(p => p.Name == name).First();

                   

                    object value = reader.GetValue(i);
                    if (!value.Equals(System.DBNull.Value))
                    {
                        if (info.PropertyType.IsGenericType)
                        {
                            Type typeUse = info.PropertyType.GetGenericArguments()[0];
                            if (typeUse.IsEnum)
                            {
                                object obj = Enum.ToObject(typeUse, value);
                                info.SetValue(model, obj, null);
                            }
                            else
                            {
                                if (typeUse != value.GetType())
                                {
                                    try
                                    {
                                        object objv = System.Convert.ChangeType(value, typeUse);
                                        info.SetValue(model, objv, null);
                                    }
                                    catch (Exception ex) { LogHelper<ModelConvertUtility>.GetLogger().Warn(info.ToString(), ex); };
                                }
                                else
                                {
                                    info.SetValue(model, value, null);
                                }
                            }
                        }
                        else
                        {
                            if (info.PropertyType.IsEnum)
                            {
                                info.SetValue(model, Enum.ToObject(info.PropertyType, value), null);
                            }
                            else
                            {
                                info.SetValue(model, value, null);
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogHelper<ModelConvertUtility>.GetLogger().Error(reader.GetName(i), ex);
                };
            }

        }
        #endregion

        /// <summary>
        /// DataRow给实体赋值扩展代理
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="row">DataRow对象</param>
        public delegate void ExDataRowToModelDelegate<T>(T model, DataRow row);

        /// <summary>
        /// IDataReader给实体赋值扩展代理
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="reader">IDataReader对象</param>
        public delegate void ExIDataReaderToModelDelegate<T>(T model, IDataReader reader);
    }
}
