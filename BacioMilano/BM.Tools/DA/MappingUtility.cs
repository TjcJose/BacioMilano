using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using BM.Util;

namespace BM.DA
{
    internal static class MappingUtility
    {
        internal const int SqlLength = 2000;
       

        #region internal
        /// <summary>
        /// 正则分组属性名
        /// </summary>
        internal const string PropertyName = "Property";
        /// <summary>
        /// 参数前缀
        /// </summary>
        internal const string PreParam = "@";

        /// <summary>
        /// 属性前缀
        /// </summary>
        internal const string PreProp = "#";

        /// <summary>
        /// 表名或视图前缀
        /// </summary>
        internal const string PreTable = "~";


        #region GetOrderMatchCollection 排序匹配集合
        internal static readonly Regex reg_GetOrderMatchCollection = new Regex(@"(?<Property>[a-z0-9_]+)\s+(?<Order>(a|de)sc)",
            RegexOptions.Compiled |
            RegexOptions.IgnoreCase |
            RegexOptions.ExplicitCapture);

        /// <summary>
        /// 得到排序匹配集合
        /// </summary>
        /// <param name="orderByExpression">排序字符串</param>
        /// <returns>排序匹配集合</returns>
        internal static MatchCollection GetOrderMatchCollection(string orderByExpression)
        {
            if (reg_GetOrderMatchCollection.IsMatch(orderByExpression))
                return reg_GetOrderMatchCollection.Matches(orderByExpression);
            return null;
        }
        #endregion

        #region GetParamListFromSql 得到Sql的参数列表
        private const string str_regGetFromSql = @"(?<Property>[a-z_][a-z0-9_]*)((?=[\s,()""'=+-/><*%]+)|$)";

        internal static readonly Regex reg_GetTableListFromSql = new Regex(PreTable + str_regGetFromSql,
          RegexOptions.Compiled |
          RegexOptions.IgnoreCase |
          RegexOptions.ExplicitCapture);
        /// <summary>
        /// 得到Sql的表名或视图列表
        /// </summary>
        /// <param name="sqlPart">Sql字符串或部分Sql字符串</param>
        /// <returns>Sql的表名或视图列表</returns>
        internal static List<string> GetTableListFromSql(string sqlPart)
        {
            List<string> list = new List<string>();
            foreach (Match m in reg_GetTableListFromSql.Matches(sqlPart))
            {
                string property = m.Groups[PropertyName].Value;
                if (!list.Contains(property))
                {
                    list.Add(property);
                }
            }
            return list;
        }




        internal static readonly Regex reg_GetParamListFromSql = new Regex(PreParam + str_regGetFromSql,
            RegexOptions.Compiled |
            RegexOptions.IgnoreCase |
            RegexOptions.ExplicitCapture);

        /// <summary>
        /// 得到Sql的参数列表
        /// </summary>
        /// <param name="sqlPart">Sql字符串或部分Sql字符串</param>
        /// <returns>Sql的参数列表</returns>
        internal static List<string> GetParamListFromSql(string sqlPart)
        {
            List<string> list = new List<string>();
            foreach (Match m in reg_GetParamListFromSql.Matches(sqlPart))
            {
                string property = m.Groups[PropertyName].Value;
                if (!list.Contains(property))
                {
                    list.Add(property);
                }
            }
            return list;
        }

        #endregion

        #region GetPropListFromSql 得到Sql的字段对应属性列表
        internal static readonly Regex reg_GetPropListFromSql = new Regex(PreProp + str_regGetFromSql,
            RegexOptions.Compiled |
            RegexOptions.IgnoreCase |
            RegexOptions.ExplicitCapture);

        /// <summary>
        /// 得到Sql的字段对应属性列表
        /// </summary>
        /// <param name="sqlPart">Sql字符串或部分Sql字符串</param>
        /// <returns>Sql的字段对应属性列表</returns>
        internal static List<string> GetPropListFromSql(string sqlPart)
        {
            List<string> list = new List<string>();
            foreach (Match m in reg_GetPropListFromSql.Matches(sqlPart))
            {
                string property = m.Groups[PropertyName].Value;
                if (!list.Contains(property))
                {
                    list.Add(property);
                }
            }
            return list;
        }

       

        #endregion

        #region GetWhereAndOrder 得到查询条件和排序条件
        internal static readonly Regex reg_GetWhereAndOrder = new Regex(@"order\s+by",
                RegexOptions.Compiled |
                RegexOptions.IgnoreCase |
                RegexOptions.ExplicitCapture);


