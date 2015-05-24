using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Model.VModel
{
    public class TemplateSearchModel
    {
        [Display(Name = "模板名称")]
        public string TemplateNameS { get; set; }

        [Display(Name = "模板编号")]
        public int? TemplateIdS { get; set; }
    }

    public class TemplateAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "模板名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("AddCheckTemplateNameUnique", "Template", ErrorMessage = "该模板名称已经存在")]
        public virtual string TemplateName
        {
            get;
            set;
        }


        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "模板编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        [Remote("AddCheckTemplateIdUnique", "Template", ErrorMessage = "该模板编号已经存在")]
        public virtual int TemplateId
        {
            get;
            set;
        }

        [AllowHtml]
        [Display(Name = "模板内容")]
        [StringLength(30000, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        public string TemplateContent
        {
            get;
            set;
        }
    }

    public class TemplateModifyModel : TemplateAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "模板名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("ModifyCheckTemplateNameUnique", "Template", AdditionalFields = "TemplateId", ErrorMessage = "该模板名称已经存在")]
        public override string TemplateName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "模板编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        public override int TemplateId
        {
            get;
            set;
        }
    }
}
