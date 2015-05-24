using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Tools.Web.Captcha
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AdCaptchaValidateAttribute : ActionFilterAttribute
    {
        /// <param name="id">验证控件Id</param>
        public AdCaptchaValidateAttribute(string id = AdCaptchaHelper.AdCaptchaId)
        {
            this.id = id;
        }

        public string id { get; private set; }

        /// <summary> 
        /// Called when [action executed]. 
        /// </summary> 
        /// <param name="filterContext">The filter filterContext.</param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string inputCtrlId = AdCaptchaHelper.Get_InputCtrlId(id);
            string hidValue = filterContext.HttpContext.Request.Form[AdCaptchaHelper.Get_HiddenCtrlId(id)];
            string value = filterContext.HttpContext.Request.Form[inputCtrlId];
            var confirmValue = AdCaptchaHelper.GetCaptchaFromHiddenValue(hidValue);
            if(confirmValue == AdCaptchaHelper.AdCaptchaOver)
            {
                ((Controller)filterContext.Controller).ModelState.AddModelError(inputCtrlId, "验证码已过期");
            }
            else
            {
                var isValid = !String.IsNullOrEmpty(value) && string.Equals(confirmValue, value, StringComparison.OrdinalIgnoreCase);
                if(!isValid)
                {
                    ((Controller)filterContext.Controller).ModelState.AddModelError(inputCtrlId, "验证码不匹配");
                }
            }
        }
    }
}
