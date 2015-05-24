using BM.Fw;
using BM.Model.VModel;
using BM.Tools.Web.Captcha;
using BM.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BM.DA;

namespace BM.Web.Controllers
{
    public class HomeController : BaseSysController
    {
        public ActionResult Index()
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    return this.Redirect("/sys/index");
            //}
            //else
            //{
            //    return this.View("Sys_Login");
            //}
            return this.View("Sys_Login");
        }


        [AllowAnonymous]
        public ActionResult Sys_Login()
        {
            if (this.UseContext.IsLogin)
            {
                FormsAuthentication.SignOut();
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(true)]
        [AdCaptchaValidate()]
        public ActionResult Sys_Login(ManagerLoginModel model)
        {
            string msg = null;
            if (ModelState.IsValid)
            {
                var manager = BllManager.Login(model.UserName, model.Password);
                if (manager == null)
                {
                    msg = "用户名或密码不对";
                }
                else
                {
                    BaseSysController.ContextData datas = new BaseSysController.ContextData
                    {
                        UserId = manager.ManagerId.Value,
                        UserName = model.UserName,
                        Role = (int)Role.Manager
                    };

                    this.UseContext.Sign(datas);
                    return this.Json(new BM.Util.ResultU(true, 0, "登录成功"));
                }
            }

            return this.Json(new ResultU<Dictionary<string, string>>(false, GetModelStateError(ModelState), msg));
        }
    }
}