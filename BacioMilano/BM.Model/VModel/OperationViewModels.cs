using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BM.Model.VModel
{
    public class OperationAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "操作名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Operation_AddCheckOperationNameUnique", "Sys", ErrorMessage = "该名称已经存在")]
        public virtual string OperationName
        {
            get;
            set;
        }



        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "操作编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumber_str, ErrorMessage = "请输入正整数")]
        [Remote("Operation_AddCheckOperationIdUnique", "Sys", ErrorMessage = "此操作编号已经存在")]
        public virtual int OperationId
        {
            get;
            set;
        }
    }

    public class OperationModifyModel : OperationAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "操作名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Operation_ModifyCheckOperationNameUnique", "Sys", AdditionalFields = "OperationId", ErrorMessage = "该操作名称已经存在")]
        public override string OperationName
        {
            get;
            set;
        }

        [Display(Name = "操作编号")]
        public override int OperationId
        {
            get;
            set;
        }
    }
}