        /// <summary>
        /// 得到查询条件和排序条件 
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">如(#property1=@param1 and #property2=param2 order by #property1 desc)格式的表达式</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件</param>
        internal static void GetPropertyWhereAndOrder<T>(string expression, out string where, out string order)
        {
            if (String.IsNullOrEmpty(expression))
            {
                order = where = String.Empty;
            }
            else if (!reg_GetWhereAndOrder.IsMatch(expression))
            {
                where = expression;
                order = String.Empty;
            }
            else
            {
                expression = expression.Trim();
                string[] strArr = reg_GetWhereAndOrder.Split(expression);
                if (strArr.Length != 2)
                {
                    throw new ArgumentException(String.Format(@"{0} whereOrderBy expression error", typeof(T).Name));
                }
                where = strArr[0];
                order = strArr[1];
            }
        }

        /// <summary>
        /// 得到查询条件和排序条件 
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">如(#property1=@param1 and #property2=param2 order by #property1 desc)格式的表达式</param>
        /// <param name="values">查询参数值数组</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="parameterNames">查询参数名称列表</param>
        /// <param name="parameterValues">查询参数值列表</param>
        public static void GetFieldWhereAndOrder<T>(string expression, object[] values, out string where, out string order, out List<string> parameterNames, out List<object> parameterValues, Func< IList<string>, string, string> whereFormat)
        {
            GetPropertyWhereAndOrder<T>(expression, out where, out order);
            if (String.IsNullOrEmpty(where) && String.IsNullOrEmpty(order) && (values == null || values.Length == 0))
            {
                parameterNames = new List<string>();
                parameterValues = new List<object>();
                return;
            }
            if (String.IsNullOrEmpty(where) && values != null && values.Length > 0)
            {
                throw new ArgumentException(String.Format(@"{0} whereSql params error, not equal to values length."));
            }

            Dictionary<string, string> useCompareKeyReplaceValue;
            CompareHelper<string> compareHelp = new CompareHelper<string>(StringHelper.CompareIgnoreCase);
            if (String.IsNullOrEmpty(order) == false)
            {
                useCompareKeyReplaceValue = GetPropertiesFields<T>(GetPropListFromSql(order).ToArray());
                order = reg_GetPropListFromSql.ReplaceByArray(PropertyName, order, useCompareKeyReplaceValue, compareHelp);
            }

            if (String.IsNullOrEmpty(where) == false)
            {
                parameterNames = GetParamListFromSql(where);
                parameterValues = values == null ? new List<object>() : values.ToList();
                useCompareKeyReplaceValue = GetPropertiesFields<T>(GetPropListFromSql(where).ToArray());
                where = reg_GetPropListFromSql.ReplaceByArray(PropertyName, where, useCompareKeyReplaceValue, compareHelp);
                where = whereFormat(parameterNames, where);
            }
            else
            {
                parameterNames = new List<string>();
                parameterValues = new List<object>();
            }
        }
        #endregion

        #region GetParamName 根据初始参数名得到参数名并且和使其和参数名列表中的参数名不相同
        /// <summary>
        /// 根据初始参数名得到参数名并且和使其和参数名列表中的参数名不相同
        /// </summary>
        /// <param name="list">参数名列表</param>
        /// <param name="paramName">初始参数名</param>
        /// <returns>参数名</returns>
        internal static string GetParamName(List<string> list, string paramName)
        {
            CompareHelper<string> compareHelp = new CompareHelper<string>(StringHelper.CompareIgnoreCase);
            compareHelp.SetCompareObj(paramName);
            int i = 0;
            while (list.FindIndex(compareHelp.Compare) > -1)
            {
                paramName = String.Format(@"{0}{1}", paramName, i++);
                compareHelp.SetCompareObj(paramName);
            }
            return paramName;
        }
        #endregion

        #region GetPropertiesFields 得到属性字段对应字典
        /// <summary>
        /// 得到属性字段对应字典
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Properties">搜索字段数组对应的属性数组</param>
        /// <returns>属性字段对应字典</returns>
        internal static Dictionary<string, string> GetPropertiesFields<T>(string[] properties)
        {
            Dictionary<string, string> dic = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).PropertyField_Dictionary;

