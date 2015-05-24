using BM.Tools.Web.Captcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Tools.Web
{
    public class AdCtrlController:Controller
    {
        public ActionResult CaptchaImage(string v)
        {
            AdCaptchaImageOption imgOpt = AdCaptchaHelper.GetCaptchaImageOptionFromHiddenValue(v);
            return new AdCaptchaImageResult(imgOpt);
        }

        /// <summary>
        /// 获取加密验证码相关隐藏控件值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Captcha_v(string id = AdCaptchaHelper.AdCaptchaId, Style style = Style.Default)
        {
            return this.Content(AdCaptchaHelper.Get_HiddenValue(id, style));
        }
    }
}
