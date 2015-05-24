using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using BM.Util;
using System.Data.SqlClient;
using System.Configuration;


namespace BM.DA
{
    public sealed class MappingSqlServer<T> : MappingBase<T> where T : IModel,new()
    {
        private static bool? isSqlserverHight;

        public static bool IsSqlserverHight
        {
            get
            {
                if (isSqlserverHight != null)
                {
                    return isSqlserverHight.Value;
                }
                else
                {
                    var val = ConfigurationManager.AppSettings["IsSqlServerHight"];
                    if (val == null)
                    {
                        isSqlserverHight = true;
                    }
                    else
                    {
                        return val == "1";
                    }
                    return isSqlserverHight.Value;
                }

            }
        }

        public MappingSqlServer()
        {
            this.OperationDB = new OperationSqlServer();
        }

        #region insert

        protected override int Insert(List<string> fields, List<object> fieldValues, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.AppendFormat(@"insert into [{0}] (", helper.TableName);

            for (int i = 0; i < fields.Count; i++)
            {
                sb.AppendFormat(@"[{0}]", fields[i]);
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
            }
           
            sb.Append(") values(");
            for (int i = 0; i < fields.Count; i++)
            {
                sb.Append(this.GetParamFormat(fields[i]));
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append(");");

            return OperationDB.ExecuteNonQuery(sb.ToString(), fields, fieldValues, conn, trans);
            
        }


        protected override object InsertReturnIdentity(List<string> fields, List<object> fieldValues, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.AppendFormat(@"insert into [{0}] (", helper.TableName);

            for (int i = 0; i < fields.Count; i++)
            {
                sb.AppendFormat(@"[{0}]", fields[i]);
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
            }

            sb.Append(") values(");
            for (int i = 0; i < fields.Count; i++)
            {
                sb.Append(this.GetParamFormat(fields[i]));
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append(") select @@Identity as r_i_v_x;");

            return OperationDB.ExecuteScalar(sb.ToString(), fields, fieldValues, conn, trans);
        }

        #endregion

        #region update

        protected override int Update(List<string> fields, List<object> fieldValues, object[] values, string whereSql, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.AppendFormat(@"Update [{0}] Set ", helper.TableName);
            for (int i = 0; i < fields.Count; i++)
            {
                if (fieldValues[i] != null)
                {
                    string paramName = MappingUtility.GetParamName(parameterNames, fields[i]);
                    sb.AppendFormat(@"[{0}]={1}", fields[i], this.GetParamFormat(paramName));
                    parameterNames.Add(paramName);
                    parameterValues.Add(fieldValues[i]);
                }
                else
                {
                    sb.AppendFormat(@"[{0}]=null", fields[i]);
                }
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
            }

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0};", where));
            }

            return OperationDB.ExecuteNonQuery(sb.ToString(), parameterNames, parameterValues, conn, trans);
        }

       

        #endregion

        public override int Delete(string whereSql, object[] values, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.Append(String.Format(@"delete from [{0}] ", helper.TableName));
            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0};", where));
            }

