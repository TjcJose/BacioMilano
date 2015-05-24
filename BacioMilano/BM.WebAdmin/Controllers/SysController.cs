using BM.Fw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BM.Model.VModel;
using BM.Util;
using BM.Model.DbModel;
using System.Web.UI;

namespace BM.Web.Controllers
{

    [SysAuthorize()]
    public class SysController : BaseSysController
    {
        [Authorize]
        public ActionResult Index()
        {
            var data = this.UseContext.Data;
            if(BllManager.IsAdmin(data.UserName))
            {
                return this.View(BllManager.Menu_SelectByManagerId());
            }
            else
            {
                return this.View(BllManager.Menu_SelectByManagerId(data.UserId, data.Get_Rights()));
            }
        }

        public ActionResult Home()
        {
            return this.PartialView();
        }

        #region Function

        [HttpPost]
        [SysViewRight(ConstSysFun.Function, ConstSysOper.View)]
        public ActionResult Function_All()
        {
            var data = BllManager.Function_SelectAll();
            return this.Json(data);
        }

        [SysViewRight(ConstSysFun.Function, ConstSysOper.View)]
        public ActionResult Function()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Function, ConstSysOper.Delete);
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult Function_Delete(int id)
        {
            if (!this.IsOperRight(ConstSysFun.Function, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            try
            {
                var result = BllManager.Function_Delete(id);
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
        [SysViewRight(ConstSysFun.Function, ConstSysOper.Add)]
        public ActionResult Function_Add()
        {
            var model = new FunctionAddModel();
            model.FunctionId = BllManager.Function_GetNewFunctionId();
            return this.PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Function_Add(FunctionAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Function, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MFunction();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllManager.Function_Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.function_exist_id == result)
                {
                    ModelState.AddModelError(T_MFunction_Description.FunctionId, "该功能编号已存在");
                }
                else if (ConstResult.function_exist_name == result)
                {
                    ModelState.AddModelError(T_MFunction_Description.FunctionName, "该功能名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.Function, ConstSysOper.Modify)]
        public ActionResult Function_Modify(int id)
        {
            var entity = BllManager.Function_GetById(id);
            var model = new FunctionModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Function_Modify(FunctionModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Function, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MFunction();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllManager.Function_Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.function_exist_name == result)
                {
                    ModelState.AddModelError(T_MFunction_Description.FunctionName, "该功能名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Function_AddCheckFunctionNameUnique(string FunctionName)
        {
            bool isValidate = false;
            if (!BllManager.Function_Is_FunctionName_Unique_Add(FunctionName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Function_AddCheckFunctionIdUnique(int FunctionId)
        {
            bool isValidate = false;
            if (!BllManager.Function_Is_FunctionId_Unique_Add(FunctionId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Function_ModifyCheckFunctionNameUnique(int FunctionId, string FunctionName)
        {
            bool isValidate = false;
            if (!BllManager.Function_Is_FunctionName_Unique_Modify(FunctionName, FunctionId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Operation

        [HttpPost]
        [SysViewRight(ConstSysFun.Operation, ConstSysOper.View)]
        public ActionResult Operation_All()
        {
            var data = BllManager.Operation_SelectAll();
            return this.Json(data);
        }

        [SysViewRight(ConstSysFun.Operation, ConstSysOper.View)]
        public ActionResult Operation()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Operation, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Operation, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Operation, ConstSysOper.Delete);
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult Operation_Delete(int id)
        {
            if (!this.IsOperRight(ConstSysFun.Operation, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            try
            {
                var result = BllManager.Operation_Delete(id);
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
        [SysViewRight(ConstSysFun.Operation, ConstSysOper.Add)]
        public ActionResult Operation_Add()
        {
            var model = new OperationAddModel();
            model.OperationId = BllManager.Operation_GetNewOperationId();
            return this.PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Operation_Add(OperationAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Operation, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MOperation();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllManager.Operation_Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.operation_exist_id == result)
                {
                    ModelState.AddModelError(T_MOperation_Description.OperationId, "该操作编号已存在");
                }
                else if (ConstResult.operation_exist_name == result)
                {
                    ModelState.AddModelError(T_MOperation_Description.OperationName, "该操作名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.Operation, ConstSysOper.Modify)]
        public ActionResult Operation_Modify(int id)
        {
            var entity = BllManager.Operation_GetById(id);
            var model = new OperationModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Operation_Modify(OperationModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Operation, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MOperation();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllManager.Operation_Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.operation_exist_name == result)
                {
                    ModelState.AddModelError(T_MOperation_Description.OperationName, "该操作名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Operation_AddCheckOperationNameUnique(string OperationName)
        {
            bool isValidate = false;
            if (!BllManager.Operation_Is_OperationName_Unique_Add(OperationName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Operation_AddCheckOperationIdUnique(int OperationId)
        {
            bool isValidate = false;
            if (!BllManager.Operation_Is_OperationId_Unique_Add(OperationId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Operation_ModifyCheckOperationNameUnique(int OperationId, string OperationName)
        {
            bool isValidate = false;
            if (!BllManager.Operation_Is_OperationName_Unique_Modify(OperationName, OperationId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region MenuFunOper



        [SysViewRight(ConstSysFun.MenuFunOper, ConstSysOper.View)]
        [HttpPost]
        public ActionResult MenuFunOper_FunOpers(int id)
        {
            var all = BllManager.MenuFunOper_SelectAll();
            var data = all.Where(f => f.MenuId.Value == id).Select(f => f);
            return Json(data);
        }

        [HttpPost]
        public ActionResult MenuFunOper_Set(int id, string data)
        {
            if (!this.IsOperRight(ConstSysFun.MenuFunOper, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                Exception ex = null;
                if (BllManager.MenuFunOper_Set(id, data, ref ex))
                {
                    return this.Json(new ResultU(true, 0, "保存设置成功"));
                }
                else
                {
                    return this.Json(new ResultU(false, -1, ex.Message));
                }
            }

            return this.Json(new ResultU(false, -1, "保存设置失败"));
        }

        [SysViewRight(ConstSysFun.MenuFunOper, ConstSysOper.View)]
        public ActionResult MenuFunOper()
        {
            this.ViewBag.isModify = this.IsOperRight(ConstSysFun.MenuFunOper, ConstSysOper.Modify);
            this.ViewBag.Funs = BllManager.Function_SelectAll();
            return this.PartialView(BllManager.VFunOper_SelectAll());
        }

        #endregion

        #region Menu

        [SysViewRight(ConstSysFun.Menu, ConstSysOper.View)]
        [HttpPost]
        public ActionResult Menu_SelectByParentId(int id)
        {
            var allMenu = BllManager.Menu_SelectAll();
            return this.Json(allMenu.Where(f => f.ParentId.Value == id).Select(f => f));
        }

        [SysViewRight(ConstSysFun.Menu, ConstSysOper.View)]
        public ActionResult Menu()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Delete);
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult Menu_Delete(int id)
        {
            if (!this.IsOperRight(ConstSysFun.Menu, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            try
            {
                var result = BllManager.Menu_Delete(id);
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

        [SysViewRight(ConstSysFun.Menu, ConstSysOper.View)]
        public ActionResult Menu_All()
        {
            var datas = BllManager.Menu_SelectByManagerId(0, null, true);
            List<T_MMenu> ls = new List<T_MMenu>();
            foreach(var data in datas)
            {
                ls.Add(data);
                foreach (var d in data.Sons)
                {
                    ls.Add(d);
                }
            }
            return this.Json(ls);
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.Menu, ConstSysOper.Add)]
        public ActionResult Menu_Add(int id)
        {
            var model = new MenuAddModel();
            model.MenuId = BllManager.Menu_GetNewMenuId(); 
            model.ParentId = id;
            model.MenuSort = BllManager.Menu_GetNewMenuSort(id);
            return this.PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Menu_Add(MenuAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Menu, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MMenu();
                ObjectConvert.ConvertAToB(model, ref entity);
                var result = BllManager.Menu_Add(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.menu_exist_id == result)
                {
                    ModelState.AddModelError(T_MMenu_Description.MenuId, "该菜单编号已存在");
                }
                else if (ConstResult.menu_exist_name == result)
                {
                    ModelState.AddModelError(T_MMenu_Description.MenuName, "该菜单名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.Menu, ConstSysOper.Modify)]
        public ActionResult Menu_Modify(int id)
        {
            var entity = BllManager.Menu_GetById(id);
            var model = new MenuModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            this.ViewBag.HasSon = BllManager.Menu_HasSon(id);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Menu_Modify(MenuModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Menu, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var entity = new T_MMenu();
                ObjectConvert.ConvertAToB(model, ref entity);
                ObjectConvert.SetPropertiesStringVal_NullToStr(entity, "");
               
                var result = BllManager.Menu_Modify(entity);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.menu_exist_name == result)
                {
                    ModelState.AddModelError(T_MMenu_Description.MenuName, "该菜单名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }
       
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Menu_Is_MenuName_Unique_Modify(string menuName, int menuId, int parentId)
        {
            bool isValidate = false;
            if (!BllManager.Menu_Is_MenuName_Unique_Modify(menuName, menuId, parentId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Menu_Is_MenuId_Unique_Add(int menuId)
        {
            bool isValidate = false;
            if (!BllManager.Menu_Is_MenuId_Unique_Add(menuId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Menu_Is_MenuName_Unique_Add(int menuId, string menuName, int parentId)
        {
            bool isValidate = false;
            if (!BllManager.Menu_Is_MenuName_Unique_Add(menuName, parentId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }
        
        #endregion

        #region FunOper
        [SysViewRight(ConstSysFun.FunOper, ConstSysOper.View)]
        public ActionResult FunOper()
        {
            this.ViewBag.isModify = this.IsOperRight(ConstSysFun.FunOper, ConstSysOper.Modify);
            this.ViewBag.Funs = BllManager.Function_SelectAll();
            this.ViewBag.Opers = BllManager.Operation_SelectAll();
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult FunOper(int id, string data)
        {
            if (!this.IsOperRight(ConstSysFun.FunOper, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                Exception ex = null;
                if (BllManager.FunOper_Set(id, data, ref ex))
                {
                    return this.Json(new ResultU(true, 0, "保存设置成功"));
                }
                else
                {
                    return this.Json(new ResultU(false, -1, ex.Message));
                }
            }

            return this.Json(new ResultU(false, -1, "保存设置失败"));
        }

        [HttpPost]
        public ActionResult FunOper_Operations(int id)
        {
           var datas = BllManager.FunOper_SelectByFunctionId(id);
           return this.Json(datas);
        }

        #endregion

        #region Manager

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.View)]
        public ActionResult Manager()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Delete);
            this.ViewBag.isChangePwd = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.ChangePwd);
            this.ViewBag.isRight = this.IsOperRight(data, ConstSysFun.Manager, ConstSysOper.Right);
            this.ViewBag.isUserGroup = this.IsOperRight(data, ConstSysFun.MGroup, ConstSysOper.UserGroup);
            return this.PartialView();
        }

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.UserGroup)]
        public ActionResult Manager_Group(long id)
        {
            this.ViewBag.ManagerId = id;
            this.ViewBag.GroupIds = BllManager.Group_SelectGroupIdByManagerId(id);
            return this.PartialView(BllManager.Group_All());
        }

        [HttpPost]
        public ActionResult Manager_Group(long id, string data)
        {
            if (!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.UserGroup))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                if (BllManager.Group_UserGroupsSet(id, data))
                {
                    return this.Json(new ResultU(true, 0, "保存设置成功"));
                }
            }

            return this.Json(new ResultU(false, -1, "保存设置失败，请检查表之间的约束关系。"));
        }


        [HttpPost]
        public ActionResult Manager_Search(ManagerSearchModel model, int pageSizeS, int pageIndexS)
        {
            var data = BllManager.Manager_Search(model, pageSizeS, pageIndexS);
            return this.Json(data);
        }

        [HttpGet]
        public ActionResult Manager_ChangeSelfPwd()
        {
            var model = new ManagerChangePwdModel();
            model.UserName = this.User.Identity.Name;
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager_ChangeSelfPwd(ManagerChangePwdModel model)
        {
            if (ModelState.IsValid)
            {
                if(BllManager.ChangePwd(model.UserName, model.OldPwd, model.NewPwd))
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else
                {
                    ModelState.AddModelError("OldPwd", "原密码错误");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.Add)]
        [HttpGet]
        public ActionResult Manager_Add()
        {
            var model = new ManagerAddModel();
            return this.PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager_Add(ManagerAddModel model)
        {
            if(!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.Add) )
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var result = BllManager.Manager_Add(model);
                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if(ConstResult.manager_exist_userName == result)
                {
                    ModelState.AddModelError(T_Manager_Description.UserName, "该用户名已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        public ActionResult Manager_Addx()
        {
            var model = new ManagerAddModel();
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager_Addx(ManagerAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var result = BllManager.Manager_Add(model);
                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.manager_exist_userName == result)
                {
                    ModelState.AddModelError(T_Manager_Description.UserName, "该用户名已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }



        [SysViewRight(ConstSysFun.Manager, ConstSysOper.Modify)]
        [HttpGet]
        public ActionResult Manager_Modify(long id)
        {
            var model = BllManager.Manager_Get_ManagerModifyModel(id);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager_Modify(ManagerModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }
            if (BllManager.IsAdmin(model.UserName) && !BllManager.IsAdmin(this.UseContext.Data.UserName))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                if(BllManager.Manager_Modify(model))
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.ChangePwd)]
        [HttpGet]
        public ActionResult Manager_ChangePwd(long id)
        {
            var model = BllManager.Manager_Get_ManagerModifyPwdModel(id);
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager_ChangePwd(ManagerModifyPwdModel model)
        {
            if (!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.ChangePwd))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }
            if (BllManager.IsAdmin(model.UserName) && !BllManager.IsAdmin(this.UseContext.Data.UserName))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var result = BllManager.Manager_ModifyPwd(model.ManagerId, model.NewPwd);
                if (result == ConstResult.success)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if(result == ConstResult.manager_exist_userName)
                {
                    ModelState.AddModelError("UserName", "此操作员不存在");
                }
                else
                {
                    return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState), "修改密码失败"));
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpPost]
        public ActionResult Manager_Delete(long id)
        {
            if (!this.IsOperRight(ConstSysFun.Manager, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            var result = BllManager.Manager_Delete(id);
            if(result == ConstResult.success)
            {
                return this.Json(new ResultU(true, result, "删除成功"));
            }
            else if(result == ConstResult.manager_delete_admin)
            {
                return this.Json(new ResultU(false, result, "不能删除超级管理员"));
            }
            else if(result == ConstResult.manager_notExist_managerId)
            {
                return this.Json(new ResultU(false, result, "不存在编号为 " + id + " 的管理员"));
            }
            return this.Json(new ResultU(false, result, "删除失败"));
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Manager_AddCheckUserNameUnique(string userName)
        {
            bool isValidate = false;
            if (!BllManager.Manager_ExistUserName(userName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.Right)]
        [HttpGet]
        public ActionResult Manager_Right(long id)
        {
            this.ViewBag.Rights = BllManager.Right_SelectByManagerId(id);
            this.ViewBag.FunOper = BllManager.VFunOper_SelectAll();
            this.ViewBag.UserName = BllManager.Manager_GetUserName(id);
            this.ViewBag.ManagerId = id;
            this.ViewBag.VMManagerGroups = BllManager.VMManagerGroup_SelectByManagerId(id);
           
            return this.PartialView();
        }

        [SysViewRight(ConstSysFun.Manager, ConstSysOper.Right)]
        [HttpPost]
        public ActionResult Manager_Right(long id, string data)
        {
            if(!string.IsNullOrWhiteSpace(data))
            {
                if(BllManager.Right_Set(id, data))
                {
                    return this.Json(new ResultU(true, 0, "保存设置成功"));
                }
            }

            return this.Json(new ResultU(false, -1, "保存设置失败，请检查表之间的约束关系。"));
        }

        public ActionResult ViewRightError()
        {
            return this.PartialView();
        }

        #endregion

        #region Group

        [SysViewRight(ConstSysFun.MGroup, ConstSysOper.View)]
        public ActionResult Group()
        {
            var data = this.UseContext.Data;
            this.ViewBag.isAdd = this.IsOperRight(data, ConstSysFun.MGroup, ConstSysOper.Add);
            this.ViewBag.isModify = this.IsOperRight(data, ConstSysFun.MGroup, ConstSysOper.Modify);
            this.ViewBag.isDelete = this.IsOperRight(data, ConstSysFun.MGroup, ConstSysOper.Delete);
            this.ViewBag.isRight = this.IsOperRight(data, ConstSysFun.MGroup, ConstSysOper.Right);
           
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult Group_Search(MGroupSearchModel model, int pageSizeS, int pageIndexS)
        {
            var data = BllManager.Group_Search(model, pageSizeS, pageIndexS);
            return this.Json(data);
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Group_AddCheckGroupNameUnique(string GroupName)
        {
            bool isValidate = false;
            if (!BllManager.Is_GroupName_Unique_Add(GroupName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Group_AddCheckMGroupIdUnique(int MGroupId)
        {
            bool isValidate = false;
            if (!BllManager.Is_GroupId_Unique_Add(MGroupId))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult Group_ModifyCheckMGroupNameUnique(int MGroupId, string MGroupName)
        {
            bool isValidate = false;
            if (!BllManager.Is_GroupName_Unique_Modify(MGroupId, MGroupName))
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        [SysViewRight(ConstSysFun.MGroup, ConstSysOper.Add)]
        public ActionResult Group_Add()
        {
            var model = new MGroupAddModel();
            return this.PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Group_Add(MGroupAddModel model)
        {
            if (!this.IsOperRight(ConstSysFun.MGroup, ConstSysOper.Add))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                var result = BllManager.Group_Add(model);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "添加成功"));
                }
                else if (ConstResult.group_exist_groupId == result)
                {
                    ModelState.AddModelError(T_MGroup_Description.GroupId, "该组编号已存在");
                }
                else if (ConstResult.group_exist_groupName == result)
                {
                    ModelState.AddModelError(T_MGroup_Description.GroupName, "该组名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpGet]
        [SysViewRight(ConstSysFun.MGroup, ConstSysOper.Modify)]
        public ActionResult Group_Modify(int id)
        {
            var entity = BllManager.Group_GetById(id);
            var model = new MGroupModifyModel();
            ObjectConvert.ConvertAToB(entity, ref model);
            
            return this.PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Group_Modify(MGroupModifyModel model)
        {
            if (!this.IsOperRight(ConstSysFun.MGroup, ConstSysOper.Modify))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (ModelState.IsValid)
            {
                ObjectConvert.SetPropertiesStringVal_NullToStr(model, "");
                var result = BllManager.Group_Modify(model);

                if (ConstResult.success == result)
                {
                    return this.Json(new ResultU(true, 0, "修改成功"));
                }
                else if (ConstResult.group_exist_groupId == result)
                {
                    ModelState.AddModelError(T_MGroup_Description.GroupId, "该组编号已存在");
                }
                else if (ConstResult.group_exist_groupName == result)
                {
                    ModelState.AddModelError(T_MGroup_Description.GroupName, "该组名称已存在");
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState)));
        }

        [HttpPost]
        public ActionResult Group_Delete(int id)
        {
            if (!this.IsOperRight(ConstSysFun.MGroup, ConstSysOper.Delete))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            try
            {
                var result = BllManager.Group_Delete(id);
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

        [SysViewRight(ConstSysFun.MGroup, ConstSysOper.Right)]
        [HttpGet]
        public ActionResult Group_Right(int id)
        {
            this.ViewBag.Rights = BllManager.Right_SelectByGroupId(id);
            this.ViewBag.FunOper = BllManager.VFunOper_SelectAll();
            this.ViewBag.GroupId = id;
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult Group_Right(int id, string data)
        {
            if (!this.IsOperRight(ConstSysFun.MGroup, ConstSysOper.Right))
            {
                return this.Json(new ResultU(false, -1, "权限不足"));
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
               if (BllManager.Right_Group_Set(id, data))
               {
                    return this.Json(new ResultU(true, 0, "保存设置成功"));
               }
            }

            return this.Json(new ResultU(false, -1, "保存设置失败，请检查表之间的约束关系。"));
        }


        #endregion

        
    }
}