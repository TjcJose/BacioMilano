using BM.DA;
using BM.Model.DbModel;
using BM.Model.VModel;
using BM.Util;
using DotLiquid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    public static class BllTemplate
    {
        internal static MappingBase<T_Template> map = SingletonHelper<ModelDAL<T_Template>>.Instance.Mapping;

        public static int GetNewTemplateId()
        {
            var map = SingletonHelper<ModelDAL<T_Template>>.Instance.Mapping;
            return map.SelectStatisticInt(null, null, StatisticType.Max, T_Template_Description.TemplateId) + 1;
        }

        public static T_Template GetById(int templateId)
        {
            return map.GetEntityById(templateId);
        }

        public static int Add(T_Template entity)
        {
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistById(entity.TemplateId.Value, conn, ts))
                    {
                        return ConstResult.template_exist_id;
                    }
                    if (map.IsExistByField(T_Template_Description.TemplateName, entity.TemplateName, conn, ts))
                    {
                        return ConstResult.template_exist_name;
                    }

                    if (map.Insert(entity, conn, ts) > 0)
                    {
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static int Modify(T_Template entity)
        {
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    if (map.IsExistByField_NotEqualByField(T_Template_Description.TemplateName, entity.TemplateName, T_Template_Description.TemplateId, entity.TemplateId.Value, conn, ts))
                    {
                        return ConstResult.template_exist_name;
                    }

                    if (map.Update(entity, conn, ts) > 0)
                    {
                        ts.Commit();
                        return ConstResult.success;
                    }
                    return ConstResult.fail;
                }
            }
        }

        public static PageListModel<T_Template> Search(TemplateSearchModel options, int pageSize, int pageIndex)
        {
            List<object> ls = new List<object>();
            StringBuilder sb = new StringBuilder("1=1");

            if (options.TemplateIdS != null)
            {
                sb.AppendFormat(@" and #{0} = @{0}", T_Template_Description.TemplateId);
                ls.Add(options.TemplateIdS.Value);
            }
            if (!String.IsNullOrWhiteSpace(options.TemplateNameS))
            {
                sb.AppendFormat(@" and #{0} like @{0}", T_Template_Description.TemplateName);
                ls.Add("%" + options.TemplateNameS + "%");
            }


            sb.AppendFormat(@" order by #{0} asc", T_Template_Description.TemplateId);

            int recordCount, pageCount;
            var datas = map.SelectSplit(null, sb.ToString(), ls.ToArray(), false, pageIndex, pageSize, out pageCount, out recordCount);

            if (datas.Count == 0)
            {
                return new PageListModel<T_Template>(pageSize);
            }
            return new PageListModel<T_Template>(datas, pageSize, pageIndex, recordCount, pageCount);
        }

        public static bool Delete(int templateId)
        {
            if (map.DeleteById(templateId) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Is_TemplateName_Unique_Add(string templateName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_Template_Description.TemplateName, templateName);
            return map.IsExistByFields(dic);
        }

        public static bool Is_TemplateName_Unique_Modify(int templateId, string templateName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_Template_Description.TemplateName, templateName);
            return map.IsExistByFields_NotEqualByField(dic, T_Template_Description.TemplateId, templateId);
        }

        public static bool Is_TemplateId_Unique_Add(int templateId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(T_Template_Description.TemplateId, templateId);
            return map.IsExistByFields(dic);
        }
    }
}
