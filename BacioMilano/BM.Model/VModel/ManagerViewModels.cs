using System;
using System.ComponentModel.DataAnnotations;

namespace BM.Model.VModel
{
    public class ManagerLoginModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "用户名称")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "验证码")]
        [StringLength(4, ErrorMessage = "字符数必须是{2}个", MinimumLength = 4)]
        public string AdCaptcha_input { get; set; }
    }

    public class ManagerSearchModel
    {
        [Display(Name = "姓名")]
        public string TrueNameS { get; set; }

        [Display(Name = "用户名")]
        public string UserNameS { get; set; }
    }

    public class ManagerAddModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "姓名")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        public string TrueName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "用户名")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        [System.Web.Mvc.Remote("Manager_AddCheckUserNameUnique", "Sys", ErrorMessage = "该用户名已存在")]//ActionName,Controller,错误信息
        public virtual string UserName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "密码")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        public string UserPwd
        {
            get;
            set;
        }
    }

    public class ManagerModifyModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "姓名")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 2)]
        public string TrueName
        {
            get;
            set;
        }

        [Display(Name = "用户名")]
        public  string UserName
        {
            get;
            set;
        }

        public long ManagerId
        {
            get;
            set;
        }
    }

    public class ManagerModifyPwdModel
    {
        [Display(Name = "姓名")]
        public string TrueName
        {
            get;
            set;
        }

        [Display(Name = "用户名")]
        public string UserName
        {
            get;
            set;
        }

        public long ManagerId
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "新密码")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        public string NewPwd
        {
            get;
            set;
        }
    }


    public class ManagerChangePwdModel
    {
        [Required(ErrorMessage = "必须输入")]
        [Display(Name = "用户名")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [DataType(DataType.Password)]
        [Display(Name = "原密码")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        public string OldPwd { get; set; }

        [Required(ErrorMessage = "必须输入")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [StringLength(20, ErrorMessage = "字符数必须在 {2} - {1} 个之间。", MinimumLength = 5)]
        public string NewPwd { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("NewPwd", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPwd { get; set; }
    }
}
