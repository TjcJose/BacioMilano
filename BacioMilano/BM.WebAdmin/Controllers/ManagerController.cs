using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ad.Fw;
using Ad.Util;
using Ad.Model.DbModel;
using Ad.Model.VModel;

namespace Ad.Web.Controllers
{
    public class ManagerController : BaseSysController
    {
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            return View();
        }

        #region MemberLevel  会员类型

        //[SysViewRight(ConstSysFun.Function, ConstSysOper.View)]
        public ActionResult MemberLevel()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = true;
            this.ViewBag.isModify = true;
            this.ViewBag.isDelete = true;
            //this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Add);
            //this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Modify);
            //this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Delete);
            return this.PartialView();
        }

        public ActionResult MemberLevel_All()
        {
            var datas = BllMemberLevel.MemberLevel_SelectAll();
            return this.Json(datas);
            //List<T_MemberLevel> ls = new List<T_MemberLevel>();
            //foreach (var data in datas)
            //{
            //    ls.Add(data);
            //    foreach (var d in data.Sons)
            //    {
            //        ls.Add(d);
            //    }
            //}
            //return this.Json(ls);
        }

        public ActionResult MemberLevel_Add()
        {
            var model = new MemberLevelAddModel();
            model.MemberLevelId = BllMemberLevel.MemberLevel_GetNewLevelId();
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberLevel_Add(MemberLevelAddModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new T_MemberLevel();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllMemberLevel.MemberLevel_Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.memberlevel_exist_id == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelId, "会员类型编号已存在");
                }
                else if (ConstResult.memberlevel_exist_name == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelName, "会员类型名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpPost]
        public ActionResult MemberLevel_Delete(int id)
        {
            try
            {
                var result = BllMemberLevel.MemberLevel_Delete(id);
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

        [HttpGet]
        public ActionResult MemberLevel_Modify(int id)
        {
            var entity = BllMemberLevel.MemberLevel_GetById(id);
            var model = new MemberLevelModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberLevel_Modify(MemberLevelModifyModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new T_MemberLevel();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllMemberLevel.MemberLevel_Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.memberlevel_exist_name == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelName, "该会员类型名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }


        public JsonResult MemberType_AddCheckLevelNameUnique(string MemberLevelName)
        {
            bool isValidate = false;
            if (!BllMemberLevel.Function_Is_MemberLevelName_Unique_Add(MemberLevelName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        public JsonResult MemberType_AddCheckLevelIdUnique(int MemberLevelId)
        {
            bool isValidate = false;
            if (!BllMemberLevel.Function_Is_MemberLevelId_Unique_Add(MemberLevelId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        public JsonResult MemberType_ModifyCheckLevelNameUnique(int MemberLevelId, string MemberLevelName)
        {
            bool isValidate = false;
            if (!BllMemberLevel.MemberType_Is_LevelName_Unique_Modify(MemberLevelName, MemberLevelId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }
        #endregion 

        #region 顾客类型
        public ActionResult CustomerType()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = true;
            this.ViewBag.isModify = true;
            this.ViewBag.isDelete = true;
            //this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Add);
            //this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Modify);
            //this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Delete);
            return this.PartialView();
        }

        public ActionResult CustomerType_All()
        {
            var datas = BllMemberLevel.CustomerType_SelectAll();
            return this.Json(datas);
            //List<T_MemberLevel> ls = new List<T_MemberLevel>();
            //foreach (var data in datas)
            //{
            //    ls.Add(data);
            //    foreach (var d in data.Sons)
            //    {
            //        ls.Add(d);
            //    }
            //}
            //return this.Json(ls);
        }

        public ActionResult CustomerType_Add()
        {
            var model = new CustomerTypeAddModel();
            model.CustomerTypeId = BllMemberLevel.CustomerType_GetNewLevelId();
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerType_Add(CustomerTypeAddModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new T_CustomerType();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllMemberLevel.CustomerType_Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.customertype_exist_id == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelId, "客户类型编号已存在");
                }
                else if (ConstResult.customertype_exist_name == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelName, "客户类型名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpPost]
        public ActionResult CustomerType_Delete(int id)
        {
            try
            {
                var result = BllMemberLevel.CustomerType_Delete(id);
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

        [HttpGet]
        public ActionResult CustomerType_Modify(int id)
        {
            var entity = BllMemberLevel.CustomerType_GetById(id);
            var model = new CustomerTypeModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerType_Modify(CustomerTypeModifyModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new T_CustomerType();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllMemberLevel.CustomerType_Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.customertype_exist_name == result)
                {
                    ModelState.AddModelError(T_MemberLevel_Description.MemberLevelName, "该客户类型名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }
        #endregion

    }
}