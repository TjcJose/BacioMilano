using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Fw
{
    public static class ConstResult
    {
        public const int success = 0;
        public const int fail = -1;


        /// <summary>
        /// 操作员用户名已存在
        /// </summary>
        public const int manager_exist_userName = -11;

        /// <summary>
        /// 操作员编号不存在
        /// </summary>
        public const int manager_notExist_managerId = -12;

        /// <summary>
        /// 不能删除admin操作员
        /// </summary>
        public const int manager_delete_admin = -13;

        /// <summary>
        /// 平台主键存在
        /// </summary>
        public const int plat_exist_id = -14;

        /// <summary>
        /// 平台名称存在
        /// </summary>
        public const int plat_exist_name = -15;

        /// <summary>
        /// 平台功能主键存在
        /// </summary>
        public const int plat_function_exist_id = -16;

        /// <summary>
        /// 平台功能名称存在
        /// </summary>
        public const int plat_function_exist_name = -17;

        /// <summary>
        /// 平台操作主键存在
        /// </summary>
        public const int plat_operation_exist_id = -18;

        /// <summary>
        /// 平台操作名称存在
        /// </summary>
        public const int plat_operation_exist_name = -19;

        /// <summary>
        /// 组名已存在
        /// </summary>
        public const int group_exist_groupName = -20;

        /// <summary>
        /// 组编号不存在
        /// </summary>
        public const int group_notExist_groupId = -21;

        /// <summary>
        /// 存在组Id
        /// </summary>
        public const int group_exist_groupId = 22;

        /// <summary>
        /// 功能主键存在
        /// </summary>
        public const int function_exist_id = -23;

        /// <summary>
        /// 功能名称存在
        /// </summary>
        public const int function_exist_name = -24;

        /// <summary>
        /// 操作主键存在
        /// </summary>
        public const int operation_exist_id = -25;

        /// <summary>
        /// 操作名称存在
        /// </summary>
        public const int operation_exist_name = -26;

        /// <summary>
        /// 操作主键存在
        /// </summary>
        public const int menu_exist_id = -27;

        /// <summary>
        /// 操作名称存在
        /// </summary>
        public const int menu_exist_name = -28;

        /// <summary>
        /// 产品名称存在
        /// </summary>
        public const int product_exist_name = -29;

        /// <summary>
        /// 产品分类名称存在
        /// </summary>
        public const int product_category_exist_name = -30;

        /// <summary>
        /// 产品价格名称存在
        /// </summary>
        public const int product_price_exist_name = -31;

        /// <summary>
        /// 用户名称已存在
        /// </summary>
        public const int user_exist_name = -32;

        /// <summary>
        /// 已经存在微信Id
        /// </summary>
        public const int userweixin_exist_weixinid = -50;

        /// <summary>
        /// 模板主键存在
        /// </summary>
        public const int template_exist_id = -60;

        /// <summary>
        /// 模板名称存在
        /// </summary>
        public const int template_exist_name = -65;

        /// <summary>
        /// 样式主键存在
        /// </summary>
        public const int style_exist_id = -70;

        /// <summary>
        /// 样式名称存在
        /// </summary>
        public const int style_exist_name = -75;

        /// <summary>
        /// 站点主键存在
        /// </summary>
        public const int site_exist_id = -70;

        /// <summary>
        /// 站点名称存在
        /// </summary>
        public const int site_exist_name = -75;

        /// <summary>
        /// 分类名称已经存在
        /// </summary>
        public const int category_exist_name = -80;
    }
}