            return OperationDB.ExecuteNonQuery(sb.ToString(), parameterNames, parameterValues, conn, trans);
        }

        private  DataTable SelectSplitT_0(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            ModelDescriptionHelper helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (selectProperties == null || selectProperties.Length == 0)
            {
                selectProperties = helper.PropertyField_Dictionary.Keys.ToArray();
            }


            var distinctPropertys = isDistinct ? selectProperties : null;
            recordCount = this.SelectCount(whereSql, values, distinctPropertys, conn, trans);
            pageCount = SplitPageHelper.GetPageCount(pageSize, recordCount);
            if (recordCount == 0)
            {
                return null;
            }


            string fields = GetSelectFields(selectProperties);
            
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            int topAll = SplitPageHelper.GetTopNum(pageIndex, pageSize, recordCount);
            StringBuilder sb = new StringBuilder();
            if (isDistinct)
            {
                sb.Append(String.Format(@"Select distinct top {0} {1} from {2}", topAll, fields, helper.TableName));

            }
            else
            {
                sb.Append(String.Format(@"Select top {0} {1} from {2} ", topAll, fields, helper.TableName));

            }

            if (!String.IsNullOrEmpty(where))
            {
                sb.Append(" where ");
                sb.Append(where);
            }
            
            List<string> orderfields;
            List<string> ascDescs;
            
            order = SplitPageHelper.SplitSortFields(order, out orderfields, out ascDescs, helper.PrimaryFields);
            sb.Append(" order by ");
            sb.Append(order);

            string sql = GetFinalSql(sb.ToString(), orderfields, ascDescs, pageSize, recordCount, pageIndex);

            DataSet ds = OperationDB.ExecuteDataSet(sql, parameterNames, parameterValues, this.GetDataAdapter(), conn, trans);
            return ds.Tables[0];
        }

        private DataTable SelectSplitT_1(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            ModelDescriptionHelper helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (selectProperties == null || selectProperties.Length == 0)
            {
                selectProperties = helper.PropertyField_Dictionary.Keys.ToArray();
            }


            var distinctPropertys = isDistinct ? selectProperties : null;
            recordCount = this.SelectCount(whereSql, values, distinctPropertys, conn, trans);
            pageCount = SplitPageHelper.GetPageCount(pageSize, recordCount);
            if (recordCount == 0)
            {
                return null;
            }


            string fields = GetSelectFields(selectProperties);
           

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            List<string> orderfields;
            List<string> ascDescs;
            order = SplitPageHelper.SplitSortFields(order, out orderfields, out ascDescs, helper.PrimaryFields);
           
            if (String.IsNullOrEmpty(where))
            {
                where = " 1=1 ";
            }

            string tempTableName = "temp_table_12543dwz";
            string rownumber = "rownumber" + tempTableName;
          

            //设置开始行号
            int startRow = (pageIndex - 1) * pageSize + 1;
            //设置结束行号
            int endRow = startRow + pageSize - 1;
            string sqlTempWhere = string.Format("{0} between {1} and {2}", rownumber, startRow, endRow);


            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            sb.AppendLine(String.Format(@"With {0}({1}, {2}) as (", tempTableName, rownumber, fields));
            sb.AppendLine(String.Format(@"select ROW_NUMBER() over(order by {0}) as {1}, {2} from [{3}] where {4})", order, rownumber, fields, helper.TableName, where));
            sb.AppendLine(String.Format(@"select {0} from {1} where {2}", fields, tempTableName, sqlTempWhere));

            DataSet ds = OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans);
            return ds.Tables[0];
        }


        public override DataTable SelectSplitT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            if (IsSqlserverHight)
            {
               return SelectSplitT_1(selectProperties, whereSql, values, isDistinct, pageIndex, pageSize, out  pageCount, out  recordCount, conn, trans);
            }
            else
            {
              return  SelectSplitT_0(selectProperties, whereSql, values, isDistinct, pageIndex, pageSize, out  pageCount, out  recordCount, conn, trans);
            }
        }
      
        public override DataTable SelectT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, IDbConnection conn, IDbTransaction trans)
        {
            ModelDescriptionHelper helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (selectProperties == null || selectProperties.Length == 0)
            {
                selectProperties = helper.PropertyField_Dictionary.Keys.ToArray();
            }

            string fields = GetSelectFields(selectProperties);

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

          

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            if (isDistinct)
            {
                sb.Append(String.Format(@"select distinct {0} from [{1}]", fields, helper.TableName));
            }
            else
            {
                sb.Append(String.Format(@"select {0} from [{1}]", fields, helper.TableName));
            }
            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0}", where));
            }
            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
        }

        public override DataTable SelectTopT(string[] selectProperties, string whereSql, object[] values, bool isDistinct, int top, IDbConnection conn, IDbTransaction trans)
        {
            ModelDescriptionHelper helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (selectProperties == null || selectProperties.Length == 0)
            {
                selectProperties = helper.PropertyField_Dictionary.Keys.ToArray();
            }

            string fields = GetSelectFields(selectProperties);

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

          
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            if (isDistinct)
            {
                sb.Append(String.Format(@"select distinct top {0} {1} from [{2}]", top, fields, helper.TableName));
            }
            else
            {
                sb.Append(String.Format(@"select top {0} {1} from [{2}]", top, fields, helper.TableName));
            }
            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0}", where));
            }
            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
        }

        public override DataTable SelectByValuesT(string[] selectProperties, string filterName, IEnumerable filterValues, string exWhereSql, object[] exValues, IDbConnection conn, IDbTransaction trans)
        {
            ModelDescriptionHelper helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            if (selectProperties == null || selectProperties.Length == 0)
            {
                selectProperties = helper.PropertyField_Dictionary.Keys.ToArray();
            }

            string fields = GetSelectFields(selectProperties);

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(exWhereSql, exValues, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

           

            filterName = MappingUtility.GetField<T>(filterName);

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            StringBuilder sbWhere = new StringBuilder(MappingUtility.SqlLength);

            int i = 0;
            foreach (object value in filterValues)
            {
                string paramName = MappingUtility.GetParamName(parameterNames, String.Format(@"{0}{1}", filterName, i));
                parameterNames.Add(paramName);
                parameterValues.Add(value);
                if (i > 0)
                {
                    sbWhere.Append("or");
                }
                sbWhere.AppendFormat(@" {0} = {1} ", filterName, this.GetParamFormat(paramName));
                i++;
            }

            string sqlWhere;
            if (String.IsNullOrEmpty(where))
            {
                sqlWhere = "1=1";
            }
            else
            {
                sqlWhere = where;
            }
            if (sbWhere.ToString() != "")
            {
                sqlWhere = String.Format(@"({0}) and ({1})", sqlWhere, sbWhere.ToString());
            }


            sb.Append(string.Format(@"select {0} from [{1}] where {2}", fields, helper.TableName, sqlWhere));

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            return  OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
            
        }

        public override int AddNum(string whereSql, object[] values, string[] addProperties, object[] nums, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            List<string> fields;
            List<object> fieldValues;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int i = 0; i < addProperties.Length; i++)
            {
                dic.Add(addProperties[i], nums[i]);
            }
            MappingUtility.GetFieldsValues<T>(dic, out fields, out fieldValues, true);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.AppendFormat(@"Update [{0}] Set ", helper.TableName);
            for (int i = 0; i < fields.Count; i++)
            {
                string paramName = MappingUtility.GetParamName(parameterNames, fields[i]);
                sb.AppendFormat(@"[{0}]=[{0}] + {1}", fields[i], this.GetParamFormat(paramName));
                if (i < fields.Count - 1)
                {
                    sb.Append(',');
                }
                parameterNames.Add(paramName);
                parameterValues.Add(fieldValues[i]);
            }

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0};", where));
            }

            return OperationDB.ExecuteNonQuery(sb.ToString(), parameterNames, parameterValues, conn, trans);
        }

      

        public override object SelectStatisticObject(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];

            sb.Append(String.Format(@"select {0}([{1}]) from [{2}] ", this.GetStatisticFunName(statisticType), statisticField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            object value = OperationDB.ExecuteScalar(sb.ToString(), parameterNames, parameterValues, conn, trans);
            if (value is DBNull || value == null)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        public override DataTable SelectStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];

            sb.Append(String.Format(@"select {0}([{1}]), [{2}] from [{3}] ", this.GetStatisticFunName(statisticType), statisticField, groupField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by [{0}]", groupField));

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
        }

        public override DataTable SelectStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];
            if (selectProperties != null)
            {
                selectProperties = (from x in selectProperties where x != groupProperty select x).ToArray();
            }
            string fields = selectProperties == null || selectProperties.Count() == 0 ? "" : "," + GetSelectFields(selectProperties, "B");

            sb.Append(String.Format(@"select * from (select distinct A.* {0} from (select {1}({2}) as {3}, {4} from {5} ", fields, this.GetStatisticFunName(statisticType), statisticField, statisticTypeFieldName, groupField,  helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by {0}", groupField));

            sb.AppendFormat(@") as A join {0} B on A.{1}=B.{1}) as C", helper.TableName, groupField);

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }


            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];

        }

        public override DataTable SelectSplitStatistic(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];
            if (selectProperties != null)
            {
                selectProperties = (from x in selectProperties where x != groupProperty select x).ToArray();
            }
            string fields = selectProperties == null || selectProperties.Count() == 0 ? "" : "," + GetSelectFields(selectProperties, "B");

            sb.Append(String.Format(@"select *   from (select distinct A.* {0} from (select {1}({2}) as {3}, {4} from {5} ", fields, this.GetStatisticFunName(statisticType), statisticField, statisticTypeFieldName, groupField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by {0}", groupField));

            sb.AppendFormat(@") as A join {0} B on A.{1}=B.{1}) as C", helper.TableName, groupField);

            string sqlCount = String.Format(@"select count(*) from ({0}) as T", sb.ToString());

            recordCount = Util.ObjectConvert.GetIntValue(OperationDB.ExecuteScalar(sqlCount, parameterNames, parameterValues, conn, trans));
            pageCount = SplitPageHelper.GetPageCount(pageSize, recordCount);
            if (recordCount == 0)
            {
                return null;
            }



            List<string> orderfields;
            List<string> ascDescs;
            if (String.IsNullOrEmpty(order))
            {
                order = groupField + " desc";
            }
            order = SplitPageHelper.SplitSortFields(order, out orderfields, out ascDescs, new string[]{groupField});

            sb.AppendFormat(@" order by {0}", order);


            int topAll = SplitPageHelper.GetTopNum(pageIndex, pageSize, recordCount);
            string sql = sb.ToString();
            sql = "select top " + topAll + " * " + sql.Substring(10);

            sql = GetFinalSql(sql, orderfields, ascDescs, pageSize, recordCount, pageIndex);


            DataSet ds = OperationDB.ExecuteDataSet(sql, parameterNames, parameterValues, this.GetDataAdapter(), conn, trans);
            return ds.Tables[0];
        }

        public override DataTable SelectSplitStatistic(string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int pageIndex, int pageSize, out int pageCount, out int recordCount, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];

            sb.Append(String.Format(@"select {0}({1}) as {2}, {3} from {4} ", this.GetStatisticFunName(statisticType), statisticField, statisticTypeFieldName, groupField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by {0}", groupField));


            string sqlCount = String.Format(@"select count(*) from ({0}) as T", sb.ToString());

            recordCount = Util.ObjectConvert.GetIntValue(OperationDB.ExecuteScalar(sqlCount, parameterNames, parameterValues, conn, trans));
            pageCount = SplitPageHelper.GetPageCount(pageSize, recordCount);
            if (recordCount == 0)
            {
                return null;
            }


            List<string> orderfields;
            List<string> ascDescs;
            if (String.IsNullOrEmpty(order))
            {
                order = groupField + " desc";
            }
            order = SplitPageHelper.SplitSortFields(order, out orderfields, out ascDescs, new string[]{groupField});


            string sql = String.Format(@"select * from ({0}) order by {1}", sb.ToString(), order);

           

            sql = GetFinalSql(sb.ToString(), orderfields, ascDescs, pageSize, recordCount, pageIndex);


            DataSet ds = OperationDB.ExecuteDataSet(sql, parameterNames, parameterValues, this.GetDataAdapter(), conn, trans);
            return ds.Tables[0];

        }
       
        public override DataTable SelectStatisticTop(string whereSql, object[] values, StatisticType statisticType, string statisticProperty, string groupProperty, int topNum, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];

            sb.Append(String.Format(@"select top {0} {1}({2}), {3} from {4} ", topNum, this.GetStatisticFunName(statisticType), statisticField, groupField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by [{0}]", groupField));

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }

            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];

        }


        public override DataTable SelectStatisticTop(string[] selectProperties, string whereSql, object[] values, StatisticType statisticType, string statisticTypeFieldName, string statisticProperty, string groupProperty, int topNum, IDbConnection conn, IDbTransaction trans)
        {

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));

            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            string statisticField = helper.PropertyField_Dictionary[statisticProperty];
            string groupField = helper.PropertyField_Dictionary[groupProperty];
            if (selectProperties != null)
            {
                selectProperties = (from x in selectProperties where x != groupProperty select x).ToArray();
            }
            string fields = selectProperties == null || selectProperties.Count() == 0 ? "" : "," + GetSelectFields(selectProperties, "B");

            sb.Append(String.Format(@"select top {0} * from (select distinct A.* {1} from (select {2}({3}) as {4}, {5} from {6} ", topNum, fields, this.GetStatisticFunName(statisticType), statisticField, statisticTypeFieldName, groupField, helper.TableName));

            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0} ", where));
            }


            sb.Append(String.Format(@" group by {0}", groupField));

            sb.AppendFormat(@") as A join {0} B on A.{1}=B.{1}) as C", helper.TableName, groupField);

            if (String.IsNullOrEmpty(order) == false)
            {
                sb.Append(String.Format(@" order by {0}", order));
            }


            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];

        }


        public override int SelectCount(string whereSql, object[] values, string[] selectProperties, IDbConnection conn, IDbTransaction trans)
        {
            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);
            if (selectProperties != null && selectProperties.Count() > 0)
            {
                string fields = GetSelectFields(selectProperties);

                sb.Append(String.Format(@"select count(distinct {0}) from {1}", fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0], helper.TableName));
            }
            else
            {
                sb.Append(String.Format(@"select count(*) from [{0}]", helper.TableName));
            }
            if (String.IsNullOrEmpty(where) == false)
            {
                sb.Append(String.Format(@" where {0}", where));
            }

            object value = OperationDB.ExecuteScalar(sb.ToString(), parameterNames, parameterValues, conn, trans);
            if (value is DBNull || value == null)
            {
                return 0;
            }
            else
            {
                return (int)value;
            }
        }


        #region 分页时得到最终的降序或升序的sql GetFinalSql
        private  string GetFinalSql(string sql, List<string> orderField, List<string> ascdesc, int pageSize, int count, int pageIndex)
        {
            int topNum = pageIndex * pageSize;
            if (count - topNum < 0)
            {
                topNum = pageSize - topNum + count;
                if (topNum < pageSize)
                {
                    pageSize = topNum;
                }
            }
            string order1 = "";
            for (int i = 0; i < orderField.Count; i++)
            {
                if (i > 0)
                {
                    order1 += ",";
                }
                if (ascdesc[i].ToUpper() == "ASC")
                {
                    order1 += orderField[i] + " Desc";
                }
                else
                {
                    order1 += orderField[i] + " Asc";
                }
            }

            string order2 = "";
            for (int i = 0; i < orderField.Count; i++)
            {
                if (i > 0)
                {
                    order2 += ",";
                }
                if (ascdesc[i].ToUpper() == "ASC")
                {
                    order2 += orderField[i] + " Asc";
                }
                else
                {
                    order2 += orderField[i] + " Desc";
                }
            }

            if (pageSize < 0)
                pageSize = 0;

            return "SELECT * FROM (SELECT TOP " + pageSize + " * FROM (" + sql + ") AS T ORDER BY " + order1 + ") AS T1 ORDER BY " + order2;
        }
        #endregion


        public override DataTable SelectTRecursion(string[] selectProperties, string whereSql, object[] values, string propertyIdName, string propertyFatherIdName, object propertyFatherIdValue, IDbConnection conn, IDbTransaction trans)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            if (selectProperties == null)
            {
               selectProperties = helper.PropertyField_Dictionary.Select(f => f.Key).ToArray();
            }
            selectProperties = OperationBase.AddPropertyFields(selectProperties, propertyIdName, propertyFatherIdName);

            string fieldIdName = MappingUtility.GetField<T>(propertyIdName);
            string fieldFatherIdName = MappingUtility.GetField<T>(propertyFatherIdName);
            string fields = GetSelectFields(selectProperties);
            

            List<string> parameterNames;
            List<object> parameterValues;
            string where, order;
            MappingUtility.GetFieldWhereAndOrder<T>(whereSql, values, out where, out order, out parameterNames, out parameterValues, this.GetWhereFormat);

            if (String.IsNullOrEmpty(order))
            {
                order = String.Format(@"order by [{0}] desc", MappingUtility.GetDefaultOrderField<T>());
            }
            if (String.IsNullOrEmpty(where))
            {
                where = "1=1";
            }

            string fieldFatherNameParamName = fieldFatherIdName + "ParamName_";
            parameterNames.Add(fieldFatherNameParamName);
            parameterValues.Add(propertyFatherIdValue);

            string tablefields = OperationBase.GetSelectTableFields(fields, helper.TableName);
            StringBuilder sb = new StringBuilder(MappingUtility.SqlLength);

            sb.AppendLine(String.Format(@"with tempRecursion({0}) as (", fields));
            sb.AppendLine(String.Format(@"select {0} from [{1}] where [{2}]={3}", fields, helper.TableName, fieldFatherIdName, GetParamFormat(fieldFatherNameParamName)));
            sb.AppendLine("union all");
            sb.AppendLine(String.Format(@"select {4} from [{0}], tempRecursion where [{0}].[{2}]=tempRecursion.[{1}] and {3})", helper.TableName, fieldIdName, fieldFatherIdName, where, tablefields));
            sb.AppendLine(String.Format(@"select {0} from tempRecursion {1}", fields, order));

            return OperationDB.ExecuteDataSet(sb.ToString(), parameterNames, parameterValues, this.GetDataAdapter(), conn, trans).Tables[0];
        }
    }
}
