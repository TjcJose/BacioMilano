using BM.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.VModel
{
    public class MenuAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "菜单编号")]
        [RegularExpression(RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        [System.Web.Mvc.Remote("Menu_Is_MenuId_Unique_Add", "Sys", AdditionalFields = "ParentId", ErrorMessage = "该菜单编号已经存在")]
        public virtual int MenuId { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "上级分类编号")]
        [RegularExpression(RegexHelper.reg_PositiveIntegerNumberAndZero_str, ErrorMessage = "请输入正整数")]
        public int ParentId { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "菜单名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [System.Web.Mvc.Remote("Menu_Is_MenuName_Unique_Add", "Sys", AdditionalFields = "MenuId,ParentId", ErrorMessage = "该菜单名称已经存在")]
        public virtual String MenuName { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "菜单排序")]
        [RegularExpression(RegexHelper.reg_IntegerNumber_str, ErrorMessage = "请输入整数")]
        public int MenuSort { get; set; }

        [Required(ErrorMessage = "必须选择")]
        [Display(Name = "菜单类型")]
        public int MenuType { get; set; }

        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Display(Name = "活动名称")]
        public String ActionName { get; set; }

        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [Display(Name = "控制器名称")]
        public String ControllerName { get; set; }

        [StringLength(128, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 1)]
        [Display(Name = "参数")]
        public String Params { get; set; }

        [StringLength(50, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 1)]
        [Display(Name = "菜单图标")]
        public String Icon { get; set; }
    }


    public class MenuModifyModel : MenuAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "菜单名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        [System.Web.Mvc.Remote("Menu_Is_MenuName_Unique_Modify", "Sys", AdditionalFields = "MenuId,ParentId", ErrorMessage = "该菜单名称已经存在")]
        public override string MenuName { get; set; }

        [Display(Name = "菜单编号")]
        public override int MenuId { get; set; }
    }
}

