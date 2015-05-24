using BM.DA;
using BM.Model.BModel;
using BM.Model.DbModel;
using BM.Model.VModel;
using BM.Security;
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
        public static bool IsAdmin(string userName)
        {
            return userName == "admin";
        }

        private static string _Manager_CalculatePwd(string userName, string pwd)
        {
            return EncryptUnRe.Instance.EncryptString(userName + pwd);
        }

        public static T_Manager Login(string userName, string pwd)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            string sPwd = _Manager_CalculatePwd(userName, pwd);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_Manager_Description.UserName, userName);
            dic.Add(T_Manager_Description.UserPwd, sPwd);
            dic.Add(T_Manager_Description.IsUse, true);
            object obj = map.Get(dic, T_Manager_Description.TrueName, T_Manager_Description.UserName, T_Manager_Description.ManagerId);
            if(obj == null)
            {
                return null;
            }
            return (T_Manager)obj;
        }


       
        public static bool ChangePwd(string userName, string oldPwd, string newPwd)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            string sPwdOld = _Manager_CalculatePwd(userName, oldPwd);
            string sPwdNew = _Manager_CalculatePwd(userName, newPwd);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_Manager_Description.UserPwd, sPwdNew);
            string where = String.Format(@"#{0}=@{0} and #{1}=@{1}", T_Manager_Description.UserName, T_Manager_Description.UserPwd);
            object[] values = { userName, sPwdOld };
            return map.Update(dic, where, values) > 0;
        } 

        public static IEnumerable<Right> Right_GetAllByManagerId(long managerId)
        {
            var mapMRight = SingletonHelper<ModelDAL<T_MRight>>.Instance.Mapping;
            var mapMGroupRight = SingletonHelper<ModelDAL<T_MGroupRight>>.Instance.Mapping;
            using (var conn = mapMRight.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    var lsMRight = MRight_SelectByManagerId(managerId, conn, ts);
                    var lsMGroupRight = VMManagerGroup_SelectByManagerId(managerId, conn, ts);

                    var ls = new List<Right>();
                    foreach (var mright in lsMRight)
                    {
                        var data = new Right();
                        data.FunctionId = mright.FunctionId.Value;
                        data.OperationId = mright.OperationId.Value;
                        ls.Add(data);
                    }

                    foreach (var mright in lsMGroupRight)
                    {
                        var data = new Right();
                        data.FunctionId = mright.FunctionId.Value;
                        data.OperationId = mright.OperationId.Value;
                        ls.Add(data);
                    }

                    return ls.Distinct();
                }
            }

        }

        public static IEnumerable<T_MRight> MRight_SelectByManagerId(long managerId, IDbConnection conn, IDbTransaction ts)
        {
            var map = SingletonHelper<ModelDAL<T_MRight>>.Instance.Mapping;
            return map.SelectByProperty(T_MRight_Description.ManagerId, managerId, T_MRight_Description.FunctionId, true, conn, ts, T_MRight_Description.FunctionId, T_MRight_Description.OperationId);
        }

        public static IEnumerable<T_MRight> MRight_SelectByManagerId(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_MRight>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return MRight_SelectByManagerId(managerId, conn, null);
            }
        }

        public static PageListModel<T_Manager> Manager_Search(ManagerSearchModel options, int pageSize, int pageIndex)
        {
            List<object> ls = new List<object>();
            StringBuilder sb = new StringBuilder(String.Format(@"#{0}=@{0}", T_Manager_Description.IsUse));
            ls.Add(true);

            if (!String.IsNullOrWhiteSpace(options.TrueNameS))
            {
                sb.AppendFormat(@" and #{0} like @{0}", T_Manager_Description.TrueName);
                ls.Add("%" + options.TrueNameS + "%");
            }
            if (!String.IsNullOrWhiteSpace(options.UserNameS))
            {
                sb.AppendFormat(@" and #{0} like @{0}", T_Manager_Description.UserName);
                ls.Add("%" + options.UserNameS + "%");
            }

            sb.AppendFormat(@" order by #{0} asc", T_Manager_Description.ManagerId);

            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;

            string[] selectProperties = new string[] { T_Manager_Description.ManagerId, T_Manager_Description.TrueName, T_Manager_Description.UserName };
            int recordCount, pageCount;
            var datas = map.SelectSplit(selectProperties, sb.ToString(), ls.ToArray(), false, pageIndex, pageSize, out pageCount, out recordCount);

            if (datas.Count == 0)
            {
                return new PageListModel<T_Manager>(pageSize);
            }
            return new PageListModel<T_Manager>(datas, pageSize, pageIndex, recordCount, pageCount);
        }

        public static int Manager_Add(ManagerAddModel model)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistByField(T_Manager_Description.UserName, model.UserName, conn, ts))
                    {
                        return ConstResult.manager_exist_userName;
                    }
                    T_Manager entity = new T_Manager();
                    entity.TrueName = model.TrueName;
                    entity.UserName = model.UserName;
                    entity.UserPwd = _Manager_CalculatePwd(model.TrueName, model.UserPwd);
                    entity.ManagerId = BM.Util.ObjectConvert.GetLongValue( map.InsertReturnIdentity(entity, conn, ts));
                    ts.Commit();
                }
            }
            return ConstResult.success;
        }

        public static bool Manager_Modify(ManagerModifyModel model)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            T_Manager entity = new T_Manager();
            entity.TrueName = model.TrueName;
            entity.ManagerId = model.ManagerId;
            return map.Update(entity) > 0;
        }

        public static int Manager_ModifyPwd(long managerId, string newPwd)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    var obj = map.GetById(managerId, conn, ts, T_Manager_Description.UserName);
                    if (obj == null)
                    {
                        return ConstResult.manager_notExist_managerId;
                    }
                    string userName = ((T_Manager)obj).UserName;

                    string sPwdNew = _Manager_CalculatePwd(userName, newPwd);
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add(T_Manager_Description.UserPwd, sPwdNew);
                    string where = String.Format(@"#{0}=@{0}", T_Manager_Description.ManagerId);
                    object[] values = { managerId };
                    if( map.Update(dic, where, values) > 0)
                    {
                        ts.Commit();
                        return ConstResult.success;
                    }
                    
                }
            }
            return ConstResult.fail;
        }

        public static int Manager_Delete(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    var obj = map.GetById(managerId, conn, ts, T_Manager_Description.UserName);
                    if(obj == null)
                    {
                        return ConstResult.manager_notExist_managerId;
                    }
                    string userName = ((T_Manager)obj).UserName;
                    if(IsAdmin(userName))
                    {
                        return ConstResult.manager_delete_admin;
                    }

                    if(map.DeleteById(managerId) > 0)
                    {
                        ts.Commit();
                        return ConstResult.success;
                    }
                }
            }
            return ConstResult.fail;
        }

        public static ManagerModifyModel Manager_Get_ManagerModifyModel(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            var entity = (T_Manager)map.GetById(managerId, T_Manager_Description.TrueName, T_Manager_Description.UserName, T_Manager_Description.ManagerId);
            if(entity == null)
            {
                return null;
            }
            else
            {
                ManagerModifyModel model = new ManagerModifyModel();
                model.ManagerId = entity.ManagerId.Value;
                model.TrueName = entity.TrueName;
                model.UserName = entity.UserName;
                return model;
            }
        }

        public static ManagerModifyPwdModel Manager_Get_ManagerModifyPwdModel(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            var entity = (T_Manager)map.GetById(managerId, T_Manager_Description.TrueName, T_Manager_Description.UserName, T_Manager_Description.ManagerId);
            if (entity == null)
            {
                return null;
            }
            else
            {
                ManagerModifyPwdModel model = new ManagerModifyPwdModel();
                model.ManagerId = entity.ManagerId.Value;
                model.TrueName = entity.TrueName;
                model.UserName = entity.UserName;
                model.NewPwd = "";
                return model;
            }
        }

        public static bool Manager_ExistUserName(string userName)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            return map.IsExistByField(T_Manager_Description.UserName, userName);
        }

        public static string Manager_GetUserName(long managerId)
        {
            var map = SingletonHelper<ModelDAL<T_Manager>>.Instance.Mapping;
            var obj = map.GetById(managerId, T_Manager_Description.UserName);
            if(obj == null)
            {
                return null;
            }
            return ((T_Manager)obj).UserName;
        }
    }
}
