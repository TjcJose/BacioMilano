using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Tools.Web.Captcha
{
    
    public class AdCaptchaOptions
    {
         #region private fields

        private int _width;
        private int _height;
        private int _length;
        private string _chars;

        #endregion

        #region public properties

        /// <summary>
        /// 验证字符长度（字符个数）
        /// </summary>
        public int TextLength {
            get { return _length; }
            set {_length = value < 3 ? 3 : value;}
        }

        /// <summary>
        /// 生成验证码用的字符
        /// </summary>
        public string TextChars {
            get {return _chars;}
            set {_chars =(string.IsNullOrEmpty(value)||value.Trim().Length < 3) ? "ACDEFGHJKLMNPQRSTUVWXYZ2346789" : value.Trim();}
        }

        /// <summary>
        /// Font warp factor
        /// </summary>
        public Level FontWarp { get; set; }

        /// <summary>
        /// Background Noise level
        /// </summary>
        public Level BackgroundNoise { get; set; }

        /// <summary>
        /// 线条杂色级别
        /// </summary>
        public Level LineNoise { get; set; }


        /// <summary>
        /// Width of captcha image
        /// </summary>
        public int Width
        {
            get { return _width; }
            set{_width = value < (TextLength * 18)?TextLength*18:value;}
        }

        /// <summary>
        /// Height of captcha image
        /// </summary>
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value<32?32:value;
            }
        }

        public string ReloadLinkText {
            get; set;
        }

        /// <summary>
        /// 验证控件Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 隐藏控件Id
        /// </summary>
        public string HiddenCtrlId
        {
            get
            {
                return AdCaptchaHelper.Get_HiddenCtrlId(Id);
            }
        }

        /// <summary>
        /// 图片控件Id
        /// </summary>
        public string ImgCtrlId
        {
            get
            {
                return AdCaptchaHelper.Get_ImgCtrlId(Id);
            }
        }

        /// <summary>
        /// 验证码输入超时秒数
        /// </summary>
        public int OverSecond
        {
            get;set;
        }

        #endregion

        #region constructor

        public AdCaptchaOptions()
        {
            FontWarp = Level.Medium;
            BackgroundNoise = Level.Low;
            LineNoise = Level.Low;
            ReloadLinkText = "换一张";
            Width = 160;
            Height = 40;
            TextLength = 4;
            OverSecond = 120;
        }

        #endregion

       
        public static AdCaptchaOptions Instance(Style style = Style.Default)
        {
            var opt = new AdCaptchaOptions();
            if (style == Style.Small)
            {
                opt.FontWarp = Level.Low;
                opt.Width = 78;
                opt.Height = 38;
            }
            return opt;
        }
    }
}
