using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Model.VModel
{
    public class FunctionAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "功能名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Function_AddCheckFunctionNameUnique", "Sys", ErrorMessage = "该名称已经存在")]
        public virtual string FunctionName
        {
            get;
            set;
        }




        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "功能编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumber_str, ErrorMessage = "请输入正整数")]
        [Remote("Function_AddCheckFunctionIdUnique", "Sys", ErrorMessage = "该功能编号已经存在")]
        public virtual int FunctionId
        {
            get;
            set;
        }
    }

    public class FunctionModifyModel : FunctionAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "功能名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Function_ModifyCheckFunctionNameUnique", "Sys", AdditionalFields = "FunctionId", ErrorMessage = "该功能名称已经存在")]
        public override string FunctionName
        {
            get;
            set;
        }

        [Display(Name = "功能编号")]
        public override int FunctionId
        {
            get;
            set;
        }


    }
}
