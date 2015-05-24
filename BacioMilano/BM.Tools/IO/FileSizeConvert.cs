using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.IO
{
    public static class FileSizeConvert
    {
        public const double ONE_KB = 1024;
        public const double ONE_MB = ONE_KB * 1024;
        public const double ONE_GB = ONE_MB * 1024;
        public const double ONE_TB = ONE_GB * 1024;
        public const double ONE_PB = ONE_TB * 1024;
        public const double ONE_EB = ONE_PB * 1024;
        public const double ONE_ZB = ONE_EB * 1024;
        public const double ONE_YB = ONE_ZB * 1024;


        public static string FormatBytes(long bytes)
        {
            if ((double)bytes <= 999)
                return bytes.ToString() + " bytes";
            else if ((double)bytes <= ONE_KB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_KB) + " KB";
            else if ((double)bytes <= ONE_MB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_MB) + " MB";
            else if ((double)bytes <= ONE_GB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_GB) + " GB";
            else if ((double)bytes <= ONE_TB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_TB) + " TB";
            else if ((double)bytes <= ONE_PB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_PB) + " PB";
            else if ((double)bytes <= ONE_EB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_EB) + " EB";
            else if ((double)bytes <= ONE_ZB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_ZB) + " ZB";
            else
                return ThreeNonZeroDigits((double)bytes / ONE_YB) + " YB";
        }

        public static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100)
                return ((int)value).ToString();
            else if (value >= 10)
                return value.ToString("0.0");
            else
                return value.ToString("0.00");
        }

        /// <summary>
        /// 得到数据大小
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetSize(string param)
        {
            int n = 1;
            double size = double.Parse(param);
            string result = string.Empty;
            while (true)
            {
                if (param.Length >= 4 * n - 1)
                {
                    size = size / 1024.0;
                    n++;
                }
                else
                {
                    break;
                }
            }
            size = Math.Round(size, 2);
            if (n == 1)
            {
                result = size.ToString() + "B";
            }
            else if (n == 2)
            {
                result = size.ToString() + "K";
            }
            else if (n == 3)
            {
                result = size.ToString() + "M";
            }
            else if (n == 4)
            {
                result = size.ToString() + "G";
            }
            return result;
        }
    }
}
