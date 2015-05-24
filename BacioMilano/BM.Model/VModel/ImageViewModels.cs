using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Model.VModel
{
    public class ImageSearchModel
    {
        [Display(Name = "图片名称")]
        public string ImageNameS { get; set; }
    }

    public class ImageAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 1)]
        [Display(Name = "图片名称")]
        public string ImageName { get; set; }

        
    }

    public class ImageModifyModel:ImageAddModel
    {
        public long? ImageId { get; set; }

        public long? UserId { get; set; }
    }
}
