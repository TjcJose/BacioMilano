using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace BM.Util
{
    /// <summary>
    /// Random 随机数类
    /// </summary>
    public sealed class RandomHelper
    {
        /// <summary>
        ///  随机数类构造函数
        /// </summary>
        /// <param name="len">随机数长度</param>
        private RandomHelper(int len)
        {
            this.len = len;
        }

        #region 得到随机生成数
        /// <summary>
        /// 得到随机生成数
        /// </summary>
        /// <param name="randomWordType">随机生成数类型枚举</param>
        /// <returns>随机生成数</returns>
        public string GetRandomWord(RandomWordType randomWordType)
        {
            if (randomWordType == RandomWordType.Num)
            {
                return this.getWordNum();
            }
            else if (randomWordType == RandomWordType.WordAndNum)
            {
                return this.getWord();
            }
            else
            {
                return this.getWordNoNum();
            }
        }
        #endregion

        #region 得到一个随机号
        /// <summary>
        /// 得到一个随机字母串
        /// </summary>
        /// <returns>随机字母</returns>
        public static string Get_RandomID()
        {
            return GetRandomWord(20, RandomWordType.WordNoNum);
        }
        #endregion

        #region 得到一个小于maxValue的随机数
        /// <summary>
        /// 得到一个小于maxValue的随机数
        /// </summary>
        /// <param name="maxValue">一定小于最大值</param>
        /// <returns>一个小于maxValue的随机数</returns>
        public static int Get_Random(int maxValue)
        {
            System.Random random = new System.Random();
            return random.Next(maxValue);
        }
        #endregion

        #region 得到一个小于 maxValue, 大于 minValue 的随机数
        private static Random random = new Random();
        /// <summary>
        /// 得到一个小于 maxValue, 大于 minValue 的随机数
        /// </summary>
        /// <param name="minValue">一定大于的最小值</param>
        /// <param name="maxValue">一定小于最大值</param>
        /// <returns>一个小于 maxValue, 大于 minValue 的随机数</returns>
        public static int Get_Random(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
        #endregion

        #region 得到随机生成数
        /// <summary>
        /// 得到随机生成数
        /// </summary>
        /// <param name="len">得到随机生成数长度</param>
        /// <param name="randomWordType">随机生成数类型枚举</param>
        /// <returns></returns>
        public static string GetRandomWord(int len, RandomWordType randomWordType)
        {
            RandomHelper r = new RandomHelper(len);
            return r.GetRandomWord(randomWordType);
        }
        #endregion

        #region private
        private int len;
        
        private static readonly CultureInfo cultureInfo = new CultureInfo("de-DE");


        private string getWord()
        {
            StringBuilder word = new StringBuilder();
            for (int i = 0; i < this.len; i++)
            {
                word.Append(this.getCharStr(random.Next(10)));
            }
            return word.ToString();
        }
        private string getWordNum()
        {
            StringBuilder word = new StringBuilder();
            for (int i = 0; i < this.len; i++)
            {
                word.Append(random.Next(10));
            }
            return word.ToString();
        }
        private string getWordNoNum()
        {
            StringBuilder word = new StringBuilder();
            for (int i = 0; i < this.len; i++)
            {
                word.Append(this.getCharStrNoNum(random.Next(10)));
            }
            return word.ToString();
        }

        /// <summary>
        /// 将数字随机变成字符
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        private string getCharStr(int num)
        {
            int i = random.Next(5);
            if (i == 3)
            {
                return num.ToString();
            }
            else
            {
                return this.GetLetter(num);
            }
        }
        /// <summary>
        /// 将数字随机变成字符(无数字)
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        private string getCharStrNoNum(int num)
        {
            int i = random.Next(5);
            return this.GetLetter(i);
        }
        /// <summary>
        /// 将数字随机变成英文字母
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns></returns>
        private string GetLetter(int num)
        {
            int i = random.Next(3);
            if (i == 2)
            {
                i = random.Next(65, 90);
            }
            else
            {
                i = random.Next(97, 122);
            }
            return System.Convert.ToChar(System.Convert.ToByte(i)).ToString();
        }
        #endregion

        #region 得到一个唯一号
        /// <summary>
        /// 得到一个唯一号
        /// </summary>
        /// <returns></returns>
        public static string Get_UniqueID()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 得到一个时间为前缀的唯一号
        /// </summary>
        /// <returns></returns>
        public static string Get_UniqueID_PreIsTime()
        {
            return DateTime.Now.ToString("o", cultureInfo) + "_" + Guid.NewGuid().ToString();
        }
        #endregion
    }

    /// <summary>
    /// 随机生成数类型枚举
    /// </summary>
    public enum RandomWordType
    {
        /// <summary>
        /// 字母或数字
        /// </summary>
        WordAndNum = 0,
        /// <summary>
        /// 数字
        /// </summary>
        Num = 1,
        /// <summary>
        /// 字母
        /// </summary>
        WordNoNum = 2
    }
}

