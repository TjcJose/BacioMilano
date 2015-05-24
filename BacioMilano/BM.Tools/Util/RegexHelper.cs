using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BM.Util
{
    /// <summary>
    /// 正则帮助类
    /// </summary>
    public static class RegexHelper
    {
        #region ReplaceByArray 正则表达式匹配数组替换
        /// <summary>
        /// 正则表达式匹配数组替换
        /// </summary>
        /// <param name="reg">正则对象</param>
        /// <param name="groupsIndexName">替换属性名</param>
        /// <param name="input">输入要匹配的字符串</param>
        /// <param name="useCompareKeyReplaceValue">用于比较替换的字典</param>
        /// <param name="compareHelp">比较类</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceByArray(this Regex reg, string groupsPropertyName, string input, Dictionary<string, string> useCompareKeyReplaceValue, CompareHelper<string> compareHelp)
        {
            StringBuilder sb = new StringBuilder();
            int startIndex = 0;
            MatchCollection ms = reg.Matches(input);
            for (int i = 0; i < ms.Count; i++)
            {
                if (ms[i].Index > -1)
                {
                    sb.Append(input.Substring(startIndex, (ms[i].Index - startIndex)));

                    string value = ms[i].Groups[groupsPropertyName].Value;
                    compareHelp.SetCompareObj(value);
                    bool isReplace = false;
                    for (int j = 0; j < useCompareKeyReplaceValue.Count; j++)
                    {
                        if (compareHelp.Compare(useCompareKeyReplaceValue.ElementAt(j).Key))
                        {
                            sb.Append(useCompareKeyReplaceValue.ElementAt(j).Value);
                            isReplace = true;
                            break;
                        }
                    }
                    if (isReplace == false)
                    {
                        sb.Append(ms[i]);
                    }

                    startIndex = ms[i].Index + value.Length + 1;
                }
                else
                {
                    sb.Append(startIndex);
                }
            }
            if (startIndex < input.Length)
            {
                sb.Append(input.Substring(startIndex));
            }
            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 正则表达式匹配位置
        /// </summary>
        public enum E_Regex_Match_Position
        {
            /// <summary>
            /// 开始
            /// </summary>
            begin = 1,
            /// <summary>
            /// 结束
            /// </summary>
            end = 2,
            /// <summary>
            /// 开始结束
            /// </summary>
            beginEnd = 3,
            /// <summary>
            /// 任意位置
            /// </summary>
            any = 4
        }

        /// <summary>
        /// 获取匹配正则表达式
        /// </summary>
        /// <param name="regString">匹配正则表达式</param>
        /// <param name="matchPosition">正则表达式匹配位置</param>
        private static string GetRegString(string regString, E_Regex_Match_Position matchPosition)
        {
            if (matchPosition == E_Regex_Match_Position.begin)
            {
                return "^" + regString;
            }
            else if (matchPosition == E_Regex_Match_Position.end)
            {
                return regString + "$";
            }
            else if (matchPosition == E_Regex_Match_Position.beginEnd)
            {
                return "^" + regString + "$";
            }
            else
            {
                return regString;
            }
        }

        /// <summary>
        /// 非负浮点数（正浮点数   +   0）
        /// </summary>
        public const string reg_PositiveFloatPointNumberAndZero_str = "\\d+(\\.\\d+)?";

        /// <summary>
        /// //非负浮点数（正浮点数   +   0）
        /// </summary>
        /// <param name="matchPosition">正则表达式匹配位置</param>
        public static string Get_PositiveFloatPointNumberAndZero(E_Regex_Match_Position matchPosition)
        {
            return GetRegString(reg_PositiveFloatPointNumberAndZero_str, matchPosition);
        }

        /// <summary>
        /// 正浮点数
        /// </summary>
        public const string reg_PositiveFloatPointNumber_str = "(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))";

        /// <summary>
        /// 正浮点数
        /// </summary>
        public static string Get_PositiveFloatPointNumber(E_Regex_Match_Position matchPosition) { return GetRegString(reg_PositiveFloatPointNumber_str, matchPosition); }

        /// <summary>
        /// 非正浮点数（负浮点数   +   0）
        /// </summary>
        public const string reg_NegativeFloatPointAndZero_str = "((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))";


        /// <summary>
        /// 非正浮点数（负浮点数   +   0）   
        /// </summary>
        public static string Get_NegativeFloatPointAndZero(E_Regex_Match_Position matchPosition) { return GetRegString(reg_NegativeFloatPointAndZero_str, matchPosition); }


        /// <summary>
        /// 非正浮点数（负浮点数）   
        /// </summary>
        public const string reg_NegativeFloatPoint_str = "(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))";

        /// <summary>
        /// 非正浮点数（负浮点数）   
        /// </summary>
        public static string Get_NegativeFloatPoint(E_Regex_Match_Position matchPosition) { return GetRegString(reg_NegativeFloatPoint_str, matchPosition); }

        /// <summary>
        /// 浮点数   
        /// </summary>
        public const string reg_FloatPoint_str = "(-?\\d+)(\\.\\d+)?";

        /// <summary>
        /// 浮点数   
        /// </summary>
        public static string Get_FloatPoint(E_Regex_Match_Position matchPosition) { return GetRegString(reg_FloatPoint_str, matchPosition); }

        /// <summary>
        ///  非负整数（正整数   +   0）   
        /// </summary>
        public const string reg_PositiveIntegerNumberAndZero_str = "\\d+";

        /// <summary>
        /// 非负整数（正整数   +   0）
        /// </summary>
        public static string Get_PositiveIntegerNumberAndZero(E_Regex_Match_Position matchPosition) { return GetRegString(reg_PositiveIntegerNumberAndZero_str, matchPosition); }

        /// <summary>
        /// 正整数   
        /// </summary>
        public const string reg_PositiveIntegerNumber_str = "[0-9]*[1-9][0-9]*";

        /// <summary>
        /// 正整数
        /// </summary>
        public static string Get_PositiveIntegerNumber(E_Regex_Match_Position matchPosition) { return GetRegString(reg_PositiveIntegerNumber_str, matchPosition); }

        /// <summary>
        /// 非正整数（负整数   +   0）   
        /// </summary>
        public const string reg_NegativeIntegerAndZero_str = "((-\\d+)|(0+))";

        /// <summary>
        /// 非正整数（负整数   +   0）
        /// </summary>
        public static string Get_NegativeIntegerAndZero(E_Regex_Match_Position matchPosition) { return GetRegString(reg_NegativeIntegerAndZero_str, matchPosition); }

        /// <summary>
        /// 负整数   
        /// </summary>
        public const string reg_NegativeInteger_str = "-[0-9]*[1-9][0-9]*";

        /// <summary>
        /// 负整数
        /// </summary>
        public static string Get_NegativeInteger(E_Regex_Match_Position matchPosition) { return GetRegString(reg_NegativeInteger_str, matchPosition); }

        /// <summary>
        /// 整数   
        /// </summary>
        public const string reg_IntegerNumber_str = "-?\\d+";

        /// <summary>
        /// 整数
        /// </summary>
        public static string Get_IntegerNumber(E_Regex_Match_Position matchPosition) { return GetRegString(reg_IntegerNumber_str, matchPosition); }

        /// <summary>
        /// 由26个英文字母组成的字符串   
        /// </summary>
        public const string reg_EnglishLetter_str = "[A-Za-z]+";

        /// <summary>
        /// 由26个英文字母组成的字符串
        /// </summary>
        public static string Get_EnglishLetter(E_Regex_Match_Position matchPosition) { return GetRegString(reg_EnglishLetter_str, matchPosition); }

        /// <summary>
        /// 由26个英文字母的大写组成的字符串  
        /// </summary>
        public const string reg_EnglishLetterUpper_str = "[A-Z]+";

        /// <summary>
        /// 由26个英文字母的大写组成的字符串 
        /// </summary>
        public static string Get_EnglishLetterUpper(E_Regex_Match_Position matchPosition) { return GetRegString(reg_EnglishLetterUpper_str, matchPosition); }

        /// <summary>
        /// 由26个英文字母的小写组成的字符串 
        /// </summary>
        public const string reg_EnglishLetterLower_str = "[a-z]+";

        /// <summary>
        /// 由26个英文字母的小写组成的字符串 
        /// </summary>
        public static string Get_EnglishLetterLower(E_Regex_Match_Position matchPosition) { return GetRegString(reg_EnglishLetterLower_str, matchPosition); }

        /// <summary>
        /// 由数字和26个英文字母组成的字符串 
        /// </summary>
        public const string reg_DigitLetter_str = "[A-Za-z0-9]+";

        /// <summary>
        /// 由数字和26个英文字母组成的字符串 
        /// </summary>
        public static string Get_DigitLetter(E_Regex_Match_Position matchPosition) { return GetRegString(reg_DigitLetter_str, matchPosition); }

        /// <summary>
        /// 由数字、26个英文字母或者下划线组成的字符串 
        /// </summary>
        public const string reg_DigitLetterUnderline_str = "\\w+";

        /// <summary>
        /// 由数字、26个英文字母或者下划线组成的字符串 
        /// </summary>
        public static string Get_DigitLetterUnderline(E_Regex_Match_Position matchPosition) { return GetRegString(reg_DigitLetterUnderline_str, matchPosition); }

        /// <summary>
        /// 由数字、26个英文字母或者下划线组成或者汉子的字符串 
        /// </summary>
        public const string reg_DigitLetterUnderlineChina_str = "^[\u4E00-\u9FA5A-Za-z0-9_]+$";

        /// <summary>
        /// 由数字、26个英文字母或者下划线组成或者汉子的字符串 
        /// </summary>
        public static string Get_DigitLetterUnderlineChina(E_Regex_Match_Position matchPosition) { return GetRegString(reg_DigitLetterUnderlineChina_str, matchPosition); }


        /// <summary>
        /// Email 
        /// </summary>
        public const string reg_Email_str = "[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+";

        public static string Get_Email(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Email_str, matchPosition); }

        /// <summary>
        /// Url
        /// </summary>
        public const string reg_Url_str = "(http|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?";

        public static string Get_Url(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Url_str, matchPosition); }

        /// <summary>
        ///验证电话号码：正确格式为："XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX"
        /// </summary>
        public const string reg_Tel_str = @"^[1]([3][0-9]{1}|59|58|88|86|89|56|53|151|155)[0-9]{8}$";

        /// <summary>
        /// 验证电话号码：正确格式为："XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX"
        /// </summary>
        public static string Get_Tel(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Tel_str, matchPosition); }

        /// <summary>
        /// 验证身份证号（15位或18位数字）
        /// </summary>
        public const string reg_IdentityCard_str = @"(^\d{18}$)|(^\d{15}$)";

        /// <summary>
        /// 验证身份证号（15位或18位数字）
        /// </summary>
        public static string Get_IdentityCard(E_Regex_Match_Position matchPosition) { return GetRegString(reg_IdentityCard_str, matchPosition); }

        /// <summary>
        /// 验证一年的12个月: 正确格式为："01"～"09"和"1"～"12"
        /// </summary>
        public const string reg_Month_str = @"(0?[1-9]|1[0-2])";

        /// <summary>
        /// 验证一年的12个月: 正确格式为："01"～"09"和"1"～"12"
        /// </summary>
        public static string Get_Month(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Month_str, matchPosition); }

        /// <summary>
        ///验证一个月的31天: 正确格式为："01"～"09"和"1"～"31"
        /// </summary>
        public const string reg_Day_str = @"((0?[1-9])|((1|2)[0-9])|30|31)";

        /// <summary>
        ///验证一个月的31天: 正确格式为："01"～"09"和"1"～"31"
        /// </summary>
        public static string Get_Day(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Day_str, matchPosition); }

        /// <summary>
        /// 日期
        /// </summary>
        public const string reg_Date_str = @"([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))";

        /// <summary>
        /// 日期
        /// </summary>
        public static string Get_Date(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Date_str, matchPosition); }


        /// <summary>
        /// 验证手机号码
        /// </summary>
        public const string reg_Mobile_str = "^[1]([3][0-9]{1}|59|58|88|86|89|56|53|151|155)[0-9]{8}$";

        /// <summary>
        ///验证手机号码
        /// </summary>
        public static string Get_Mobile(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Mobile_str, matchPosition); }

        /// <summary>
        /// 价格
        /// </summary>
        public const string reg_Price_str = @"^\d{0,8}\.{0,1}(\d{1,2})?$";

        /// <summary>
        ///价格
        /// </summary>
        public static string Get_Price(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Price_str, matchPosition); }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public const string reg_Postcode_str = @"^\d{6}$";

        /// <summary>
        ///邮政编码
        /// </summary>
        public static string Get_Postcode(E_Regex_Match_Position matchPosition) { return GetRegString(reg_Postcode_str, matchPosition); }


    }
}
