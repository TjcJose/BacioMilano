using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Model.BModel;
using BM.Model.DbModel;
using BM.Util;
using BM.DA;
using System.Data;
using BM.Model.EnumType;


namespace BM.Fw
{
    public partial class BllManager
    {
        private static IEnumerable<T_MMenu> all_menu;
        private static IEnumerable<T_MMenuFunOper> all_menuFunOper;
        private static IEnumerable<T_MFunOper> all_funOper;
        private static IEnumerable<T_MFunction> all_function;
        private static IEnumerable<T_MOperation> all_operation;
       

        static BllManager()
        {
            var map_funOper = SingletonHelper<ModelDAL<T_MFunOper>>.Instance.Mapping;
            var map_function = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            var map_operation = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;

            using(var conn = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping.CreateConnection())
            {
                if(conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using(var ts = conn.BeginTransaction())
                {
                    all_funOper = map_funOper.GetAll(conn, ts);
                    _init_menu(conn, ts);
                    _init_menuFunOper(conn, ts);
                    all_function = map_function.GetAll(conn, ts);
                    all_operation = map_operation.GetAll(conn, ts);
                }
            }
        }

        #region Function
        public static T_MFunction Function_GetById(int functionId)
        {
            if( all_function.Count(f=>f.FunctionId == functionId) > 0)
            {
                return all_function.First(f => f.FunctionId == functionId);
            }
            return null;
        }

        public static IEnumerable<T_MFunction> Function_SelectAll()
        {
            return all_function;
        }

        public static bool Function_Delete(int functionId)
        {
           var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
           using (var conn = map.CreateConnection())
           {
               if (conn.State != ConnectionState.Open)
               {
                   conn.Open();
               }
               using (var ts = conn.BeginTransaction())
               {
                   if (map.DeleteById(functionId, conn, ts) > 0)
                   {
                       all_function = map.GetAll(conn, ts);
                       ts.Commit();
                       return true;
                   }
                   return false;
               }
           }
        }

        public static int Function_GetNewFunctionId()
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            return map.SelectStatisticInt(null, null, StatisticType.Max, T_MFunction_Description.FunctionId) + 20;
        }

