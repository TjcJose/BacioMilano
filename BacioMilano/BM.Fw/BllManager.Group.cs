using BM.DA;
using BM.Model.DbModel;
using BM.Model.VModel;
using BM.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    public partial class BllManager
    {
        public static bool Group_UserGroupsSet(long managerId, string data)
        {
            var map_manager = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            var map_managerGroup = SingletonHelper<ModelDAL<T_MManagerGroup>>.Instance.Mapping;
            
            using (var conn = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (!map_manager.IsExistByField(T_Manager_Description.ManagerId, managerId, conn, ts))
                    {
                        return false;
                    }

                    var managerGroups = map_managerGroup.SelectByProperty(T_MManagerGroup_Description.ManagerId, managerId, conn, ts);

                    T_MManagerGroup mg = new T_MManagerGroup();
                    mg.ManagerId = managerId;
                    var arr = data.Split(',');
                    foreach (var s in arr)
                    {
                        var arr2 = s.Split('_');
                        mg.GroupId = int.Parse(arr2[0]);
                        
                        if (arr2[1] == "1")
                        {
                            if (managerGroups.Count(f =>  f.GroupId.Value == mg.GroupId.Value && f.ManagerId.Value == managerId) == 0)
                            {
                                map_managerGroup.Insert(mg, conn, ts);
                            }
                        }
                        else
                        {
                            if (managerGroups.Count(f => f.GroupId.Value == mg.GroupId.Value && f.ManagerId.Value == managerId) > 0)
                            {
                                map_managerGroup.Delete(mg, conn, ts);
                            }
                        }
                    }

                    ts.Commit();
                    return true;
                }
            }
        }

        public static IEnumerable<int> Group_SelectGroupIdByManagerId(long managerId)
        {
            var map_managerGroup = SingletonHelper<ModelDAL<T_MManagerGroup>>.Instance.Mapping;
            var datas = map_managerGroup.SelectByProperty(T_MManagerGroup_Description.ManagerId, managerId);
            return datas.Select(f => f.GroupId.Value);
        }

        public static IEnumerable<V_MManagerGroup> VMManagerGroup_SelectByManagerId(long managerId, IDbConnection conn, IDbTransaction ts)
        {
            var map_managerGroup = SingletonHelper<ModelDAL<V_MManagerGroup>>.Instance.Mapping;
            return map_managerGroup.SelectByProperty(V_MManagerGroup_Description.ManagerId, managerId, conn, ts);
        }

        public static IEnumerable<V_MManagerGroup> VMManagerGroup_SelectByManagerId(long managerId)
        {
            var map = SingletonHelper<ModelDAL<V_MManagerGroup>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return VMManagerGroup_SelectByManagerId(managerId, conn, null);
            }
        }


        public static T_MGroup Group_GetById(int groupId)
        {
             var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
             return map.GetEntityById(groupId);
        }

        public static int Group_Add(MGroupAddModel model)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistByField(T_MGroup_Description.GroupName, model.GroupName, conn, ts))
                    {
                        return ConstResult.group_exist_groupName;
                    }
                    T_MGroup entity = new T_MGroup();
                    entity.GroupMemo = model.GroupMemo;
                    entity.GroupName = model.GroupName;
                    entity.GroupId = BM.Util.ObjectConvert.GetIntValue(map.InsertReturnIdentity(entity, conn, ts));
                    ts.Commit();
                }
            }
            return ConstResult.success;
        }

        public static int Group_Modify(MGroupModifyModel model)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistByField_NotEqualByField(T_MGroup_Description.GroupName, model.GroupName, T_MGroup_Description.GroupId, model.GroupId, conn, ts))
                    {
                        return ConstResult.group_exist_groupName;
                    }

                    var entity = new T_MGroup();
                    ObjectConvert.ConvertAToB(model, ref entity);
                    if (map.Update(entity, conn, ts) > 0)
                    {
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static IEnumerable<T_MGroup> Group_All()
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            return map.GetAll();
        }

        public static PageListModel<T_MGroup> Group_Search(MGroupSearchModel options, int pageSize, int pageIndex)
        {
            List<object> ls = new List<object>();
            StringBuilder sb = new StringBuilder("1=1");
            ls.Add(true);

            if (options.GroupIdS != null)
            {
                sb.AppendFormat(@" and #{0} = @{0}", T_MGroup_Description.GroupId);
                ls.Add(options.GroupIdS.Value);
            }
            if (!String.IsNullOrWhiteSpace(options.GroupNameS))
            {
                sb.AppendFormat(@" and #{0} like @{0}", T_MGroup_Description.GroupName);
                ls.Add("%" + options.GroupNameS + "%");
            }


            sb.AppendFormat(@" order by #{0} asc", T_MGroup_Description.GroupId);

            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;

            int recordCount, pageCount;
            var datas = map.SelectSplit(null, sb.ToString(), ls.ToArray(), false, pageIndex, pageSize, out pageCount, out recordCount);

            if (datas.Count == 0)
            {
                return new PageListModel<T_MGroup>(pageSize);
            }
            return new PageListModel<T_MGroup>(datas, pageSize, pageIndex, recordCount, pageCount);
        }

        public static bool Group_Delete(int groupId)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            return map.DeleteById(groupId) > 0;
        }

        public static bool Is_GroupName_Unique_Add(string groupName)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MGroup_Description.GroupName, groupName);
            return map.IsExistByFields(dic);
        }

        public static bool Is_GroupName_Unique_Modify(int groupId, string groupName)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MGroup_Description.GroupName, groupName);
            return map.IsExistByFields_NotEqualByField(dic, T_MGroup_Description.GroupId, groupId);
        }

        public static bool Is_GroupId_Unique_Add(int groupId)
        {
            var map = SingletonHelper<ModelDAL<T_MGroup>>.Instance.Mapping;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_MGroup_Description.GroupId, groupId);
            return map.IsExistByFields(dic);
        }
    }
}
