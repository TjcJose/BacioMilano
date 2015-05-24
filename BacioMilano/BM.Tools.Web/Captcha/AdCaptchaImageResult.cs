using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Tools.Web.Captcha
{
    internal class AdCaptchaImageResult : ActionResult
    {
        private AdCaptchaImageOption imgOpt;
        public AdCaptchaImageResult( AdCaptchaImageOption imgOpt)
        {
            this.imgOpt = imgOpt;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (imgOpt == null)
            {
                context.HttpContext.Response.StatusCode = 404;
                context.HttpContext.Response.StatusDescription = "Not Found";
                context.HttpContext.Response.End();
                return;
            }

            AdCaptchaImage captchImage = new AdCaptchaImage(this.imgOpt);
            using (var b = captchImage.RenderImage())
            {
                b.Save(context.HttpContext.Response.OutputStream, ImageFormat.Gif);
            }
            context.HttpContext.Response.Cache.SetNoStore();
            context.HttpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            context.HttpContext.Response.ContentType = "image/gif";
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.StatusDescription = "OK";
            context.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}