        public static int Function_Add(T_MFunction entity)
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistById(entity.FunctionId.Value, conn, ts))
                    {
                        return ConstResult.function_exist_id;
                    }

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MFunction_Description.FunctionName, entity.FunctionName);
                    if (map.IsExistByFields(dic, conn, ts))
                    {
                        return ConstResult.function_exist_name;
                    }

                    if (map.Insert(entity, conn, ts) > 0)
                    {
                        all_function = map.GetAll(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static int Function_Modify(T_MFunction entity)
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MFunction_Description.FunctionName, entity.FunctionName);
                    if (map.IsExistByFields_NotEqualByField(dic, T_MFunction_Description.FunctionId, entity.FunctionId.Value, conn, ts))
                    {
                        return ConstResult.function_exist_name;
                    }

                    if (map.Update(entity, conn, ts) > 0)
                    {
                        all_function = map.GetAll(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }


        public static bool Function_Is_FunctionName_Unique_Add( string functionName)
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MFunction_Description.FunctionName, functionName);
            return map.IsExistByFields(dic);
        }

        public static bool Function_Is_FunctionName_Unique_Modify(string functionName, int functionId)
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MFunction_Description.FunctionName, functionName);
            return map.IsExistByFields_NotEqualByField(dic, T_MFunction_Description.FunctionId, functionId);
        }

        public static bool Function_Is_FunctionId_Unique_Add(int functionId)
        {
            var map = SingletonHelper<ModelDAL<T_MFunction>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MFunction_Description.FunctionId, functionId);
            return map.IsExistByFields(dic);
        }

        
        #endregion

        #region Operation

        public static IEnumerable<T_MOperation> Operation_SelectAll()
        {
            return all_operation;
        }

        public static int Operation_GetNewOperationId()
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            return map.SelectStatisticInt(null, null, StatisticType.Max, T_MOperation_Description.OperationId) + 20;
        }

        public static T_MOperation Operation_GetById(int operationId)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            return map.GetEntityById(operationId);
        }

        public static int Operation_Add(T_MOperation entity)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MOperation_Description.OperationId, entity.OperationId);
                    if (map.IsExistByFields(dic, conn, ts))
                    {
                        return ConstResult.operation_exist_id;
                    }

                    dic.Remove(T_MOperation_Description.OperationId);
                    dic.Add(T_MOperation_Description.OperationName, entity.OperationName);
                    if (map.IsExistByFields(dic, conn, ts))
                    {
                        return ConstResult.operation_exist_name;
                    }

                    if (map.Insert(entity, conn, ts) > 0)
                    {
                        all_operation = map.GetAll(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static int Operation_Modify(T_MOperation entity)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MOperation_Description.OperationName, entity.OperationName);
                    if (map.IsExistByFields_NotEqualByField(dic, T_MOperation_Description.OperationId, entity.OperationId.Value, conn, ts))
                    {
                        return ConstResult.operation_exist_name;
                    }

                    if (map.Update(entity, conn, ts) > 0)
                    {
                        all_operation = map.GetAll(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static bool Operation_Delete(int OperationId)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.DeleteById(OperationId, conn, ts) > 0)
                    {
                        all_operation = map.GetAll(conn, ts);
                        ts.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }


        public static bool Operation_Is_OperationName_Unique_Add(string OperationName)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MOperation_Description.OperationName, OperationName);
            return map.IsExistByFields(dic);
        }

        public static bool Operation_Is_OperationName_Unique_Modify(string OperationName, int OperationId)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MOperation_Description.OperationName, OperationName);
            return map.IsExistByFields_NotEqualByField(dic, T_MOperation_Description.OperationId, OperationId);
        }

        public static bool Operation_Is_OperationId_Unique_Add(int OperationId)
        {
            var map = SingletonHelper<ModelDAL<T_MOperation>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MOperation_Description.OperationId, OperationId);
            return map.IsExistByFields(dic);
        }

        #endregion

        #region Right

        public static bool Right_Group_Set(int groupId, string data)
        {
            var map_group = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            var map_right = SingletonHelper<ModelDAL<T_MGroupRight>>.Instance.Mapping;
            var map_funOper = SingletonHelper<ModelDAL<T_MFunOper>>.Instance.Mapping;
            using (var conn = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (!map_group.IsExistByField(T_MGroup_Description.GroupId, groupId, conn, ts))
                    {
                        return false;
                    }

                    var rights = map_right.SelectByProperty(T_MGroupRight_Description.GroupId, groupId, conn, ts);

                    T_MGroupRight right = new T_MGroupRight();
                    right.GroupId = groupId;
                    var arr = data.Split(',');
                    foreach (var s in arr)
                    {
                        var arr2 = s.Split('_');
                        right.FunctionId = int.Parse(arr2[0]);
                        right.OperationId = int.Parse(arr2[1]);

                        if (arr2[2] == "1")
                        {
                            if (rights.Count(f => f.FunctionId.Value == right.FunctionId.Value && f.OperationId.Value == right.OperationId.Value && f.GroupId.Value == groupId) == 0)
                            {
                                map_right.Insert(right, conn, ts);
                            }
                        }
                        else
                        {
                            if (rights.Count(f => f.FunctionId.Value == right.FunctionId.Value && f.OperationId.Value == right.OperationId.Value && f.GroupId.Value == groupId) > 0)
                            {
                                map_right.Delete(right, conn, ts);
                            }
                        }
                    }

                    ts.Commit();
                    return true;
                }
            }
        }

        public static bool Right_Set(long managerId, string data)
        {
            var map_manager = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            var map_right = SingletonHelper<ModelDAL<T_MRight>>.Instance.Mapping;
            var map_funOper = SingletonHelper<ModelDAL<T_MFunOper>>.Instance.Mapping;
            using (var conn = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if(!map_manager.IsExistByField(T_Manager_Description.ManagerId, managerId, conn, ts))
                    {
                        return false;
                    }

                    var rights = map_right.SelectByProperty(T_MRight_Description.ManagerId, managerId, conn, ts);
                    
                    T_MRight right = new T_MRight();
                    right.ManagerId = managerId;
                    var arr = data.Split(',');
                    foreach(var s in arr)
                    {
                        var arr2 = s.Split('_');
                        right.FunctionId = int.Parse(arr2[0]);
                        right.OperationId = int.Parse(arr2[1]);

                        if (arr2[2] == "1")
                        {
                           if (rights.Count(f => f.FunctionId.Value == right.FunctionId.Value && f.OperationId.Value == right.OperationId.Value && f.ManagerId.Value == managerId) == 0)
                           {
                               map_right.Insert(right, conn, ts);
                           }
                        }
                        else
                        {
                            if (rights.Count(f => f.FunctionId.Value == right.FunctionId.Value && f.OperationId.Value == right.OperationId.Value && f.ManagerId.Value == managerId) > 0)
                            {
                                map_right.Delete(right, conn, ts);
                            }
                        }
                    }

                    ts.Commit();
                    return true;
                }
            }
        }


        public static IEnumerable<T_MRight> Right_SelectByManagerId(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_MRight>>.Instance.Mapping;
            return map.SelectByProperty(T_MRight_Description.ManagerId, managerId);
        }

        public static IEnumerable<T_MGroupRight> Right_SelectByGroupId(int groupId)
        {
            var map = SingletonHelper<ModelDAL<T_MGroupRight>>.Instance.Mapping;
            return map.SelectByProperty(T_MGroupRight_Description.GroupId, groupId);
        }


        #endregion

        #region FunOper


        public static IEnumerable<V_MFunOper> VFunOper_SelectAll()
        {
            var map = SingletonHelper<ModelDAL<V_MFunOper>>.Instance.Mapping;
            return map.GetAll().OrderBy(f => f.FunctionId.Value).Select(f => f);
        }

        public static IEnumerable<int> FunOper_SelectByFunctionId(int functionId)
        {
            var map = SingletonHelper<ModelDAL<T_MFunOper>>.Instance.Mapping;
            return map.SelectByProperty(T_MFunOper_Description.FunctionId, functionId).Select(f=>f.OperationId.Value); 
        }

        public static bool FunOper_Set(int functionId, string data, ref Exception error)
        {
            var map_funOper = SingletonHelper<ModelDAL<T_MFunOper>>.Instance.Mapping;
            using (var conn = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (all_function.Count(f=>f.FunctionId.Value == functionId) == 0)
                    {
                        return false;
                    }
                    T_MFunOper funOper = new T_MFunOper();
                    funOper.FunctionId = functionId;
                    var arr = data.Split(',');
                    foreach (var s in arr)
                    {
                        var arr2 = s.Split('_');
                        funOper.OperationId = int.Parse(arr2[0]);

                        if(all_operation.Count(f=>f.OperationId.Value == funOper.OperationId) == 0)
                        {
                            return false;
                        }

                        if (arr2[1] == "1")
                        {
                            if (all_funOper.Count(f => f.FunctionId.Value == functionId && f.OperationId.Value == funOper.OperationId.Value) == 0)
                            {
                                try
                                {
                                    map_funOper.Insert(funOper, conn, ts);
                                }
                                catch(Exception ex)
                                {
                                    error = ex;
                                    BM.Log.LogHelper<BllManager>.GetLogger().Error(ex.Message, ex);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (all_funOper.Count(f => f.FunctionId.Value == functionId && f.OperationId.Value == funOper.OperationId.Value) > 0)
                            {
                                try
                                {
                                    map_funOper.Delete(funOper, conn, ts);
                                }
                                catch (Exception ex)
                                {
                                    error = ex;
                                    BM.Log.LogHelper<BllManager>.GetLogger().Error(ex.Message, ex);
                                    return false;
                                }
                            }
                        }
                    }

                    all_funOper = map_funOper.GetAll(conn, ts);

                    ts.Commit();
                    return true;
                }
            }
        }

        #endregion

        #region MenuFunOper


        public static IEnumerable<T_MMenuFunOper> MenuFunOper_SelectAll()
        {
            return all_menuFunOper;
        }


        public static bool MenuFunOper_Set(int menuId, string data, ref Exception error)
        {
            var map_menufunOper = SingletonHelper<ModelDAL<T_MMenuFunOper>>.Instance.Mapping;
            using (var conn = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (all_menu.Count(f => f.MenuId.Value == menuId) == 0)
                    {
                        return false;
                    }
                    T_MMenuFunOper menuFunOper = new T_MMenuFunOper();
                    menuFunOper.MenuId = menuId;
                    var arr = data.Split(',');
                    foreach (var s in arr)
                    {
                        var arr2 = s.Split('_');
                        menuFunOper.FunctionId = int.Parse(arr2[0]);
                        menuFunOper.OperationId = int.Parse(arr2[1]);

                        if (all_operation.Count(f => f.OperationId.Value == menuFunOper.OperationId) == 0)
                        {
                            return false;
                        }

                        if (arr2[2] == "1")
                        {
                            if (all_menuFunOper.Count(f => f.FunctionId.Value == menuFunOper.FunctionId.Value && f.OperationId.Value == menuFunOper.OperationId.Value && f.MenuId.Value == menuId) == 0)
                            {
                                try
                                {
                                    map_menufunOper.Insert(menuFunOper, conn, ts);
                                }
                                catch (Exception ex)
                                {
                                    error = ex;
                                    BM.Log.LogHelper<BllManager>.GetLogger().Error(ex.Message, ex);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (all_menuFunOper.Count(f => f.FunctionId.Value == menuFunOper.FunctionId && f.OperationId.Value == menuFunOper.OperationId.Value && f.MenuId.Value == menuId) > 0)
                            {
                                try
                                {
                                    map_menufunOper.Delete(menuFunOper, conn, ts);
                                }
                                catch (Exception ex)
                                {
                                    error = ex;
                                    BM.Log.LogHelper<BllManager>.GetLogger().Error(ex.Message, ex);
                                    return false;
                                }
                            }
                        }
                    }

                    
                    _init_menuFunOper(conn, ts);
                    ts.Commit();
                    return true;
                }
            }
        }

        #endregion

        #region Menu


        private static void _init_menu(IDbConnection conn, IDbTransaction ts)
        {
            var map_menu = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            all_menu = map_menu.GetAll(conn, ts).Where(f => f.IsUse.Value).OrderBy(f => f.MenuSort.Value);
        }

        private static void _init_menuFunOper(IDbConnection conn, IDbTransaction ts)
        {
            var map_menuFunOper = SingletonHelper<ModelDAL<T_MMenuFunOper>>.Instance.Mapping;
            var tmp = map_menuFunOper.GetAll(conn, ts);
            all_menuFunOper = from x in tmp join y in all_menu on x.MenuId.Value equals y.MenuId.Value where y.IsUse.Value select x;
        }




        private static void _Menu_SelectByManagerId_Sub(T_MMenu pm, IList<T_MMenu> list, long managerId = 0, IEnumerable<Right> rights = null, bool isContainSon_Zero = false)
        {
            bool isAdd;
            if (managerId == 0)
            {
                isAdd = true;
            }
            else
            {
                if (rights == null || rights.Count() == 0)
                {
                    return;
                }

                if (pm.MenuType != (int)MenuType.Group)
                {
                    var tmp = all_menuFunOper.Where(f => f.MenuId.Value == pm.MenuId.Value);
                    if (tmp.Count() == 0)
                    {
                        isAdd = false;
                    }
                    else
                    {
                        var menu = tmp.First();
                        isAdd = rights.Count(f => f.FunctionId == menu.FunctionId) > 0;
                    }
                }
                else
                {
                    isAdd = true;
                }
            }

            if (isAdd)
            {
                if (pm.MenuType == (int)MenuType.Group)
                {
                    var sons = all_menu.Where(f => f.ParentId == pm.MenuId);
                    if (sons.Count() > 0)
                    {
                        foreach (var son in sons)
                        {
                            _Menu_SelectByManagerId_Sub(son, pm.Sons, managerId, rights);
                        }

                        
                    }

                    if (pm.Sons.Count > 0 || isContainSon_Zero)
                    {
                        list.Add(pm);
                    }
                }
                else
                {
                    list.Add(pm);
                }
            }
        }

        public static IEnumerable<T_MMenu> Menu_SelectByManagerId(long managerId = 0, IEnumerable<Right> rights = null, bool isContainSon_Zero = false)
        {
            List<T_MMenu> list = new List<T_MMenu>();
            var arrOne = all_menu.Where(f => f.ParentId == 0).Select(f => f);
            foreach (var pm in arrOne)
            {
                pm.Sons = new List<T_MMenu>();
                _Menu_SelectByManagerId_Sub(pm, list, managerId, rights, isContainSon_Zero);
            }

            return list;
        }



        public static int Menu_GetNewMenuSort(int parentId)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            string where = string.Format(@"#{0}=@{0}", T_MMenu_Description.ParentId);
            object[] values = { parentId };
            return map.SelectStatisticInt(where, values, StatisticType.Max, T_MMenu_Description.MenuSort) + 20;
        }

        public static T_MMenu Menu_GetById(int id)
        {
            if (all_menu.Count(f => f.MenuId == id) > 0)
            {
                return all_menu.First(f => f.MenuId == id);
            }
            return null;
        }


        public static int Menu_GetNewMenuId()
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            return map.SelectStatisticInt(null, null, StatisticType.Max, T_MMenu_Description.MenuId) + 20;
        }

       
        public static IEnumerable<T_MMenu> Menu_SelectAll()
        {
            return all_menu;
        }

        public static bool Menu_HasSon(int id)
        {
            return all_menu.Count(f => f.ParentId.Value == id) > 0;
        }

        public static int Menu_Add(T_MMenu entity)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MMenu_Description.MenuId, entity.MenuId);
                    if (map.IsExistByFields(dic, conn, ts))
                    {
                        return ConstResult.menu_exist_id;
                    }

                    dic.Remove(T_MMenu_Description.MenuId);
                    dic.Add(T_MMenu_Description.MenuName, entity.MenuName);
                    dic.Add(T_MMenu_Description.ParentId, entity.ParentId);
                    if (map.IsExistByFields(dic, conn, ts))
                    {
                        return ConstResult.menu_exist_name;
                    }

                    if (map.Insert(entity, conn, ts) > 0)
                    {
                        _init_menu(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static int Menu_Modify(T_MMenu entity)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_MMenu_Description.ParentId, entity.ParentId);
                    dic.Add(T_MMenu_Description.MenuName, entity.MenuName);
                    if (map.IsExistByFields_NotEqualByField(dic, T_MMenu_Description.MenuId, entity.MenuId.Value, conn, ts))
                    {
                        return ConstResult.menu_exist_name;
                    }

                    if (map.Update(entity, conn, ts) > 0)
                    {
                        _init_menu(conn, ts);
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }


        public static bool Menu_Delete(int menuId)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.DeleteById(menuId, conn, ts) > 0)
                    {
                        _init_menu(conn, ts);
                        ts.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }


        public static bool Menu_Is_MenuName_Unique_Add(string menuName, int parentId)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MMenu_Description.ParentId, parentId);
            dic.Add(T_MMenu_Description.MenuName, menuName);
            return map.IsExistByFields(dic);
        }

        public static bool Menu_Is_MenuName_Unique_Modify(string menuName, int menuId, int parentId)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MMenu_Description.MenuName, menuName);
            dic.Add(T_MMenu_Description.ParentId, parentId);
            return map.IsExistByFields_NotEqualByField(dic, T_MMenu_Description.MenuId, menuId);
        }

        public static bool Menu_Is_MenuId_Unique_Add(int menuId)
        {
            var map = SingletonHelper<ModelDAL<T_MMenu>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MMenu_Description.MenuId, menuId);
            return map.IsExistByFields(dic);
        }

        #endregion
    }
}
