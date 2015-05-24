using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using org.mozilla.intl.chardet;


namespace BM.Util
{
    public class CharsetHelper
    {
        private class CharsetDetectionObserver : nsICharsetDetectionObserver
        {
            public string Charset = null;
            public void Notify(string charset)
            {
                Charset = charset;
            }

        }

        public static Encoding GetCharset(Stream stream, CharsetType charsetType)
        {
            //用指定的语参数实例化Detector
            var det = new nsDetector((int)charsetType);
            //初始化
            CharsetDetectionObserver cdo = new CharsetDetectionObserver();
            det.Init(cdo);

            byte[] buf = new byte[1024];
            int len;
            bool done = false;
            bool isAscii = true;
            stream.Position = 0;

            while ((len = stream.Read(buf, 0, buf.Length)) != 0)
            {
                // 探测是否为Ascii编码
                if (isAscii)
                    isAscii = det.isAscii(buf, len);

                // 如果不是Ascii编码，并且编码未确定，则继续探测
                if (!isAscii && !done)
                    done = det.DoIt(buf, len, false);
            }

            det.DataEnd();

            if (isAscii)
            {
                return Encoding.ASCII;
            }

            if (cdo.Charset != null)
            {
                return Encoding.GetEncoding(cdo.Charset);
            }

            return null;
        }

        public static Encoding GetCharset(Byte[] bytes, CharsetType charsetType)
        {
            using (Stream stream = new MemoryStream(bytes))
            {
                return CharsetHelper.GetCharset(stream, charsetType);
            }
        }

        public enum CharsetType
        {
            Japanese = 1,
            Chinese = 2,
            Simplified_Chinese = 3,
            Traditional_Chinese = 4,
            Korean = 5,
            /// <summary>
            /// 默认
            /// </summary>
            DontKnow = 6
        }
    }
}
