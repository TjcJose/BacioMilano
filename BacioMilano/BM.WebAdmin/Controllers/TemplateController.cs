using BM.Fw;
using BM.Model.DbModel;
using BM.Model.VModel;
using BM.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BM.Web.Controllers
{
    [SysAuthorize()]
    public class TemplateController : BaseSysController
    {
        [SysViewRight(ConstSysFun.Template, ConstSysOper.View)]
        public ActionResult Template()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Template, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Template, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Template, ConstSysOper.Delete);
            return this.PartialView();
        }


        [HttpPost]
        public ActionResult Search(TemplateSearchModel model, int pageSizeS, int pageIndexS)
        {
            var data = BllTemplate.Search(model, pageSizeS, pageIndexS);
            return this.Json(data);
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult AddCheckTemplateNameUnique(string templateName)
        {
            bool isValidate = false;
            if (!BllTemplate.Is_TemplateName_Unique_Add(templateName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult AddCheckTemplateIdUnique(int templateId)
        {
            bool isValidate = false;
            if (!BllTemplate.Is_TemplateId_Unique_Add(templateId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult ModifyCheckTemplateNameUnique(int templateId, string templateName)
        {
            bool isValidate = false;
            if (!BllTemplate.Is_TemplateName_Unique_Modify(templateId, templateName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        [SysViewRight(ConstSysFun.Template, ConstSysOper.Add)]
        public ActionResult Add()
        {
            var model = new TemplateAddModel();
            model.TemplateId = BllTemplate.GetNewTemplateId();
            return this.PartialView(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TemplateAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Template, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_Template();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllTemplate.Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.template_exist_id == result)
                {
                    ModelState.AddModelError(T_Template_Description.TemplateId, "该模板编号已存在");
                }
                else if (ConstResult.template_exist_name == result)
                {
                    ModelState.AddModelError(T_Template_Description.TemplateName, "该模板名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.Template, ConstSysOper.Modify)]
        public ActionResult Modify(int id)
        {
            var entity = BllTemplate.GetById(id);
            var model = new TemplateModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);

            return this.PartialView(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(TemplateModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Template, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_Template();
                ObjectConvert.ConvertAToB(model, ref entity);
                ObjectConvert.SetPropertiesStringVal_NullToStr(entity, "");
                var result = BllTemplate.Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.template_exist_id == result)
                {
                    ModelState.AddModelError(T_Template_Description.TemplateId, "该模板编号已存在");
                }
                else if (ConstResult.template_exist_name == result)
                {
                    ModelState.AddModelError(T_Template_Description.TemplateName, "该模板名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!this.IsOperRight(ConstSysFun.Template, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            try
            {
                var result = BllTemplate.Delete(id);
                if (result)
                {
                    return this.Json(new ResultU(true, 0, "删除成功"));
                }
                return this.Json(new ResultU(false, ConstResult.fail, "删除失败"));
            }
            catch (Exception ex)
            {
                return this.Json(new ResultU(false, ConstResult.fail, ex.Message));
            }
        }
    }
}


