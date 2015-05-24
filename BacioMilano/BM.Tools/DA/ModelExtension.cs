using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Reflection;

namespace BM.DA
{
    /// <summary>
    /// IModel扩展
    /// </summary>
    public static class ModelExtension
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <returns>影响行数</returns>
        public static int Insert(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType() };
            var methodInfo = mapping.GetType().GetMethod("Insert", types);

            object[] args = { imodel };
            return (int)(methodInfo.Invoke(mapping, args));

        }

       
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public static int Insert(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("Insert", types);

            object[] args = { imodel, conn, trans };
            return (int)(methodInfo.Invoke(mapping, args));
        }

      

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>自增量主键</returns>
        public static object InsertReturnIdentity(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType() };
            var methodInfo = mapping.GetType().GetMethod("InsertReturnIdentity", types);

            object[] args = { imodel };
            return methodInfo.Invoke(mapping, args);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>自增量主键</returns>
        public static object InsertReturnIdentity(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("InsertReturnIdentity", types);

            object[] args = { imodel, conn, trans };
            return methodInfo.Invoke(mapping, args);
        }

      




        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <returns>影响行数</returns>
        public static int Update(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType()};
            var methodInfo = mapping.GetType().GetMethod("Update", types);

            object[] args = { imodel };
            return (int)(methodInfo.Invoke(mapping, args));

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public static int Update(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("Update", types);

            object[] args = { imodel , conn, trans};
            return (int)(methodInfo.Invoke(mapping, args));

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <returns>影响行数</returns>
        public static int Delete(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType() };
            var methodInfo = mapping.GetType().GetMethod("Delete", types);

            object[] args = { imodel };
           return  (int)(methodInfo.Invoke(mapping, args));

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>影响行数</returns>
        public static int Delete(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("Delete", types);

            object[] args = { imodel, conn, trans };
            return (int)(methodInfo.Invoke(mapping, args));

        }


        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        public static int InsertOrUpdate(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType() };
            var methodInfo = mapping.GetType().GetMethod("InsertOrUpdate", types);
            object[] args = { imodel };
            return (int)(methodInfo.Invoke(mapping, args));
        }

        /// <summary>
        /// 添加或修改操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        public static int InsertOrUpdate(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("InsertOrUpdate", types);
            object[] args = { imodel, conn, trans };
            return (int)(methodInfo.Invoke(mapping, args));
        }

       

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <returns>是否存在</returns>
        public static bool IsExist(this IModel imodel)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType() };
            var methodInfo = mapping.GetType().GetMethod("IsExist", types);

            object[] args = { imodel };
            return (bool)(methodInfo.Invoke(mapping, args));
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <returns>是否存在</returns>
        public static bool IsExist(this IModel imodel, IDbConnection conn, IDbTransaction trans)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { imodel.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("IsExist", types);

            object[] args = { imodel, conn, trans };
            return (bool)(methodInfo.Invoke(mapping, args));
        }


        /// <summary>
        /// 通过Id加载实体
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="ids">主键值</param>
        /// <returns>是否成功</returns>
        public static bool Load(this IModel imodel, params object[] ids)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { ids.GetType() };
            var methodInfo = mapping.GetType().GetMethod("GetById", types);

            object[] args ={ ids };
            var obj = methodInfo.Invoke(mapping, args);
            if (obj == null)
            {
                return false;
            }
            else
            {
                Util.ObjectConvert.SetValue(imodel, obj);
                return true;
            }
        }

        /// <summary>
        /// 通过Id加载实体
        /// </summary>
        /// <param name="imodel">实体对象</param>
        /// <param name="id">主键值</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">事务对象，如果不启用事务，赋null</param>
        /// <param name="ids">主键值</param>
        /// <returns>是否成功</returns>
        public static bool Load(this IModel imodel,  IDbConnection conn, IDbTransaction trans, params object[] ids)
        {
            Type type = typeof(MappingBase<>);
            type = type.MakeGenericType(imodel.GetType());

            var mapping = type.InvokeMember("GetMapping", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            Type[] types = { ids.GetType(), typeof(IDbConnection), typeof(IDbTransaction) };
            var methodInfo = mapping.GetType().GetMethod("GetById", types);

            object[] args = { conn, trans, ids };
            var obj = methodInfo.Invoke(mapping, args);
            if (obj == null)
            {
                return false;
            }
            else
            {
                Util.ObjectConvert.SetValue(imodel, obj);
                return true;
            }
        }
    }
}
