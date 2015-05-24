using BM.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Tools.Web.Captcha
{
    internal static class AdCaptchaHelper
    {
        public  const string AdCaptchaId = "AdCaptcha";
        public  const string AdCaptchaOver = "overtime";

        internal readonly static Random Rand = new Random();
        private static string GenerateRandomText(AdCaptchaOptions opts)
        {
            string txtChars = opts.TextChars;
            if (string.IsNullOrEmpty(txtChars))
            {
                txtChars = "ACDEFGHJKLMNPQRSTUVWXYZ2346789";
            }
            int len = opts.TextLength;
            var sb = new StringBuilder(len);
            int maxLength = txtChars.Length;
            for (int n = 0; n < len; n++)
            {
                sb.Append(txtChars.Substring(Rand.Next(maxLength), 1));
            }

            return sb.ToString();
        }

        public static string Get_HiddenCtrlId(string id)
        {
            return string.Format(@"{0}_hid", id);
        }

        public static string Get_ImgCtrlId(string id)
        {
            return string.Format(@"{0}_img", id);
        }

        public static string Get_InputCtrlId(string id)
        {
            return string.Format(@"{0}_input", id);
        }

        internal static string Get_HiddenValue(string id = "AdCaptcha", Style style = Style.Default)
        {
            AdCaptchaOptions opts = AdCaptchaOptions.Instance(style);
            opts.Id = id;
            return AdCaptchaHelper.GenerateHiddenValue(opts);
        }

        internal static string GenerateHiddenValue(AdCaptchaOptions opts)
        {
            string s = String.Format(@"{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", Rand.Next(0, 9), GenerateRandomText(opts), DateTime.Now.Ticks, opts.OverSecond, opts.Width, opts.Height, (int)opts.LineNoise, (int)opts.FontWarp, (int)opts.BackgroundNoise);
            return BM.Security.EncryptReUrl.Instance.EncryptString(s);
        }

        internal static AdCaptchaImageOption GetCaptchaImageOptionFromHiddenValue(string hiddenValue)
        {
            try
            {
                var s = BM.Security.EncryptReUrl.Instance.DecryptString(hiddenValue);
                var arr = s.Split('|');
                if (arr.Length != 9 || arr[0].Length != 1)
                {
                    return null;
                }
                int overSecond = int.Parse(arr[3]);
                DateTime dt = new DateTime(long.Parse(arr[2]));
                if (dt.AddMinutes(overSecond) < DateTime.Now)
                {
                    return null;
                }
                AdCaptchaImageOption imgOpt = new AdCaptchaImageOption();
                imgOpt.Text = arr[1];
                imgOpt.Width = int.Parse(arr[4]);
                imgOpt.Height = int.Parse(arr[5]);
                imgOpt.LineNoise = (Level)int.Parse(arr[6]);
                imgOpt.FontWarp = (Level)int.Parse(arr[7]);
                imgOpt.BackgroundNoise = (Level)int.Parse(arr[8]);
                return imgOpt;
            }
            catch
            {
                return null;
            }
        }

        internal static string GetCaptchaFromHiddenValue(string hiddenValue)
        {
            try
            {
                var s = BM.Security.EncryptReUrl.Instance.DecryptString(hiddenValue);
                var arr = s.Split('|');
                if(arr.Length != 9 || arr[0].Length != 1)
                {
                    return null;
                }
                int overSecond = int.Parse(arr[3]);
                DateTime dt = new DateTime(long.Parse(arr[2]));
                if (dt.AddSeconds(overSecond) < DateTime.Now)
                {
                    return AdCaptchaOver;
                }
                return arr[1];
            }
            catch
            {
                return null;
            }
        }
    }
}
