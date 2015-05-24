using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Tools.Web.Captcha
{
    internal class AdCaptchaImageOption
    {
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Level LineNoise { get; set; }
        public Level FontWarp { get; set; }
        public Level BackgroundNoise { get; set; }
        
    }
}
