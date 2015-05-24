using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BM.Fw;
using System.Collections;

namespace BM.Web.Controllers
{
    public class UploadController:BaseSysController
    {
        [Authorize]
        public ActionResult UploadImage()
        {
            var img = BllImage.Add(0, "", this.Request.Files[0]);
            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = BllUpload.GetUpload_Image_Url(img);
            return Json(hash);
        }
    }
}