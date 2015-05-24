using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model
{
    public static class ConstAd
    {
        public const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
        public const string DateFormatString = "yyyy-MM-dd";
        public const string PriceFormatString = "f2";
        public const int ForgetPassword_Deadline_Hour = 8;
        public const int Upload_Img_Size = 1024 * 512;
        public const int Upload_CategoryImg_Size = 1024 * 256;
        public const int Upload_SiteImg_Size = 1024 * 64;
        public const int Upload_ProductImg_Size = 1024 * 64;
        public const int Upload_ArticleImg_Size = 1024 * 512;
        public const int Upload_WeiXinArticleImg_Size = 1024 * 128;
        public const int ArticleItemsMaxCount = 10;

        public const int Default_Empty_Value = -1;

        public const string reg_mobile = "^[1]([3][0-9]{1}|59|58|88|86|89|56|53|151|155)[0-9]{8}$";
        public const string reg_identitycard = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
        public const string reg_postcode = @"^\d{6}$";
        public const string reg_email = @"^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$";
        public const string reg_telephone = @"^(\d{3,4}-)?\d{6,8}$";
        public const string reg_date = @"([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))";
        public const string reg_price = @"^\d{0,8}\.{0,1}(\d{1,2})?$";
        public const string reg_period = @"^\d{1,2}$";

        public const string reg_schoolcode = @"^\d{10}$";
        public const string reg_checkUserName = @"^[a-zA-Z]\w+";//  [RegularExpression("^[a-zA-Z]\\w+", ErrorMessage = "用户名称必须以字母开头")]
    }
}
