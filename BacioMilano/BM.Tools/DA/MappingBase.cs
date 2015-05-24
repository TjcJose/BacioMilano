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
using BM.Util;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.OracleClient;


namespace BM.DA
{
    /// <summary>
    /// ORMapping 基础类
    /// </summary>
    /// <typeparam name="T">ORMapping的实体类型</typeparam>
    public abstract class MappingBase<T> where T : IModel, new()
    {
        #region UpdateEmpty
        /// <summary>
        /// 设置为null
        /// </summary>
        /// <param name="propertiesEmpty">设置为空的属性数组</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int UpdateEmpty(object id, params string[] propertiesEmpty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.UpdateEmpty(id, conn, null, propertiesEmpty);
            }
        }

        /// <summary>
        /// 设置为null
        /// </summary>
        /// <param name="id">数据主键</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="propertiesEmpty">设置为空的属性数组</param>
        /// <returns></returns>
        public int UpdateEmpty(object id, IDbConnection conn, IDbTransaction trans, params string[] propertiesEmpty)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            return UpdateEmpty(propertiesEmpty, whereSql, values, conn, trans);
        }


        /// <summary>
        /// 设置为null
        /// </summary>
        /// <param name="propertiesEmpty">设置为空的属性数组</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int UpdateEmpty(string[] propertiesEmpty, string whereSql, object[] values)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.UpdateEmpty(propertiesEmpty, whereSql, values, conn, null);
            }
        }

        /// <summary>
        /// 设置为null
        /// </summary>
        /// <param name="propertiesEmpty">置为空的属性数组</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int UpdateEmpty(string[] propertiesEmpty, string whereSql, object[] values, IDbConnection conn, IDbTransaction trans)
        {
            if (propertiesEmpty == null || propertiesEmpty.Length == 0)
                return 0;

            var fields = MappingUtility.GetFields<T>(propertiesEmpty);
            List<object> fieldValues = new List<object>();
            for (int i = 0; i < fields.Count; i++)
            {
                fieldValues.Add(null);
            }
            return Update(fields, fieldValues, values, whereSql, conn, trans);
        }

        /// <summary>
        /// 更新实体（可给 canEmptyDic 字典中的空值字段赋给null)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="canEmptyDic">可赋空值字典</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int UpdateEmptyEntity(T entity, Dictionary<string, object> canEmptyDic, IDbConnection conn, IDbTransaction trans)
        {
            if (canEmptyDic == null || canEmptyDic.Count == 0)
                return Update(entity, conn, trans);

            for (int i = canEmptyDic.Count - 1; i >= 0; i--)
            {
                if (canEmptyDic.Values.ElementAt(i) != null)
                {
                    canEmptyDic.Remove(canEmptyDic.Keys.ElementAt(i));
                }
            }

            if (canEmptyDic.Count == 0)
            {
                return Update(entity, conn, trans);
            }

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            List<object> values;
            List<string> parameterNames = new List<string>();
            string whereSql = MappingUtility.GetPrimaryKeysWhereSqlFromEntity<T>(entity, out values, ref parameterNames);

            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(entity, false, out fields, out fieldValues);

            foreach (var pair in canEmptyDic)
            {
                fields.Add(helper.PropertyField_Dictionary[pair.Key]);
                fieldValues.Add(null);
            }


            return Update(fields, fieldValues, values.ToArray(), whereSql, conn, trans);
        }

        /// <summary>
        /// 更新实体（可给 canEmptyDic 字典中的空值字段赋给null)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="canEmptyDic">可赋空值字典</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int UpdateEmptyEntity(T entity, Dictionary<string, object> canEmptyDic)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.UpdateEmptyEntity(entity, canEmptyDic, conn, null);
            }
        }
        #endregion
      
        #region public Update

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public int Update(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Update(entity, conn, null);
            }
        }

       
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="ids">主键Ids</param>
        /// <returns>影响行数</returns>
        public int Update(T entity,  IDbConnection conn, IDbTransaction trans, params object[] ids)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            for(int i = 0; i < helper.PrimaryProperties.Length; i++) 
            {
                entity.GetType().GetProperty(helper.PrimaryProperties[i]).SetValue(entity, ids[i], null);
            }
            return this.Update(entity, conn, trans);
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="ids">主键Ids</param>
        /// <returns>影响行数</returns>
        public int Update(T entity, params object[] ids)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Update(entity, conn, null, ids);
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Update(T entity, string whereSql, object[] values)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Update(entity, whereSql, values, conn, null);
            }
        }


        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="propertiesValues">对应插入字段的属性名值字典</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Update(Dictionary<string, object> propertiesValues, string whereSql, object[] values)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Update(propertiesValues, whereSql, values, conn, null);
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Update(T entity, IDbConnection conn, IDbTransaction trans)
        {
            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(entity, false, out fields, out fieldValues);

            List<string> parameterNames = new List<string>();
            List<object> values;
            string whereSql = MappingUtility.GetPrimaryKeysWhereSqlFromEntity<T>(entity, out values, ref parameterNames);

            return this.Update(fields, fieldValues, values.ToArray(), whereSql, conn, trans);
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Update(T entity, string whereSql, object[] values, IDbConnection conn, IDbTransaction trans)
        {
            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(entity, false, out fields, out fieldValues);
            if (fields.Count == 0)
            {
                throw new ArgumentException(String.Format(@"{0} Update error, insert field is null", entity.GetType().Name));
            }

            return this.Update(fields, fieldValues, values, whereSql, conn, trans);
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="propertiesValues">对应插入字段的属性名值字典</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Update(Dictionary<string, object> propertiesValues, string whereSql, object[] values, IDbConnection conn, IDbTransaction trans)
        {
            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(propertiesValues, out fields, out fieldValues, false);
            if (fields.Count == 0)
            {
                throw new ArgumentException(String.Format(@"{0} Update error, insert field is null", typeof(T).GetType().Name));
            }

            return this.Update(fields, fieldValues, values, whereSql, conn, trans);
        }

        protected abstract int Update(List<string> fields, List<object> fieldValues, object[] values, string whereSql, IDbConnection conn, IDbTransaction trans);


        #endregion

        #region public InsertOrUpdate

        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public int InsertOrUpdate(T entity, IDbConnection conn, IDbTransaction trans)
        {
            if (this.IsExist(entity, conn, trans))
            {
                return this.Update(entity, conn, trans);
            }
            else
            {
                return this.Insert(entity, conn, trans);
            }
        }

        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int InsertOrUpdate(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.InsertOrUpdate(entity, conn, null);
            }
        }

        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public int InsertOrUpdate(T entity, object id, IDbConnection conn, IDbTransaction trans)
        {
            if (this.IsExistById(id, conn, trans))
            {
                return this.Update(entity, id, conn, trans);
            }
            else
            {
                return this.Insert(entity, id, conn, trans);
            }
        }

        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">主键</param>
        public int InsertOrUpdate(T entity, object id)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.InsertOrUpdate(entity, id, conn, null);
            }
        }



        #endregion

        #region public Insert

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public int Insert(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Insert(entity, conn, null);
            }
        }


        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="id">主键Ids</param>
        /// <returns>影响行数</returns>
        public int Insert(T entity, IDbConnection conn, IDbTransaction trans, params object[] ids)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                entity.GetType().GetProperty(helper.PrimaryProperties[i]).SetValue(entity, ids[i], null);
            }
            return this.Insert(entity, conn, trans);
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">主键Ids</param>
        /// <returns>影响行数</returns>
        public int Insert(T entity, params object[] ids)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Insert(entity, conn, null, ids);
            }
        }

        public int Insert(T entity, IDbConnection conn, IDbTransaction trans)
        {
            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(entity, true, out fields, out fieldValues);
            if (fields.Count == 0)
            {
                throw new ArgumentNullException(String.Format(@"{0} Insert error, insert field is null", typeof(T).Name));
            }

            return this.Insert(fields, fieldValues, conn, trans);
        }

        protected abstract int Insert(List<string> fields, List<object> fieldValues, IDbConnection conn, IDbTransaction trans);


        #endregion

        #region public InsertReturnIdentity

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>自增量主键</returns>
        public object InsertReturnIdentity(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.InsertReturnIdentity(entity, conn, null);
            }
        }


        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">键值Id</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>自增量主键</returns>
        public object InsertReturnIdentity(T entity, object id, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            entity.GetType().GetProperty(helper.PrimaryProperties[0]).SetValue(entity, id, null);
            return this.Insert(entity, conn, trans);
        }

        /// <summary>
        /// 添加操作（只有sqlserver实现了InsertReturnIdentity）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">主键</param>
        /// <returns>自增量主键（，只有sqlserver实现了此功能）</returns>
        public object InsertReturnIdentity(T entity, object id)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Insert(entity, id, conn, null);
            }
        }

        public object InsertReturnIdentity(T entity, IDbConnection conn, IDbTransaction trans)
        {
            List<string> fields;
            List<object> fieldValues;
            MappingUtility.GetFieldsValues<T>(entity, true, out fields, out fieldValues);
            if (fields.Count == 0)
            {
                throw new ArgumentNullException(String.Format(@"{0} Insert error, insert field is null", typeof(T).Name));
            }

            return this.InsertReturnIdentity(fields, fieldValues, conn, trans);
        }

        protected abstract object InsertReturnIdentity(List<string> fields, List<object> fieldValues, IDbConnection conn, IDbTransaction trans);


        #endregion

        #region AddNum 属性字段增值
        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="addProperties">增值字段数组</param>
        /// <param name="nums">增值数组</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public abstract int AddNum(string whereSql, object[] values, string[] addProperties, object[] nums, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="addProperties">增值字段数组</param>
        /// <param name="nums">增值数组</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int AddNum(string whereSql, object[] values, string[] addProperties, object[] nums)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.AddNum(whereSql, values, addProperties, nums, conn, null);
            }
        }

        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="addProperty">增量字段</param>
        /// <param name="addNum">增量值</param>
        /// <returns>影响行数</returns>
        public int AddNum(object id, string addProperty, int addNum)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.AddNum(id, addProperty, addNum, conn, null);
            }
        }

        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="addProperty">增量字段</param>
        /// <param name="addNum">增量值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int AddNum(object id, string addProperty, int addNum, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@[0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            return this.AddNum(whereSql, values, new string[] { addProperty }, new object[] { addNum }, conn, trans);
        }

        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="addProperties">增量字段</param>
        /// <param name="nums">增量值</param>
        /// <returns>影响行数</returns>
        public int AddNum(object id, string[] addProperties, object[] nums)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.AddNum(id, addProperties, nums, conn, null);
            }
        }

        /// <summary>
        /// 属性字段增值
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="addProperties">增量字段</param>
        /// <param name="nums">增量值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int AddNum(object id, string[] addProperties, object[] nums, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@[0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            return this.AddNum(whereSql, values, addProperties, nums, conn, trans);
        }

        #endregion

        #region SortSwap 交换属性字段值
        /// <summary>
        /// 交换属性字段值
        /// </summary>
        /// <param name="primaryValue1">第一条记录的主键</param>
        /// <param name="uniqueValue2">第二条记录的主键</param>
        /// <param name="swapProperty">交换值的字段</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param
        public void SortSwap(object primaryValue1, object primaryValue2, string swapProperty, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string[] selectProperties = new string[] { helper.PrimaryProperties[0], swapProperty };
            var table = this.SelectByValuesT(selectProperties, helper.PrimaryProperties[0], new object[] { primaryValue1, primaryValue2 }, null, null, conn, trans);

            object temp = table.Rows[0][1];
            table.Rows[0][1] = table.Rows[1][1];
            table.Rows[1][1] = temp;

            var entitys = ModelConvertUtility.ConvertDataTableToModelList<T>(table);
            this.Update(entitys[0], conn, trans);
            this.Update(entitys[1], conn, trans);
        }

        /// <summary>
        /// 交换属性字段值
        /// </summary>
        /// <param name="primaryValue1">第一条记录的主键</param>
        /// <param name="uniqueValue2">第二条记录的主键</param>
        /// <param name="swapProperty">交换值的字段</param>
        public void SortSwap(object primaryValue1, object primaryValue2, string swapProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                this.SortSwap(primaryValue1, primaryValue2, swapProperty, conn, null);
            }
        }

        #endregion

        #region  SortMaxOrMin 设置最大或最小值
        /// <summary>
        /// 设置最大或最小值
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="primaryValue">要排序记录的主键值</param>
        /// <param name="sortProperty">要排序的字段</param>
        /// <param name="isMax">是否最大</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int SortMaxOrMin(string whereSql, object[] values, object primaryValue, string sortProperty, bool isMax, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            int sort;
            if (isMax)
            {
                sort = Util.ObjectConvert.GetIntValue(this.SelectStatisticObject(whereSql, values, StatisticType.Max, sortProperty)) + 5;

            }
            else
            {
                sort = Util.ObjectConvert.GetIntValue(this.SelectStatisticObject(whereSql, values, StatisticType.Min, sortProperty)) - 5;
            }
            Dictionary<string, object> propertiesValues = new Dictionary<string, object>();
            propertiesValues.Add(sortProperty, sort);
            whereSql = String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]);
            return this.Update(propertiesValues, whereSql, new object[] { primaryValue }, conn, trans);
        }

        /// <summary>
        /// 设置最大或最小值
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="primaryValue">要排序记录的主键值</param>
        /// <param name="sortProperty">要排序的字段</param>
        /// <param name="isMax">是否最大</param>
        /// <returns>影响行数</returns>
        public int SortMaxOrMin(string whereSql, object[] values, object primaryValue, string sortProperty, bool isMax)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SortMaxOrMin(whereSql, values, primaryValue, sortProperty, isMax, conn, null);
            }
        }

        /// <summary>
        /// 设置最大或最小值
        /// </summary>
        /// <param name="primaryValue">要排序记录的主键值</param>
        /// <param name="sortProperty">要排序的字段</param>
        /// <param name="isMax">是否最大</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int SortMaxOrMin(object primaryValue, string sortProperty, bool isMax, IDbConnection conn, IDbTransaction trans)
        {
            return SortMaxOrMin(null, null, primaryValue, sortProperty, isMax, conn, trans);
        }

        /// <summary>
        /// 设置最大或最小值
        /// </summary>
        /// <param name="primaryValue">要排序记录的主键值</param>
        /// <param name="sortProperty">要排序的字段</param>
        /// <param name="isMax">是否最大</param>
        /// <returns>影响行数</returns>
        public int SortMaxOrMin(object primaryValue, string sortProperty, bool isMax)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SortMaxOrMin(primaryValue, sortProperty, isMax, conn, null);
            }
        }
        #endregion

        #region SortMoveTo 设置位置
        private void SortMoveToUp(int sortFrom, int sortTo, string sortProperty, string[] selectProperties, DataRow row, IDbConnection conn, IDbTransaction trans)
        {
            string whereSql = String.Format(@"#{0}>@{0}1 and #{0}<=@{0}2 order by #{0} desc", sortProperty);
            var table = this.SelectT(selectProperties, whereSql, new object[] { sortFrom, sortTo }, false, conn, trans);



            row[1] = sortTo;
            var entity = ModelConvertUtility.ConvertDataRowToModel<T>(row);
            this.Update(entity, conn, trans);

            int temp = sortTo;
            int i = 0;
            for (; i < table.Rows.Count - 1; i++)
            {
                int a = Util.ObjectConvert.GetIntValue(table.Rows[i][1]);

                if (temp - a > 1)
                {
                    table.Rows[i][1] = temp - (int)((temp - a) / 2);
                    entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                    this.Update(entity, conn, trans);
                    return;
                }


                table.Rows[i][1] = table.Rows[i + 1][1];
                entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                this.Update(entity, conn, trans);

                temp = a;
            }

            table.Rows[i][1] = sortFrom;
            entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
            this.Update(entity, conn, trans);
        }

        private void SortMoveToDown(int sortFrom, int sortTo, string sortProperty, string[] selectProperties, DataRow row, IDbConnection conn, IDbTransaction trans)
        {
            string whereSql = String.Format(@"#{0}>=@{0}1 and #{0}<@{0}2 order by #{0} asc", sortProperty);
            var table = this.SelectT(selectProperties, whereSql, new object[] { sortTo, sortFrom }, false, conn, trans);

            T entity;
            int temp;
            if (table.Rows.Count > 0)
            {
                temp = Util.ObjectConvert.GetIntValue(table.Rows[0][1]);
                if (temp - sortTo > 1)
                {
                    row[1] = temp + (int)((temp - sortTo) / 2);
                    entity = ModelConvertUtility.ConvertDataRowToModel<T>(row);
                    this.Update(entity, conn, trans);
                    return;
                }

                row[1] = temp;
                entity = ModelConvertUtility.ConvertDataRowToModel<T>(row);
                this.Update(entity, conn, trans);

                int i = 0;
                for (; i < table.Rows.Count - 1; i++)
                {
                    int a = (int)table.Rows[i][1];

                    if (temp - a > 1)
                    {
                        table.Rows[i][1] = temp + (int)((temp - a) / 2);
                        entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                        this.Update(entity, conn, trans);
                        return;
                    }

                    table.Rows[i][1] = table.Rows[i + 1][1];
                    entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                    this.Update(entity, conn, trans);

                    temp = a;
                }

                table.Rows[i][1] = sortFrom;
                entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                this.Update(entity, conn, trans);
            }
        }

        /// <summary>
        /// 设置位置(前提条件,排序字段值不重复(降序)
        /// </summary>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="primaryValueTo">移动到记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public void SortMoveTo(object primaryValueFrom, object primaryValueTo, string sortProperty, IDbConnection conn, IDbTransaction trans)
        {
            if (primaryValueFrom.Equals(primaryValueTo)) return;

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string[] selectProperties = new string[] { helper.PrimaryProperties[0], sortProperty };
            var table = this.SelectByValuesT(selectProperties, helper.PrimaryProperties[0], new object[] { primaryValueFrom, primaryValueTo }, null, null, conn, trans);

            int sortFrom, sortTo;
            DataRow row;
            if (table.Rows[0][0].Equals(primaryValueFrom))
            {
                sortFrom = (int)table.Rows[0][1];
                sortTo = (int)table.Rows[1][1];
                row = table.Rows[0];
            }
            else
            {
                sortTo = (int)table.Rows[0][1];
                sortFrom = (int)table.Rows[1][1];
                row = table.Rows[1];
            }

            if (sortFrom < sortTo)
            {
                this.SortMoveToUp(sortFrom, sortTo, sortProperty, selectProperties, row, conn, trans);
            }
            else
            {
                this.SortMoveToDown(sortFrom, sortTo, sortProperty, selectProperties, row, conn, trans);

               
            }
        }

        /// <summary>
        /// 设置位置(前提条件,排序字段值不重复)(降序)
        /// </summary>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="primaryValueTo">移动到记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        public void SortMoveTo(object primaryValueFrom, object primaryValueTo, string sortProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                this.SortMoveTo(primaryValueFrom, primaryValueTo, sortProperty, conn, null);
            }
        }
        #endregion

        #region SortUpOrDown 设置位置移一位(降序)
        /// <summary>
        /// 设置位置移一位(降序)
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="isUpOrDown">是向上还是向下</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public void SortUpOrDown(string whereSql, object[] values, object primaryValue, string sortProperty, bool isUpOrDown, IDbConnection conn, IDbTransaction trans)
        {
            List<object> lsValue = values == null ? null : values.ToList();
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string[] selectProperies = { sortProperty };

            var t = this.SelectT(selectProperies, String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]), new object[] { primaryValue }, false, conn, trans);
            if (t.Rows.Count == 0)
                return;


            int sort = Util.ObjectConvert.GetIntValue(t.Rows[0][0]);

            selectProperies = new string[] { helper.PrimaryProperties[0], sortProperty };
            object[] values1;
            if (String.IsNullOrEmpty(whereSql))
            {
                whereSql = isUpOrDown ? String.Format(@"#{0}>=@{0} order by #{0} asc", sortProperty) : String.Format(@"#{0}<=@{0} order by #{0} desc", sortProperty);
                values1 = new object[] { sort };
            }
            else
            {
                whereSql = isUpOrDown ? String.Format(@"#{0}>=@{0} and {1} order by #{0} asc", sortProperty, whereSql) : String.Format(@"#{0}<=@{0} and {1} order by #{0} desc", sortProperty, whereSql);
                if (lsValue == null)
                {
                    values1 = new object[] { sort };
                }
                else
                {
                    lsValue.Insert(0, sort);
                    values1 = lsValue.ToArray();
                }
            }

            t = this.SelectTopT(selectProperies, whereSql, values1, false, 2, conn, trans);
            if (t.Rows.Count == 2)
            {
                var temp = t.Rows[0][1];
                t.Rows[0][1] = t.Rows[1][1];
                t.Rows[1][1] = temp;

                this.SortSwap(t.Rows[0][0], t.Rows[1][0], sortProperty, conn, trans);
            }
        }


        /// <summary>
        /// 设置位置移一位(降序)
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="isUpOrDown">是向上还是向下</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public void SortUpOrDown(string whereSql, object[] values, object primaryValue, string sortProperty, bool isUpOrDown)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                this.SortUpOrDown(whereSql, values, primaryValue, sortProperty, isUpOrDown, conn, null);
            }
        }


        /// <summary>
        /// 设置位置移一位(降序)
        /// </summary>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="isUpOrDown">是向上还是向下</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public void SortUpOrDown(object primaryValue, string sortProperty, bool isUpOrDown, IDbConnection conn, IDbTransaction trans)
        {
            SortUpOrDown(null, null, primaryValue, sortProperty, isUpOrDown, conn, trans);
        }

        /// <summary>
        /// 设置位置移一位(降序)
        /// </summary>
        /// <param name="primaryValueFrom">开始移动记录的主键值</param>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="isUpOrDown">是向上还是向下</param>
        public void SortUpOrDown(object primaryValue, string sortProperty, bool isUpOrDown)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                this.SortUpOrDown(primaryValue, sortProperty, isUpOrDown, conn, null);
            }
        }
        #endregion

        #region SortInit 初始化排序字段
        /// <summary>
        /// 初始化排序字段
        /// </summary>
        /// <param name="sortProperty">排序字段</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public void SortInit(string sortProperty, IDbConnection conn, IDbTransaction trans)
        {
            //if ((int)this.SelectStatistic(null, null, StatisticType.Count, sortProperty, conn, trans) != this.SelectCount(null, null, false, conn, trans))
            //{
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string[] selectProperties = new string[] { helper.PrimaryProperties[0], sortProperty };
            string whereSql = String.Format(@"order by #{0} asc", sortProperty);
            var table = this.SelectT(selectProperties, whereSql, null, false, conn, trans);

            int sort = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i][1] = sort;
                sort += 5;
                var entity = ModelConvertUtility.ConvertDataRowToModel<T>(table.Rows[i]);
                this.Update(entity, conn, trans);
            }
            //}
        }

        /// <summary>
        /// 初始化排序字段
        /// </summary>
        /// <param name="sortProperty">排序字段</param>
        public void SortInit(string sortProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                this.SortInit(sortProperty, conn, null);
            }

        }
        #endregion

        #region public Delete
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public abstract int Delete(string whereSql, object[] values, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="whereDic">查询条件</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Delete(Dictionary<string, object> whereDic, IDbConnection conn, IDbTransaction trans)
        {
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            int i = 0;
            foreach (var pair in whereDic)
            {
                if(i != 0){
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", pair.Key);
                ls.Add(pair.Value);
                i++;
            }
            return this.Delete(sb.ToString(), ls.ToArray(), conn, trans);
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="whereDic">查询条件</param>
        /// <returns>影响行数</returns>
        public int Delete(Dictionary<string, object> whereDic)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Delete(whereDic, conn, null);
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int Delete(string whereSql, object[] values)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Delete(whereSql, values, conn, null);
            }
        }



        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int Delete(T entity, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", helper.PrimaryProperties[i]);
                var obj = entity.GetType().GetProperty(helper.PrimaryProperties[i]).GetValue(entity, null);
                if(obj == null)
                {
                    return 0;
                }
                ls.Add(obj);
            }

            return this.Delete(sb.ToString(), ls.ToArray(), conn, trans);
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public int Delete(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Delete(entity, conn, null);
            }
        }

        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int DeleteById(object id, IDbConnection conn, IDbTransaction trans)
        {
            if (id == null)
            {
                return 0;
            }
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            return this.Delete(whereSql, values, conn, trans);
        }

        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int DeleteById(object id)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.DeleteById(id, conn, null);
            }
        }

        /// <summary>
        /// 通过主键值数组删除操作
        /// </summary>
        /// <param name="ids">主键值数组</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public int DeleteByIds(object[] ids, IDbConnection conn, IDbTransaction trans)
        {
            if (ids == null || ids.Length == 0)
            {
                return 0;
            }
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (ids.Length == 1)
            {
                this.DeleteById(ids[0], conn, trans);
            }
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            int i = 0;
            for (; i < ids.Length - 1; i++)
            {
                sb.AppendFormat(@"#{0}=@{0}{1} or ", helper.PrimaryProperties[i], i);
                ls.Add(ids[i]);
            }
            sb.AppendFormat(@"#{0}=@{0}{1}", helper.PrimaryProperties[i], i);
            ls.Add(ids[i]);

            return this.Delete(sb.ToString(), ls.ToArray(), conn, trans);
        }

        /// <summary>
        /// 通过主键值数组删除操作
        /// </summary>
        /// <param name="ids">主键值数组</param>
        /// <param name="values">查询参数值</param>
        /// <returns>影响行数</returns>
        public int DeleteByIds(object[] ids)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.DeleteByIds(ids, conn, null);
            }
        }


        #endregion

        #region SelectSplit 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectSplit(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            var dt = SelectSplitT(selectProperties, whereSql, values, isDistinct, pageIndex, pageSize, out  pageCount, out  recordCount, conn, trans);
            if (dt == null)
            {
                return new List<T>();
            }
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public abstract DataTable SelectSplitT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectSplit(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                if(conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    return this.SelectSplit(selectProperties, whereSql, values, isDistinct, pageIndex, pageSize, out pageCount, out recordCount, conn, ts);
                }
            }
        }



        #endregion

        #region Select 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="selectProperties">查询属性</param>
        /// <returns>DataTable</returns>
        public IList<T> Select(string whereSql, object[] values, bool isDistinct, IDbConnection conn, IDbTransaction trans, params string[] selectProperties)
        {
            var dt = this.SelectT(selectProperties, whereSql, values, isDistinct, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="selectProperties">查询属性</param>
        /// <returns>DataTable</returns>
        public IList<T> Select(string whereSql, object[] values, bool isDistinct, params string[] selectProperties)
        {
            var dt = this.SelectT(selectProperties, whereSql, values, isDistinct);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public IList<T> Select(string[] selectProperties, string whereSql, object[] values, bool isDistinct, IDbConnection conn, IDbTransaction trans)
        {
            var dt = this.SelectT(selectProperties, whereSql, values, isDistinct, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <returns>DataTable</returns>
        public IList<T> Select(string[] selectProperties, string whereSql, object[] values, bool isDistinct)
        {
            var dt = this.SelectT(selectProperties, whereSql, values, isDistinct);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        /// <summary>
        /// 从存储过程得到查询
        /// </summary>
        /// <param name="proc">存储过程实体</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public IList<T> Select(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            var dt = this.SelectT(proc, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }
        public IList<T> Select(BaseCmdProc proc)
        {
            var dt = this.SelectT(proc);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }





        #endregion SelectByProperty

        #region SelectByProperty

        /// <summary>
        /// 根据属性获取列表
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public IList<T> SelectByProperty(string propertyName, object propertyValue, IDbConnection conn, IDbTransaction trans)
        {
            string whereSql = String.Format(@"#{0}=@{0}", propertyName);
            object[] values = { propertyValue };
            return Select(whereSql, values, false, conn, trans);
        }

        /// <summary>
        /// 根据属性获取列表
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <returns></returns>
        public IList<T> SelectByProperty(string propertyName, object propertyValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectByProperty(propertyName, propertyValue, conn, null);
            }
        }

        /// <summary>
        /// 根据某字段获取列表
        /// </summary>
        /// <param name="propertyName">查询条件属性</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="propertyOrderName">排序属性</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="selectProperties">查询的字段</param>
        /// <returns></returns>
        public IList<T> SelectByProperty(string propertyName, object propertyValue, string propertyOrderName, bool isAsc, IDbConnection conn, IDbTransaction trans, params string[] selectProperties)
        {
            string order = isAsc ? "asc" : "desc";
            string whereSql = String.Format(@"#{0}=@{0} order by #{1} {2}", propertyName, propertyOrderName, order);
            object[] values = { propertyValue };
            return Select(whereSql, values, false, conn, trans, selectProperties);
        }

        /// <summary>
        /// 根据某字段获取列表
        /// </summary>
        /// <param name="propertyName">查询条件属性</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="propertyOrderName">排序属性</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="selectProperties">查询的字段</param>
        /// <returns></returns>
        public IList<T> SelectByProperty(string propertyName, object propertyValue, string propertyOrderName, bool isAsc, params string[] selectProperties)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectByProperty(propertyName, propertyValue, propertyOrderName, isAsc, conn, null, selectProperties);
            }
        }

        #endregion

        #region SelectByProperties

        /// <summary>
        /// 根据某些属性获取列表
        /// </summary>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public IList<T> SelectByProperties(Dictionary<string, object> dictPropertyNamesValues, IDbConnection conn, IDbTransaction trans)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dictPropertyNamesValues.Keys.Count; i++)
            {
                sb.AppendFormat(@"#{0}=@{0}", dictPropertyNamesValues.Keys.ElementAt(i));
                if (i < dictPropertyNamesValues.Keys.Count - 1)
                {
                    sb.Append(" and ");
                }
            }
            string whereSql = sb.ToString();
            object[] values = dictPropertyNamesValues.Values.ToArray();
            return Select(whereSql, values, false, conn, trans);
        }

        /// <summary>
        /// 根据多属性获取列表
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <returns></returns>
        public IList<T> SelectByProperties(Dictionary<string, object> dictPropertyNamesValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectByProperties(dictPropertyNamesValues, conn, null);
            }
        }

        #endregion


        #region SelectTRecursion

        public abstract DataTable SelectTRecursion(string[] selectProperties, string whereSql, object[] values, string propertyIdName, string propertyFatherIdName, object propertyFatherIdValue, IDbConnection conn, IDbTransaction trans);

        public  DataTable SelectTRecursion(string[] selectProperties, string whereSql, object[] values, string propertyIdName, string propertyFatherIdName, object propertyFatherIdValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectTRecursion(selectProperties, whereSql, values, propertyIdName, propertyFatherIdName, propertyFatherIdValue, conn, null);
            }
        }

        #endregion

        #region SelectRecursion
        public IList<T> SelectRecursion(string[] selectProperties, string whereSql, object[] values, string propertyIdName, string propertyFatherIdName, object propertyFatherIdValue, IDbConnection conn, IDbTransaction trans)
        {
            var dt = this.SelectTRecursion(selectProperties, whereSql, values, propertyIdName, propertyFatherIdName, propertyFatherIdValue, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        public IList<T> SelectRecursion(string[] selectProperties, string whereSql, object[] values, string propertyIdName, string propertyFatherIdName, object propertyFatherIdValue)
        {
            var dt = this.SelectTRecursion(selectProperties, whereSql, values, propertyIdName, propertyFatherIdName, propertyFatherIdValue);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }
        #endregion

        #region SelectT 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public abstract DataTable SelectT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <returns>DataTable</returns>
        public DataTable SelectT(string[] selectProperties, string whereSql, object[] values, bool isDistinct)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectT(selectProperties, whereSql, values, isDistinct, conn, null);
            }
        }

        /// <summary>
        /// 从存储过程得到查询
        /// </summary>
        /// <param name="proc">存储过程实体</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public DataTable SelectT(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            return OperationDB.ExecuteDataSet(proc, this.GetDataAdapter(), conn, trans).Tables[0];
        }
        public DataTable SelectT(BaseCmdProc proc)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return OperationDB.ExecuteDataSet(proc, this.GetDataAdapter(), conn, null).Tables[0];
            }
        }

        #endregion

        #region SelectTop 头N条查询
        /// <summary>
        /// 头N条查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="top">查询的条数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectTop(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int top, IDbConnection conn, IDbTransaction trans)
        {
            var dt = this.SelectTopT(selectProperties, whereSql, values, isDistinct, top, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }

        /// <summary>
        /// 头N条查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="top">查询的条数</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectTop(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int top)
        {
            var dt = this.SelectTopT(selectProperties, whereSql, values, isDistinct, top);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(dt);
        }
        #endregion

        #region SelectTopT 头N条查询
        /// <summary>
        /// 头N条查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="top">查询的条数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public abstract DataTable SelectTopT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int top, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 头N条查询
        /// </summary>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="top">查询的条数</param>
        /// <returns>DataTable</returns>
        public DataTable SelectTopT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int top)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectTopT(selectProperties, whereSql, values, isDistinct, top, conn, null);
            }
        }
        #endregion

        #region public SelectByValues
        /// <summary>
        /// 通过数组查询
        /// </summary>
        /// <typeparam name="A">数组类型</typeparam>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="filterName">查询数组的属性名称</param>
        /// <param name="filterValues">查询数组</param>
        /// <param name="exWhereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectByValues(string[] selectProperties, string filterName, IEnumerable filterValues, string exWhereSql, object[] exValues, IDbConnection conn, IDbTransaction trans)
        {
            var table = SelectByValuesT(selectProperties, filterName, filterValues, exWhereSql, exValues, conn, trans);
            return ModelConvertUtility.ConvertDataTableToModelList<T>(table);
        }

        /// <summary>
        /// 通过数组查询
        /// </summary>
        /// <typeparam name="A">数组类型</typeparam>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="filterName">查询数组的属性名称</param>
        /// <param name="filterValues">查询数组</param>
        /// <param name="exWhereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <returns>DataTable</returns>
        public IList<T> SelectByValues(string[] selectProperties, string filterName, IEnumerable filterValues, string exWhereSql, object[] exValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectByValues(selectProperties, filterName, filterValues, exWhereSql, exValues, conn, null);
            }
        }

        #endregion

        #region public SelectByValuesT
        /// <summary>
        /// 通过数组查询
        /// </summary>
        /// <typeparam name="A">数组类型</typeparam>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="filterName">查询数组的属性名称</param>
        /// <param name="filterValues">查询数组</param>
        /// <param name="exWhereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>DataTable</returns>
        public abstract DataTable SelectByValuesT(string[] selectProperties, string filterName, IEnumerable filterValues, string exWhereSql, object[] exValues, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 通过数组查询
        /// </summary>
        /// <typeparam name="A">数组类型</typeparam>
        /// <param name="selectProperties">查询属性</param>
        /// <param name="filterName">查询数组的属性名称</param>
        /// <param name="filterValues">查询数组</param>
        /// <param name="exWhereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <returns>DataTable</returns>
        public DataTable SelectByValuesT(string[] selectProperties, string filterName, IEnumerable filterValues, string exWhereSql, object[] exValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectByValuesT(selectProperties, filterName, filterValues, exWhereSql, exValues, conn, null);
            }
        }

        #endregion

        #region
        /// <summary>
        /// 获取实体,没有返回空
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="selectProperties">查询的字段</param>
        /// <returns></returns>
        public object Get(Dictionary<string, object> dictPropertyNamesValues, IDbConnection conn, IDbTransaction trans, params string[] selectProperties)
        {  
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < dictPropertyNamesValues.Keys.Count; i++)
            {
                sb.AppendFormat(@"#{0}=@{0}", dictPropertyNamesValues.Keys.ElementAt(i));
                if(i < dictPropertyNamesValues.Keys.Count - 1)
                {
                    sb.Append(" and ");
                }
            }
            string whereSql = sb.ToString();
            object[] values = dictPropertyNamesValues.Values.ToArray();
            var ls = this.Select(whereSql, values, false, conn, trans, selectProperties);
            if(ls.Count == 0)
            {
                return null;
            }
            return ls.First();
        }

        public object Get(Dictionary<string, object> dictPropertyNamesValues, params string[] selectProperties)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.Get(dictPropertyNamesValues, conn, null, selectProperties);
            }
        }


        #endregion

        #region GetAll
        /// <summary>
        /// 得到此对象的所有实体
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>此对象的所有实体</returns>
        public IList<T> GetAll(IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            return this.Select(helper.PropertyField_Dictionary.Keys.ToList().ToArray(), null, null, false, conn, trans);
        }

        /// <summary>
        /// 得到此对象的所有实体
        /// </summary>
        /// <returns>此对象的所有实体</returns>
        public IList<T> GetAll()
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.GetAll(conn, null);
            }
        }


        #endregion

        #region GetById
        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="ids">主键值</param>
        public object GetById(IDbConnection conn, IDbTransaction trans, params object[] ids)
        {
            if (ids == null)
            {
                return null;
            }
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                if(i != 0)
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", helper.PrimaryProperties[i]);
                ls.Add(ids[i]);

            }
           
            var t = this.Select(helper.PropertyField_Dictionary.Keys.ToList().ToArray(), sb.ToString(), ls.ToArray(), false, conn, trans);
            if (t.Count == 0)
            {
                return null;
            }
            return t[0];
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="ids">主键值</param>
        /// <returns>是否存在</returns>
        public object GetById(params object[] ids)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.GetById(conn, null, ids);
            }
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public object GetById(object id, IDbConnection conn, IDbTransaction trans, params string[] fields)
        {
            if (id == null)
            {
                return null;
            }
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            var t = this.Select(fields, whereSql, values, false, conn, trans);
            if (t.Count == 0)
            {
                return null;
            }
            return t[0];
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>是否存在</returns>
        public object GetById(object id, params string[] fields)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.GetById(id, conn, null, fields);
            }
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="ids">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public object GetById(object[] ids, IDbConnection conn, IDbTransaction trans, params string[] fields)
        {
            if (ids == null || ids.Length == 0)
            {
                return null;
            }
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", helper.PrimaryProperties[i]);
                ls.Add(ids[i]);

            }

            var t = this.Select(fields, sb.ToString(), ls.ToArray(), false, conn, trans);
            if (t.Count == 0)
            {
                return null;
            }
            return t[0];
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="ids">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>是否存在</returns>
        public object GetById(object[] ids, params string[] fields)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.GetById(ids, conn, null, fields);
            }
        }

        #endregion

        #region GetEntityById
        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public T GetEntityById(object id, IDbConnection conn, IDbTransaction trans)
        {
            return (T)GetById(id, conn, trans);
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>是否存在</returns>
        public T GetEntityById(object id)
        {
            return (T)GetById(id);
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public T GetEntityById(object id, IDbConnection conn, IDbTransaction trans, params string[] fields)
        {
            return (T)GetById(id, conn, trans, fields);
        }

        /// <summary>
        /// 通过通过Id得到实体,没有返回空
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>是否存在</returns>
        public T GetEntityById(object id, params string[] fields)
        {
            return (T)GetById(id, fields);
        }
        #endregion

        #region GetByProc
        public object GetByProc(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {

            var ds = OperationDB.ExecuteDataSet(proc, this.GetDataAdapter(), conn, trans);
            var arr = ModelConvertUtility.ConvertDataTableToModelList<T>(ds.Tables[0]);
            if (arr.Count > 0)
            {
                return arr[0];
            }
            else
            {
                return null;
            }
        }
        public object GetByProc(BaseCmdProc proc)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                var ds = OperationDB.ExecuteDataSet(proc, this.GetDataAdapter(), conn, null);
                var arr = ModelConvertUtility.ConvertDataTableToModelList<T>(ds.Tables[0]);
                if (arr.Count > 0)
                {
                    return arr[0];
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Exist
        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="ids">主键值</param>
        public bool IsExistById(IDbConnection conn, IDbTransaction trans, params object[] ids)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", helper.PrimaryProperties[i]);
                ls.Add(ids[i]);
            }


            if (ls.Count == 0)
            {
                return false;
            }

            return this.SelectCount(sb.ToString(), ls.ToArray(), null, conn, trans) > 0;
        }

        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="ids">主键值</param>
        /// <returns>是否存在</returns>
        public bool IsExistById(params object[] ids)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistById(conn, null, ids);
            }
        }

        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public bool IsExistById(object id, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            string whereSql = String.Format(@"#{0}=@{0}", helper.PrimaryProperties[0]);
            object[] values = { id };
            return this.SelectCount(whereSql, values, null, conn, trans) > 0;
        }

        /// <summary>
        /// 通过主键删除操作
        /// </summary>
        /// <param name="id">主键值</param>
        /// <param name="values">查询参数值</param>
        /// <returns>是否存在</returns>
        public bool IsExistById(object id)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistById(id, conn, null);
            }
        }

        /// <summary>
        /// 通过实体判断是否存在
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否存在</returns>
        public bool IsExist(T entity, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder();
            List<object> ls = new List<object>();
            for (int i = 0; i < helper.PrimaryProperties.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(@"#{0}=@{0}", helper.PrimaryProperties[i]);
                ls.Add(entity.GetType().GetProperty(helper.PrimaryProperties[i]).GetValue(entity, null));
            }

           
            if (ls.Count == 0)
            {
                return false;
            }

            return this.SelectCount(sb.ToString(), ls.ToArray(), null, conn, trans) > 0;
        }

        /// <summary>
        /// 通过实体判断是否存在
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否存在</returns>
        public bool IsExist(T entity)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExist(entity, conn, null);
            }
        }

       
        /// <summary>
        /// 判断有某值的字段是否存在
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public bool IsExistByField(string propertyName, object propertyValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistByField(propertyName, propertyValue, conn, null);
            }
        }

        /// <summary>
        /// 判断有某值的字段是否存在
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public bool IsExistByField(string propertyName, object propertyValue, IDbConnection conn, IDbTransaction trans)
        {
            string whereSql = String.Format(@"#{0}=@{0}", propertyName);
            object[] values = { propertyValue };
            return this.SelectCount(whereSql, values, null, conn, trans) > 0;
        }

        /// <summary>
        /// 判断有某值的字段是否存在（用于修改）
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <param name="notEqualPropertyName">不等于的属性</param>
        /// <param name="notEqualPropertyValue">不等于的属性值</param>
        /// <returns>是否存在</returns>
        public bool IsExistByField_NotEqualByField(string propertyName, object propertyValue, string notEqualPropertyName, object notEqualPropertyValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistByField_NotEqualByField(propertyName, propertyValue, notEqualPropertyName, notEqualPropertyValue, conn, null);
            }
        }


        /// <summary>
        /// 判断有某值的字段是否存在（用于修改）
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="propertyValue">值</param>
        /// <param name="notEqualPropertyName">不等于的属性</param>
        /// <param name="notEqualPropertyValue">不等于的属性值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public bool IsExistByField_NotEqualByField(string propertyName, object propertyValue, string notEqualPropertyName, object notEqualPropertyValue, IDbConnection conn, IDbTransaction trans)
        {
            string whereSql = String.Format(@"#{0}=@{0} and #{1} <> @{1}", propertyName, notEqualPropertyName);
            object[] values = { propertyValue, notEqualPropertyValue };
            return this.SelectCount(whereSql, values, null, conn, trans) > 0;
        }


        /// <summary>
        /// 判断有某些值的字段是否存在（用于修改）
        /// </summary>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <param name="notEqualPropertyName">不等于的属性</param>
        /// <param name="notEqualPropertyValue">不等于的属性值</param>
        /// <returns>是否存在</returns>
        public bool IsExistByFields_NotEqualByField(Dictionary<string, object> dictPropertyNamesValues, string notEqualPropertyName, object notEqualPropertyValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistByFields_NotEqualByField(dictPropertyNamesValues, notEqualPropertyName, notEqualPropertyValue, conn, null);
            }
        }


        /// <summary>
        /// 判断有某些值的字段是否存在（用于修改）
        /// </summary>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <param name="notEqualPropertyName">不等于的属性</param>
        /// <param name="notEqualPropertyValue">不等于的属性值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public bool IsExistByFields_NotEqualByField(Dictionary<string, object> dictPropertyNamesValues, string notEqualPropertyName, object notEqualPropertyValue, IDbConnection conn, IDbTransaction trans)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dictPropertyNamesValues.Keys.Count; i++)
            {
                sb.AppendFormat(@"#{0}=@{0}", dictPropertyNamesValues.Keys.ElementAt(i));
                sb.Append(" and ");
            }
            sb.AppendFormat(@"#{0} <> @{0}", notEqualPropertyName);
            var ls = dictPropertyNamesValues.Values.ToList();
            ls.Add(notEqualPropertyValue);
            return this.SelectCount(sb.ToString(), ls.ToArray(), null, conn, trans) > 0;
        }

      

        /// <summary>
        /// 判断有某些值的字段是否存在
        /// </summary>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <returns>是否存在</returns>
        public bool IsExistByFields(Dictionary<string, object> dictPropertyNamesValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.IsExistByFields(dictPropertyNamesValues, conn, null);
            }
        }

        /// <summary>
        /// 判断有某些值的字段是否存在
        /// </summary>
        /// <param name="dictPropertyNamesValues">属性值字典</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public bool IsExistByFields(Dictionary<string, object> dictPropertyNamesValues, IDbConnection conn, IDbTransaction trans)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < dictPropertyNamesValues.Keys.Count; i++)
            {
                sb.AppendFormat(@"#{0}=@{0}", dictPropertyNamesValues.Keys.ElementAt(i));
                if(i < dictPropertyNamesValues.Keys.Count - 1)
                {
                    sb.Append(" and ");
                }
            }
            string whereSql = sb.ToString();
            object[] values = dictPropertyNamesValues.Values.ToArray();
            return this.SelectCount(whereSql, values, null, conn, trans) > 0;
        }



       
        #endregion

        #region public SelectCount
        /// <summary>
        /// 得到记录数查询
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="distinctPropertys">Distinct属性，空为不distinct</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>记录数</returns>
        public abstract int SelectCount(string whereSql, object[] values, string[] selectProperties, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到记录数查询
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="distinctPropertys">Distinct属性，空为不distinct</param>
        /// <returns>记录数</returns>
        public int SelectCount(string whereSql, object[] values, string[] selectProperties)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectCount(whereSql, values, selectProperties, conn, null);
            }
        }
        #endregion

        #region SelectCountByField
       
        public  int SelectCountByField(string propertyName, object propertyValue, IDbConnection conn, IDbTransaction trans)
        {
            string where = string.Format(@"#{0}=@{0}", propertyName);
            object[] values = new object[]{ propertyValue };
            return SelectCount(where, values, null, conn, trans);
        }

        /// <summary>
        /// 根据字段得到记录数查询
        /// </summary>
        /// <param name="propertyName">查询字段</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int SelectCountByField(string propertyName, object propertyValue)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectCountByField(propertyName, propertyValue, conn, null);
            }
        }
        #endregion

        #region SelectStatistic
        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public int SelectStatisticInt(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            return Util.ObjectConvert.GetIntValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, trans), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public int SelectStatisticInt(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            return Util.ObjectConvert.GetIntValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty), 0);
        }


        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public long SelectStatisticLong(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            return Util.ObjectConvert.GetLongValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, trans), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public long SelectStatisticLong(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            return Util.ObjectConvert.GetLongValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public float SelectStatisticFloat(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            return Util.ObjectConvert.GetFloatValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, trans), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public float SelectStatisticFloat(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            return Util.ObjectConvert.GetFloatValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public decimal SelectStatisticDecimal(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            return Util.ObjectConvert.GetDecimalValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, trans), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public decimal SelectStatisticDecimal(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            return Util.ObjectConvert.GetDecimalValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public double SelectStatisticDouble(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            return Util.ObjectConvert.GetDoubleValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, trans), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public double SelectStatisticDouble(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            return Util.ObjectConvert.GetDoubleValue(SelectStatisticObject(whereSql, values, statisticType, statisticProperty), 0);
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计字段</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计数</returns>
        public abstract object SelectStatisticObject(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <returns>统计数</returns>
        public object SelectStatisticObject(string whereSql, object[] values, StatisticType statisticType, string statisticProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectStatisticObject(whereSql, values, statisticType, statisticProperty, conn, null);
            }
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计的别名</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">统计属性</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public abstract DataTable SelectSplitStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计的别名</param>
        /// <param name="statisticProperty">分组属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public DataTable SelectSplitStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectSplitStatistic(selectProperties, whereSql, values, statisticType, statisticTypeFieldName, statisticProperty, groupProperty, pageIndex, pageSize, out  pageCount, out recordCount, conn, null);
            }
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计的别名</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">统计属性</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public abstract DataTable SelectSplitStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计的别名</param>
        /// <param name="statisticProperty">分组属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public DataTable SelectSplitStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectSplitStatistic(whereSql, values, statisticType, statisticTypeFieldName, statisticProperty, groupProperty, pageIndex, pageSize, out  pageCount, out recordCount, conn, null);
            }
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public abstract DataTable SelectStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="sumProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public DataTable SelectStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectStatistic(whereSql, values, statisticType, statisticProperty, groupProperty, conn, null);
            }
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计的别名</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField, 其他字段 列组成的table）</returns>
        public abstract DataTable SelectStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField, 其他字段 列组成的table）</returns>
        public DataTable SelectStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectStatistic(selectProperties, whereSql, values, statisticType, statisticTypeFieldName, statisticProperty, groupProperty, conn, null);
            }
        }


        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public abstract DataTable SelectStatisticTop(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty, int topNum, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <returns>统计结果(statisticField, groupField 列组成的table）</returns>
        public DataTable SelectStatisticTop(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty, int topNum)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectStatisticTop(whereSql, values, statisticType, statisticProperty, groupProperty, topNum, conn, null);
            }
        }

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticTypeFieldName">统计字段的名称</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="topNum">头几条</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField, 其他字段 列组成的table）</returns>
        public abstract DataTable SelectStatisticTop(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int topNum, IDbConnection conn, IDbTransaction trans);

        /// <summary>
        /// 得到统计
        /// </summary>
        /// <param name="selectProperties">查询的字段</param>
        /// <param name="whereSql">查询条件（在写查询语句时，属性名前加#, 参数名前加@， 语句按sql标准写 如：#property1=@param1 and #property2=@param2, 又如：#property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="values">查询参数值</param>
        /// <param name="statisticType">统计枚举</param>
        /// <param name="statisticProperty">统计属性</param>
        /// <param name="groupProperty">分组属性</param>
        /// <param name="topNum">头几条</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>统计结果(statisticField, groupField, 其他字段 列组成的table）</returns>
        public DataTable SelectStatisticTop(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int topNum)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.SelectStatisticTop(selectProperties, whereSql, values, statisticType, statisticTypeFieldName, statisticProperty, groupProperty, topNum, conn, null);
            }
        }
        #endregion

        #region Execute



        /// <summary>
        /// 执行propertySql操作并且返回执行影响的行数
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>执行影响的行数</returns>
        public virtual int ExecuteNonQuery(string propertySql, object[] exValues, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string sql = this.ReplaceSql(propertySql, exValues, out parameterNames, out parameterValues);
            return OperationDB.ExecuteNonQuery(sql, parameterNames, parameterValues, conn, trans);
        }

        /// <summary>
        /// 执行propertySql操作并且返回执行影响的行数
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <returns>执行影响的行数</returns>
        public virtual int ExecuteNonQuery(string propertySql, object[] exValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.ExecuteNonQuery(propertySql, exValues, conn, null);
            }
        }

        public int ExecuteNonQuery(BaseCmdProc proc)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(proc, conn, null);
            }
        }

        public virtual int ExecuteNonQuery(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            return OperationDB.ExecuteNonQuery(proc, conn, trans);
        }

        /// <summary>
        /// 执行propertySql操作并且返回第一行第一列的值
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>第一行第一列的值</returns>
        public virtual object ExecuteScalar(string propertySql, object[] exValues, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string sql = this.ReplaceSql(propertySql, exValues, out parameterNames, out parameterValues);
            return OperationDB.ExecuteScalar(sql, parameterNames, parameterValues, conn, trans);
        }

        /// <summary>
        /// 执行propertySql操作并且返回第一行第一列的值
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <returns>第一行第一列的值</returns>
        public object ExecuteScalar(string propertySql, object[] exValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.ExecuteScalar(propertySql, exValues, conn, null);
            }
        }

        public object ExecuteScalar(BaseCmdProc proc)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return ExecuteScalar(proc, conn, null);
            }
        }

        public virtual object ExecuteScalar(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            return OperationDB.ExecuteNonQuery(proc, conn, trans);
        }

        /// <summary>
        /// 执行propertySql操作并且返回DataTable
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>DataTable</returns>
        public virtual object ExecuteDataTable(string propertySql, object[] exValues, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string sql = this.ReplaceSql(propertySql, exValues, out parameterNames, out parameterValues);
            return OperationDB.ExecuteDataSet(sql, parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
        }

        /// <summary>
        /// 执行存储过程操作并且返回DataTable
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>DataTable</returns>
        public virtual object ExecuteDataTable(BaseCmdProc proc, IDbConnection conn, IDbTransaction trans)
        {
            return OperationDB.ExecuteDataSet(proc, this.GetDataAdapter(), conn, trans).Tables[0];
        }

        /// <summary>
        /// 执行存储过程操作并且返回DataTable
        /// </summary>
        /// <param name="proc">存储过程对象</param>
        /// <returns>DataTable</returns>
        public object ExecuteDataTable(BaseCmdProc proc)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.ExecuteDataTable(proc, conn, null);
            }
        }

        /// <summary>
        /// 执行propertySql操作并且返回DataTable
        /// </summary>
        /// <param name="propertySql">实体语句（在写实体语句时，属性名前加#, 参数名前加@, 实体名前加~ 语句按sql标准写 如：delete from $Table where #property1=@param1 and #property2=@param2 order by #property2 desc）</param>
        /// <param name="exValues">查询参数值</param>
        /// <returns>DataTable</returns>
        public object ExecuteDataTable(string propertySql, object[] exValues)
        {
            using (IDbConnection conn = this.CreateConnection())
            {
                return this.ExecuteDataTable(propertySql, exValues, conn, null);
            }
        }
        #endregion

        #region public virtual
        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection CreateConnection()
        {
            return OperationBase.GetConnection(ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).DataAccessString);
        }



        #endregion

        #region protected
        protected OperationBase OperationDB;

        /// <summary>
        /// 得到查询的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Properties">对应查询字段的属性数组</param>
        /// <returns>查询的字段(如：field1, field2, field3)</returns>
        protected virtual string GetSelectFields(string[] Properties)
        {
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            List<string> list = MappingUtility.GetFields<T>(Properties);
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(String.Format(@"{0}", list[i]));
                if (i < list.Count - 1)
                {
                    sb.Append(',');
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 得到查询的字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Properties">对应查询字段的属性数组</param>
        /// <param name="aliasTable">类似于A.Field的 A</param>
        /// <returns>查询的字段(如：field1, field2, field3)</returns>
        protected virtual string GetSelectFields(string[] Properties, string aliasTable)
        {
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            List<string> list = MappingUtility.GetFields<T>(Properties);
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(String.Format(@"{0}.{1}", aliasTable, list[i]));
                if (i < list.Count - 1)
                {
                    sb.Append(',');
                }
            }
            return sb.ToString();
        }


        protected virtual string GetParamFormat(string parameterName)
        {
            return "@" + parameterName;
        }

        /// <summary>
        /// 处理Where参数
        /// </summary>
        /// <param name="paramNames">参数列表</param>
        /// <param name="where">条件</param>
        /// <returns>处理后条件</returns>
        protected virtual string GetWhereFormat(IList<string> paramNames, string where)
        {
            return where;
        }

        protected string GetStatisticFunName(StatisticType type)
        {
            if (type == StatisticType.Avg)
            {
                return "Avg";
            }
            if (type == StatisticType.Sum)
            {
                return "Sum";
            }
            if (type == StatisticType.Max)
            {
                return "Max";
            }
            if (type == StatisticType.Count)
            {
                return "Count";
            }
            return "Min";
        }

        #region ReplaceSql
        /// <summary>
        /// Sql替换
        /// </summary>
        /// <param name="propertySql">要替换的Sql</param>
        /// <param name="values">查询参数值</param>
        /// <param name="parameterNames">参数名列表</param>
        /// <param name="parameterValues">参数值列表</param>
        /// <returns>执行的sql</returns>
        protected string ReplaceSql(string propertySql, object[] values, out List<string> parameterNames, out List<object> parameterValues)
        {
            CompareHelper<string> compareHelp = new CompareHelper<string>(StringHelper.CompareIgnoreCase);
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            Dictionary<string, string> useCompareKeyReplaceValue = new Dictionary<string, string>();
            useCompareKeyReplaceValue.Add(helper.EntityName, String.Format(@"[{0}]", helper.TableName));
            propertySql = MappingUtility.reg_GetTableListFromSql.ReplaceByArray(MappingUtility.PropertyName, propertySql, useCompareKeyReplaceValue, compareHelp);

            useCompareKeyReplaceValue = MappingUtility.GetPropertiesFields<T>(MappingUtility.GetPropListFromSql(propertySql).ToArray());

            for (int i = 0; i < useCompareKeyReplaceValue.Count; i++)
            {
                string key = useCompareKeyReplaceValue.Values.ElementAt(i);
                string value = useCompareKeyReplaceValue.Values.ElementAt(i);
                useCompareKeyReplaceValue[key] = "[" + value + "]";
            }
            propertySql = MappingUtility.reg_GetPropListFromSql.ReplaceByArray(MappingUtility.PropertyName, propertySql, useCompareKeyReplaceValue, compareHelp);

            parameterValues = values == null ? new List<object>() : values.ToList();
            parameterNames = MappingUtility.GetParamListFromSql(propertySql);

            return propertySql;
        }
        #endregion
        #endregion

        #region public static GetMapping
        /// <summary>
        /// 得到 Mapping 对象
        /// </summary>
        /// <returns>Mapping对象</returns>
        public static OperationBase GetOperation(string dataAccessName)
        {
            return DbConnectionHelper.GetOperation(dataAccessName);

        }

        /// <summary>
        /// 得到 Mapping 对象
        /// </summary>
        /// <returns>Mapping对象</returns>
        public static MappingBase<T> GetMapping()
        {
            return GetMapping(ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).DataAccessString);

        }

        /// <summary>
        /// 得到 Mapping 对象
        /// </summary>
        /// <returns>Mapping对象</returns>
        public static MappingBase<T> GetMapping(IDbConnection conn)
        {
            if (conn is SqlConnection)
            {
                return new MappingSqlServer<T>();
            }
            else if (conn is MySqlConnection)
            {
                return new MappingMySql<T>();
            }
            else if (conn.GetType().FullName == "System.Data.SQLite.SQLiteConnection")
            {
                return new MappingSqlite<T>();
            }
            else if (conn is OracleConnection)
            {
                return new MappingOracle<T>();
            }
            else// if (conn is OleDbConnection)
            {
                return new MappingOleDb<T>();
            }
        }

        /// <summary>
        /// 得到 Mapping 对象
        /// </summary>
        /// <returns>Mapping对象</returns>
        public static MappingBase<T> GetMapping(string dataAccessString)
        {
            if (!mappingDic.ContainsKey(dataAccessString))
            {
                lock (lockObj)
                {
                    if (!mappingDic.ContainsKey(dataAccessString))
                    {
                        var dbtype = OperationBase.GetDatabaseType(dataAccessString);

                        if (dbtype == DatabaseType.SqlServer)
                        {
                            mappingDic.Add(dataAccessString, new MappingSqlServer<T>());
                        }
                        else if (dbtype == DatabaseType.MySql)
                        {
                            mappingDic.Add(dataAccessString, new MappingMySql<T>());
                        }
                        else if (dbtype == DatabaseType.Sqlite)
                        {
                            mappingDic.Add(dataAccessString, new MappingSqlite<T>());
                        }
                        else if (dbtype == DatabaseType.Oracle)
                        {
                            mappingDic.Add(dataAccessString, new MappingOracle<T>());
                        }
                        else// if (providerName == "System.Data.OleDb")
                        {
                            mappingDic.Add(dataAccessString, new MappingOleDb<T>());
                        }
                    }
                }
            }
            return mappingDic[dataAccessString];
        }

        private static object lockObj = new object();
        private static Dictionary<string, MappingBase<T>> mappingDic = new Dictionary<string, MappingBase<T>>();


        #endregion

        public IDbDataAdapter GetDataAdapter()
        {
            return this.OperationDB.GetDataAdapter();

        }

        public IDbConnection CreateConnection(string connectionString)
        {
            return this.OperationDB.CreateConnection(connectionString);

        }

    }
}