            var temp = from d in dic
                       join pros in properties
                       on d.Key.ToLower() equals pros.ToLower()
                       select d;

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var x in temp)
            {
                result.Add(x.Key, x.Value);
            }
            return result;
        }


        #endregion

        #region GetFields 得到属性对应字段
        /// <summary>
        /// 得到属性对应字段列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="properties">搜索字段数组对应的属性数组</param>
        /// <returns>属性对应字段列表</returns>
        internal static List<string> GetFields<T>(string[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                throw new ArgumentNullException("GetFields is null");
            }

            Dictionary<string, string> dic = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).PropertyField_Dictionary;

            var temp = from d in dic
                       join pros in properties
                       on d.Key.ToLower() equals pros.ToLower()
                       select d.Value;


            return temp.ToList();
        }

        #endregion

        #region GetFieldsValues 从实体中提取对应的数据表中的字段和值得列表
        /// <summary>
        /// 从实体中提取对应的数据表中的字段和值得列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="containPrimary">是否包含主键</param>
        /// <param name="fields">提取的字段列表</param>
        /// <param name="values">提取的值列表</param>
        internal static void GetFieldsValues<T>(T entity, bool containPrimary, out List<string> fields, out List<object> values)
        {
            fields = new List<string>();
            values = new List<object>();
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            PropertyInfo[] infos = typeof(T).GetProperties();
           
            foreach (PropertyInfo info in infos)
            {
                if (!helper.PropertyField_Dictionary.Keys.Contains(info.Name))
                {
                    continue;
                }
                object value = info.GetValue(entity, null);
                if (!containPrimary && helper.PrimaryProperties.Contains(info.Name))
                {
                    continue;
                }

                if (value == null)
                {
                    continue;
                }

                if (helper.PropertyField_Dictionary.Keys.Contains(info.Name))
                {
                    fields.Add(helper.PropertyField_Dictionary[info.Name]);
                    values.Add(value);
                }
            }

            

        }
        #endregion

        #region GetFieldsValues 从属性名属性值字典中提取对应数据表中的字段和值得列表
        /// <summary>
        /// 从实体中提取对应的数据表中的字段和值得列表
        /// </summary>
        /// <param name="propertiesValues">属性名属性值字典</param>
        /// <param name="fields">提取的字段列表</param>
        /// <param name="values">提取的值列表</param>
        /// <param name="isFilterNull">是否过滤掉null</param>
        internal static void GetFieldsValues<T>(Dictionary<string, object> propertiesValues, out List<string> fields, out List<object> values, bool isFilterNull)
        {
            fields = new List<string>();
            values = new List<object>();
            Type type = typeof(T);
            PropertyInfo[] infos = type.GetProperties();
            var dic = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).PropertyField_Dictionary;

            
            foreach (KeyValuePair<string, object> keyValue in propertiesValues)
            {
                var query = from x in propertiesValues
                            where x.Key.ToLower() == keyValue.Key.ToLower()
                            select x;

                if (query.Count() > 0)
                {
                    var prop = query.First().Value;
                    object value = keyValue.Value;
                    if (value == null && isFilterNull)
                    {
                        continue;
                    }

                    fields.Add(dic[keyValue.Key]);
                    values.Add(keyValue.Value);
                }
            }
        }
        #endregion

        #region GetDefaultOrderField
        /// <summary>
        /// 得到默认排序字段
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <returns>默认排序字段</returns>
        internal static string GetDefaultOrderField<T>() where T : new()
        {
            return ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T)).PrimaryProperties[0];
        }
        #endregion

        #endregion

        #region public

        #region GetPropertysValues 得到可读写的属性名属性值字典
        /// <summary>
        /// 得到属性名属性值字典(可读写的且必须有对应字段)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="validPropertyReadOnly">实体配置的只读属性是否有效</param>
        /// <param name="validPropertyIgnoreDefaultValue">实体配置的默认值忽略是否有效</param>
        /// <returns>属性名属性值字典</returns>
        internal static Dictionary<string, object> GetValidPropertysValues<T>(T entity)
        {
            return GetValidPropertysValues<T, T>(entity);
        }

        /// <summary>
        /// 得到属性名属性值字典(可读写的且必须有对应字段)
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <typeparam name="U">实体配置信息类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="validPropertyReadOnly">实体配置的只读属性是否有效</param>
        /// <param name="validPropertyIgnoreDefaultValue">实体配置的默认值忽略是否有效</param>
        /// <returns>属性名属性值字典</returns>
        internal static Dictionary<string, object> GetValidPropertysValues<T, U>(T entity)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type type = typeof(T);
            PropertyInfo[] infos = type.GetProperties();
           

            foreach (PropertyInfo info in infos)
            {

                    object value = info.GetValue(entity, null);
                    if (value == null)
                    {
                        continue;
                    }

                    dic.Add(info.Name, value);
                
            }
            return dic;
        }
        #endregion

        #region GetPropertyNameIgnoreCase 得到属性名不区分大小写
        /// <summary>
        /// 得到属性名不区分大小写
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal static string GetPropertyNameIgnoreCase<T>(string propertyName)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            if (helper.PropertyField_Dictionary.Count(s => s.Key.ToLower() == propertyName.ToLower()) == 1)
            {
                return propertyName;
            }
            throw new ArgumentException(String.Format(@"{0} 实体没有 {1} 属性", typeof(T).Name, propertyName));
        }
        #endregion

        #region SetPropertysValues 设置属性名属性值字典值
        /// <summary>
        /// 设置属性名属性值字典值
        /// </summary>
        /// <param name="propertiesValues"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertysValues<T>(ref Dictionary<string, object> propertiesValues, string propertyName, object value)
        {
            if (propertiesValues.ContainsKey(propertyName))
            {
                propertiesValues[propertyName] = value;
            }
            else
            {
                propertiesValues.Add(propertyName, value);
            }
        }
        #endregion

        #region GetPrimaryKeysWhereSqlFromEntity 得到用主键条件(可用于更新)



        /// <summary>
        /// 得到用主键条件(可用于更新)
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <typeparam name="U">实体配置信息类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="values">输出主键值</param>
        /// <param name="parameterNames">参数名</param>
        /// <returns>得到主键条件(如: #fieldKey=@parma)</returns>
        public static string GetPrimaryKeysWhereSqlFromEntity<T, U>(T entity, out List<object> values, ref List<string> parameterNames)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(U));

            if(helper.PrimaryProperties.Length == 0)
            {
                throw new ArgumentNullException(@"{0} primary key not exist", helper.EntityName);
            }

            string result = "";
            values = new List<object>();
            for (int i = 0; i != helper.PrimaryProperties.Length; i++)
            {
                string primaryProperty = helper.PrimaryProperties[i];
                string parameterName = GetParamName(parameterNames, primaryProperty);
                object value = entity.GetType().GetProperty(primaryProperty).GetValue(entity, null);
                if (value == null)
                {
                    throw new ArgumentNullException(@"{0} primary key value is null", helper.EntityName);
                }
                values.Add(value);
                parameterNames.Add(parameterName);

                if(helper.PrimaryProperties.Length == 1)
                {
                    result = String.Format(@"{0}{1}={2}{3}", PreProp, helper.PrimaryProperties[i], PreParam, parameterName);
                    break;
                }

                if(i == 0)
                {
                    result = String.Format(@"({0}{1}={2}{3}", PreProp, helper.PrimaryProperties[i], PreParam, parameterName);
                }
                else
                {
                    result += String.Format(@" and {0}{1}={2}{3}", PreProp, helper.PrimaryProperties[i], PreParam, parameterName);
                }
                if(i == helper.PrimaryProperties.Length - 1)
                {
                    result += ")";
                }
            }

            return result; 
        }

        /// <summary>
        /// 得到用主键条件(可用于更新)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="values">输出主键值</param>
        /// <param name="parameterNames">参数名</param>
        /// <returns>得到主键条件(如: #fieldKey=@parma)</returns>
        public static string GetPrimaryKeysWhereSqlFromEntity<T>(T entity, out List<object> values, ref List<string> parameterNames)
        {
            return GetPrimaryKeysWhereSqlFromEntity<T, T>(entity, out values, ref parameterNames);
        }
        #endregion

        #region GetField 得到属性对应字段
        /// <summary>
        /// 得到属性对应字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="property">字段对应的属性</param>
        /// <returns>字段</returns>
        public static string GetField<T>(string property)
        {
            var helper = ModelDescriptionHelper.Get_ModelDescriptionHelper(typeof(T));
            if(!helper.PropertyField_Dictionary.ContainsKey(property))
            { 
                throw new ArgumentNullException(String.Format(@"{0} property {1} can't find field", helper.EntityName, property));
            }

            return helper.PropertyField_Dictionary[property];
        }
        #endregion

        #endregion
    }
}
