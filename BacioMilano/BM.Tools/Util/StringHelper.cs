using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Util
{
    /// <summary>
    /// 字符串操作帮助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 比较字符是否相同串忽略大小写
        /// </summary>
        /// <param name="compareA">字符串A</param>
        /// <param name="compareB">字符串B</param>
        /// <returns>比较结果</returns>
        public static bool CompareIgnoreCase(string compareA, string compareB)
        {
            return compareA.ToLower() == compareB.ToLower();
        }

        /// <summary>
        /// 比较字符是否相同串忽略大小写
        /// </summary>
        /// <param name="compareA">字符串A</param>
        /// <param name="compareB">字符串B</param>
        /// <returns>比较结果</returns>
        public static int ComparisonIgnoreCase(string compareA, string compareB)
        {
            string a = compareA.ToLower();
            string b = compareB.ToLower();
            return a.CompareTo(b);
        }

        /// <summary>
        /// 把字符数组添加分割符后转化成字符串
        /// </summary>
        /// <param name="array">字符数组</param>
        /// <param name="split">添加的分割符</param>
        /// <returns>字符数组添加分割符后转化成的字符串</returns>
        public static string GetArrayToString(string[] array, string split)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]);
                if (i < array.Length - 1)
                {
                    string temp = array[i + 1].Trim();
                    if (temp != "")
                    {
                        sb.Append(split);
                    }
                }
            }
            return sb.ToString();
        }

        #region 截取指定长度字符

        #region GetStrLen 获取字符串的字节长度
        /// <summary>
        /// 获取字符串的字节长度
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>字符串的字节长度</returns>
        public static int GetStrLen(string s)
        {
            return System.Text.Encoding.Default.GetBytes(s).Length;
        }

        /// <summary>
        /// 获取字符串的字节长度
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>字符串的字节长度</returns>
        public static int GetStrLenA(string s)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] arr = ascii.GetBytes(s);

            int len = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if ((int)arr[i] == 63)
                {
                    len += 2;
                }
                else
                {
                    len++;
                }
            }
            return len;
        }
        #endregion

        public static string CutLenStrA(string ostr, int len)
        {
            int lenA = GetStrLenA(ostr);
            if (lenA < len)
            {
                return ostr;
            }
            if (lenA == len)
            {
                return ostr;
            }

            return CutLenStrA(ostr.Substring(0, ostr.Length - 1), len);
        }

        #region CutLenStr 截取指定长度字符 （二分法）
        
        /// <summary>
        /// 截取指定长度字符
        /// 1. 如果isChWord为false, 把单个字符都看成一个长度。结果就是截取ostr的头len个字符返回
        /// 如：CutLenStr("1234567", 2, false) 返回'12'
        ///     CutLenStr("一234567", 2, false) 返回'一2'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'
        /// 2. 如果isChWord为true,把一个单字节字符都看成一个长度,把双字节字符（如中文字符，韩文，日文...）看成两个字符，len表示的是双字节个数。
        /// 如：CutLenStr("1234567", 2, true) 返回'1234'
        ///     CutLenStr("一234567", 2, false) 返回'一23'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'
        /// </summary>
        /// <param name="ostr">原字符串</param>
        /// <param name="len">截取长度</param>
        /// <param name="isChWord">是否中文长度为准</param>
        /// <returns>
        /// 截取的指定长度字符
        /// </returns>
        /// <remarks>
        /// 在截取指定长度字符时，我们可能会遇到单字节字符和双字节字符混和的情况（如中英文混合），
        /// 我们如果用一个字符占一个长度的方法计算要截取的字符串长度，对不同字符串截取，得到的长度可能不一样
        /// 如：CutLenStr("1234567", 2, false)    返回'12'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'。
        /// 为解决这个问题，我们在计算字符串长度时就一该以字节长度为准。下面的方法就是以字节长度为准计算字符串长度的，采用的是二分法算法。
        /// </remarks>
        public static string CutLenStr(string ostr, int len, bool isChWord)
        {
            if (isChWord == false)//如果isChWord为false, 把所有单个字符都看成一个长度
            {
                return ostr.Length > len ? ostr.Substring(0, len) : ostr;
            }
            else //如果isChWord为true,把一个单字节字符都看成一个长度,把双字节字符（如中文字符，韩文，日文...）看成两个字符
            {
                if(String.IsNullOrEmpty(ostr))
                {
                    return "";
                }
                if (ostr.Length == 1 && len > 0)
                    return ostr;
                if (len == 1 && ostr.Length > 0)
                    return ostr.Substring(0, 1);
                int lenc = len * 2;
                int ostrLen = GetStrLen(ostr);
                if (ostrLen <= lenc)
                    return ostr;

                return GetLenStr(ostr, ref ostr, lenc);

            }
        }

        /// <summary>
        /// 截取指定长度字符
        /// 1. 如果isChWord为false, 把单个字符都看成一个长度。结果就是截取ostr的头len个字符返回
        /// 如：CutLenStr("1234567", 2, false) 返回'12'
        ///     CutLenStr("一234567", 2, false) 返回'一2'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'
        /// 2. 如果isChWord为true,把一个单字节字符都看成一个长度,把双字节字符（如中文字符，韩文，日文...）看成两个字符，len表示的是双字节个数。
        /// 如：CutLenStr("1234567", 2, true) 返回'1234'
        ///     CutLenStr("一234567", 2, false) 返回'一23'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'
        /// </summary>
        /// <param name="ostr">原字符串</param>
        /// <param name="len">截取长度</param>
        /// <param name="isChWord">是否中文长度为准</param>
        /// <returns>
        /// 截取的指定长度字符
        /// </returns>
        /// <remarks>
        /// 在截取指定长度字符时，我们可能会遇到单字节字符和双字节字符混和的情况（如中英文混合），
        /// 我们如果用一个字符占一个长度的方法计算要截取的字符串长度，对不同字符串截取，得到的长度可能不一样
        /// 如：CutLenStr("1234567", 2, false)    返回'12'
        ///     CutLenStr("一而似利古", 2, false) 返回'一而'。
        /// 为解决这个问题，我们在计算字符串长度时就一该以字节长度为准。下面的方法就是以字节长度为准计算字符串长度的，采用的是二分法算法。
        /// </remarks>
        public static string CutLenString(this string ostr, int len, bool isChWord)
        {
            return CutLenStr(ostr, len, isChWord);
        }

        public static string CutLenString(this string ostr, int startIndex, int len, bool isChWord)
        {
            if (isChWord == false)
            {
                return ostr.Substring(startIndex, len);
            }
            else if (startIndex == 0)
            {
                return ostr.CutLenString(len, isChWord);
            }
            else if (startIndex > 0)
            {
                string s = ostr.Substring(0, startIndex + 1);
                s = ostr.Substring(ostr.IndexOf(s));
                return s.CutLenString(len, isChWord);
            }
            return "";
        }

        private static string GetLenStr(string str, ref string ostr, int len)
        {
            int totalLen = GetStrLen(str);
            if (totalLen == len)
                return str;

            if (totalLen + 1 == len)
            {
                string astr = ostr.Substring(0, str.Length + 1);
                if (GetStrLen(astr) == len)
                {
                    return astr;
                }
                return str;
            }


            if (totalLen - 1 == len)
            {
                string dstr = ostr.Substring(0, str.Length - 1);
                if (GetStrLen(dstr) <= len)
                {
                    return dstr;
                }
                return str;
            }

            if (totalLen < len)
            {
                int temp = str.Length + Convert.ToInt32((len - totalLen) * 0.5);
                str = ostr.Substring(0, temp);
                return GetLenStr(str, ref ostr, len);
            }
            else
            {
                int temp = Convert.ToInt32((totalLen - len) * 0.5);
                str = ostr.Substring(0, temp);
                return GetLenStr(str, ref ostr, len);
            }
        }

        #endregion

        #region CutLenStrAddTwoDotGB 截取指定长度字符（二分法）并加..
        public static string CutLenStrAddTwoDotGB(this string ostr, int len, bool isChWord)
        {
            if (String.IsNullOrEmpty(ostr))
            {
                return "";
            }
            string str = CutLenStr(ostr, len, isChWord);

            if (str.Length < ostr.Length)
            {
                str += "..";
            }
            return str;
        }
        #endregion

        #region CutLenStrGB 字符串截取函数
        public static string CutLenStrGB(this string inputString, int len)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);

            if (tempString.Length + 1 == inputString.Length)
                return inputString;

            if (mybyte.Length > len && inputString.Length > tempString.Length)
                tempString += "…";

            return tempString;
        }
        #endregion

        #region CutLenStrAddTwoDot 截取指定长度字符并加..
        /// <summary>
        /// 截取指定长度字符并加..
        /// </summary>
        /// <param name="ostr"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutLenStrAddTwoDot(this string ostr, int len)
        {
            if (ostr.Length < len)
                return ostr;
            else
            {
                return ostr.Substring(0, len) + "..";
            }

        }

        #endregion

        public static string SubStringBytes(this string ostr, int startIndex, int len, Encoding encoding)
        {
           if (ostr.Length == 0) return "";
           var bytes = encoding.GetBytes(ostr);
           if (startIndex > bytes.Length) return "";
           if (startIndex + len > bytes.Length) len = bytes.Length - startIndex;
           return encoding.GetString(bytes, startIndex, len);
        }

        public static string SubStringBytes(this string ostr, int startIndex, Encoding encoding)
        {
            if (ostr.Length == 0) return "";
            var bytes = encoding.GetBytes(ostr);
            if (startIndex > bytes.Length) return "";
            return encoding.GetString(bytes, startIndex, bytes.Length - startIndex);
        }

        #endregion

        #region AddCharToLenStr 补足字符长度
        /// <summary>
        /// 补足字符长度
        /// </summary>
        /// <param name="len">补足后的长度</param>
        /// <param name="x">填补的字符</param>
        /// <param name="original">被补的字符串</param>
        /// <param name="isFrontOrBehind">补在字符串前还是字符串后</param>
        public static string AddCharToLenStr(int len, char x, string original, bool isFrontOrBehind)
        {
            int strLen = original.Length;
            if (strLen == len)
                return original;
            else if (strLen < len)
            {
                if (isFrontOrBehind)
                {
                    return AddCharToLenStr(len, x, x + original, isFrontOrBehind);
                }
                else
                {
                    return AddCharToLenStr(len, x, original + x, isFrontOrBehind);
                }
            }
            else
            {
                throw new Exception("被补的字符串长度超过定义的长度");
            }
        }

        /// <summary>
        /// 补足字符长度
        /// </summary>
        /// <param name="len">补足后的长度</param>
        /// <param name="x">填补的字符</param>
        /// <param name="original">被补的字符串</param>
        /// <param name="isFrontOrBehind">补在字符串前还是字符串后</param>
        public static string AddCharToLenStrA(int len, char x, string original, bool isFrontOrBehind)
        {
            int strLen = GetStrLenA(original);
            if (strLen == len)
                return original;
            else if (strLen < len)
            {
                if (isFrontOrBehind)
                {
                    return AddCharToLenStrA(len, x, x + original, isFrontOrBehind);
                }
                else
                {
                    return AddCharToLenStrA(len, x, original + x, isFrontOrBehind);
                }
            }
            else
            {
                throw new Exception("被补的字符串长度超过定义的长度");
            }
        }
        #endregion

        #region RepeatStrNum 重复字符串
        /// <summary>
        /// 附加字符串
        /// </summary>
        /// <param name="splitStrNum">字符数</param>
        /// <param name="addstr">附加的字符串</param>
        public static string RepeatStrNum(int splitStrNum, string addstr)
        {
            StringBuilder sb = new StringBuilder("");
            while (splitStrNum > 0)
            {
                sb.Append(addstr);
                splitStrNum--;
            }
            return sb.ToString();
        }
        #endregion

        public static bool ContainRight(this string sourceString, string part)
        {
            if(sourceString.Contains(part))
            {
                return sourceString.Substring(sourceString.Length - part.Length) == part;
            }
            return false;
        }

        public static bool ContatinLeft(this string sourceString, string part)
        {
            if (sourceString.Contains(part))
            {
                return sourceString.Substring(0, part.Length) == part;
            }
            return false;
        }


        public static string FormatIntAddPre(string pre, bool isAddZero, int i, int len)
        {
            string tmp = pre + i;
            int num = len - tmp.Length;
            if (num > 0 && isAddZero)
            {
                tmp = pre + new string('0', num) + i;
            }
            return tmp;
        }
    }

}
