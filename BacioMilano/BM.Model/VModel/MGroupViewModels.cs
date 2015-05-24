using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace BM.Model.VModel
{

    public class MGroupSearchModel
    {
        [Display(Name = "组名称")]
        public string GroupNameS { get; set; }

        [Display(Name = "组编号")]
        public int? GroupIdS { get; set; }
    }

    public class MGroupAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "组名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Group_AddCheckGroupNameUnique", "Sys", ErrorMessage = "该组名称已经存在")]
        public virtual string GroupName
        {
            get;
            set;
        }


        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "组编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        [Remote("Group_AddCheckGroupIdUnique", "Sys", ErrorMessage = "该组编号已经存在")]
        public virtual int GroupId
        {
            get;
            set;
        }

        [Display(Name = "描述")]
        [StringLength(200, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        public string GroupMemo
        {
            get;
            set;
        }
    }

    public class MGroupModifyModel : MGroupAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "组名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Remote("Group_ModifyCheckGroupNameUnique", "Sys", AdditionalFields = "GroupId", ErrorMessage = "该组名称已经存在")]
        public override string GroupName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "组编号")]
        [RegularExpression(BM.Util.RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        public override int GroupId
        {
            get;
            set;
        }
    }

}
