using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace BM.GDI
{
    /// <summary>
    ///   提供图象管理的方法。
    /// </summary>
    public class ImageManager : IDisposable
    {
        #region 常量（支持的图像格式）
        public const string allowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp|.gif|.ico";
        #endregion

        #region 静态函数

        #region CheckValidExt 检测扩展名的有效性
        /// <summary>
        /// 检测扩展名的有效性
        /// </summary>
        /// <param name="sExt">文件名扩展名</param>
        /// <returns>如果扩展名有效,返回true,否则返回false.</returns>
        public static bool CheckValidExt(string sExt)
        {
            bool flag = false;
            string[] aExt = allowExt.Split('|');
            foreach (string filetype in aExt)
            {
                if (filetype.ToLower() == sExt)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        #endregion

        #region GetEncoderInfo 获取图像编码解码器的所有相关信息
        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }
        #endregion

        #region 得到 ContentType 类型

        public ImageFormat GetImageFormat(string fileExt)
        {
            switch (fileExt)
            {
                case ".jpe":
                    return ImageFormat.Jpeg;

                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".jpg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".gif":
                    return ImageFormat.Gif;

                case ".tif":
                    return ImageFormat.Tiff;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".ico":
                    return ImageFormat.Icon;
                default:
                    return ImageFormat.Bmp;

            }
        }

        public static string GetContentType(ImageFormat format)
        {
           if(ImageFormat.Jpeg == format) return "image/jpeg";
            if(ImageFormat.Png == format) return "image/png";
             if(ImageFormat.Gif == format) return "image/gif";
            if(ImageFormat.Tiff == format) return "image/tiff";
            if(ImageFormat.Icon == format) return "image/ico";
            return "image/bmp";
        }

        public static string GetContentType(string fileExt)
        {
            string rstr = "";
            switch (fileExt)
            {
                case ".jpe":
                    rstr = "image/jpeg";
                    break;
                case ".jpeg":
                    rstr = "image/jpeg";
                    break;
                case ".jpg":
                    rstr = "image/jpeg";
                    break;
                case ".png":
                    rstr = "image/png";
                    break;
                case ".gif":
                    rstr = "image/gif";
                    break;
                case ".tif":
                    rstr = "image/tiff";
                    break;
                case ".tiff":
                    rstr = "image/tiff";
                    break;
                case ".bmp":
                    rstr = "image/bmp";
                    break;
                case ".ico":
                    rstr = "image/ico";
                    break;
                default:
                    break;
            }
            return rstr;
        }
        #endregion

        public static byte[] ImageToByte(Image img, ImageFormat format, int quality)
        {
           
            ImageCodecInfo myImageCodecInfo;

            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(format));
            myEncoder = Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;


            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, myImageCodecInfo, myEncoderParameters);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static Bitmap ByteArrayToBitmap(byte[] byteArrayIn)
        {
           return new Bitmap(ByteArrayToImage(byteArrayIn));
        }

        #endregion

        #region 私有成员
        // 用于保存处理中的图像的对象
        private Bitmap bmpTemp;
        private Image imgSource;
        #endregion

        #region 公共属性

        #region 支持的图像格式
        /// <summary>
        /// 支持类型
        /// </summary>
        public string AllowExt
        {
            get
            {
                return allowExt;
            }
        }
        /// <summary>
        /// 支持的图像格式
        /// </summary>
        public System.Collections.Specialized.StringDictionary ldImgFormat
        {
            get
            {
                System.Collections.Specialized.StringDictionary ld = new System.Collections.Specialized.StringDictionary();
                ld.Add(".jpe", "image/jpeg");
                ld.Add(".jpeg", "image/jpeg");
                ld.Add(".jpg", "image/jpeg");
                ld.Add(".png", "image/png");
                ld.Add(".tif", "image/tiff");
                ld.Add(".tiff", "image/tiff");
                ld.Add(".bmp", "image/bmp");
                ld.Add(".gif", "image/gif");
                ld.Add(".ico", "image/ico");
                return ld;
            }
        }
        #endregion

        /// <summary>
        /// 获取图像
        /// </summary>
        public Image Image
        {
            get
            {
                return imgSource;
            }
        }
        #endregion

        #region 构造函数

        public ImageManager()
        {

        }

        public ImageManager(string strFile)
        {
            this.LoadFrom(strFile);
        }

        public ImageManager(Stream stream)
        {
            this.LoadFromStream(stream);
        }

        public ImageManager(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            this.LoadFromStream(ms);
        }

        public ImageManager(Image img)
        {
            this.imgSource = img;
        }

        #endregion

        #region 加载图像
        /// <summary>
        /// 从文件中加载
        /// </summary>
        /// <param name="strFile">要加载的图像文件名(包括完整路径)</param>
        public void LoadFrom(string strFile)
        {
            string strExtend = Path.GetExtension(strFile).ToLower();
            if (!CheckValidExt(strExtend))
            {
                throw new Exception("不支持此图像文件格式！");
            }
            imgSource = Image.FromFile(strFile);
        }

        /// <summary>
        /// 从流中加载
        /// </summary>
        /// <param name="stream">各种形式的流</param>
        public void LoadFromStream(Stream stream)
        {
            imgSource = System.Drawing.Image.FromStream(stream);
        }


        /// <summary>
        /// 创建一个指定长宽的空图，并以指定颜色填充
        /// </summary>
        /// <param name="intWidth">宽度</param>
        /// <param name="intHeight">高度</param>
        /// <param name="objBgColor">填充色</param>
        public void CreateImage(int intWidth, int intHeight, Color objBgColor)
        {
            imgSource = new Bitmap(intWidth, intHeight);
            Graphics g = Graphics.FromImage(imgSource);
            g.Clear(objBgColor);
            g.Dispose();
        }

        /// <summary>
        /// 清除Image上的所有图象，并用指定颜色填充
        /// </summary>
        /// <param name="objBgColor">填充色</param>
        public void Clear(Color objBgColor)
        {
            Graphics g = Graphics.FromImage(imgSource);
            g.Clear(objBgColor);
            g.Dispose();
        }
        #endregion

        #region 保存文件
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="stream">保存流</param>
        /// <param name="ext">.png, .jpg 等格式</param>
        /// <param name="quality">保存质量 1 - 100</param>
        public void Save(Stream stream, string ext, int quality)
        {
            ext = ext.ToLower();
            if (CheckValidExt(ext))
            {
                ImageCodecInfo myImageCodecInfo;

                Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(ext));
                myEncoder = Encoder.Quality;

                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                imgSource.Save(stream, myImageCodecInfo, myEncoderParameters);
            }
            else
            {
                throw new Exception("不支持保存为此种图形文件。");
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="stream">保存流</param>
        /// <param name="ext">.png, .jpg 等格式</param>
        public void Save(string strPath, int quality)
        {
            string strExtend = Path.GetExtension(strPath).ToLower();
            if (CheckValidExt(strExtend))
            {
                string strExt = Path.GetExtension(strPath).ToLower();
                ImageCodecInfo myImageCodecInfo;

                Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(strExt));
                myEncoder = Encoder.Quality;

                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                imgSource.Save(strPath, myImageCodecInfo, myEncoderParameters);
            }
            else
            {
                throw new Exception("不支持保存为此种图形文件。");
            }
        }
        
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="strPath">保存路径（完整绝对路径）</param>
        public void Save(Stream stream, string ext)
        {
            ext = ext.ToLower();
            if (CheckValidExt(ext))
            {
                long quality = 80;
                if (ext == ".jpeg" || ext == ".jpg" || ext == ".jpe")
                {
                    quality = 65;
                }
                ImageCodecInfo myImageCodecInfo;

                Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(ext));
                myEncoder = Encoder.Quality;

                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                imgSource.Save(stream, myImageCodecInfo, myEncoderParameters);
            }
            else
            {
                throw new Exception("不支持保存为此种图形文件。");
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="strPath">保存路径（完整绝对路径）</param>
        public void Save(string strPath)
        {
            string strExtend = Path.GetExtension(strPath).ToLower();
            if (CheckValidExt(strExtend))
            {
                string strExt = Path.GetExtension(strPath).ToLower();
                long quality = 75;
                if (strExt == ".jpeg" || strExt == ".jpg" || strExt == ".jpe")
                {
                    quality = 65;
                }
                ImageCodecInfo myImageCodecInfo;

                Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(strExt));
                myEncoder = Encoder.Quality;

                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                imgSource.Save(strPath, myImageCodecInfo, myEncoderParameters);
            }
            else
            {
                throw new Exception("不支持保存为此种图形文件。");
            }
        }
        #endregion

        #region 图像转化成字节数组
        public byte[] GetByteArray(ImageFormat format, int quality)
        {
            ImageCodecInfo myImageCodecInfo;

            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(format));
            myEncoder = Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            MemoryStream m = new MemoryStream();
            this.imgSource.Save(m, myImageCodecInfo, myEncoderParameters);
            return m.GetBuffer();
        }
        public byte[] GetByteArray(string fileExt, int quality)
        {
            ImageCodecInfo myImageCodecInfo;

            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo(ImageManager.GetContentType(fileExt));
            myEncoder = Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            MemoryStream m = new MemoryStream();
            this.imgSource.Save(m, myImageCodecInfo, myEncoderParameters);
            return m.GetBuffer();
        }
        public byte[] GetByteArray()
        {
            return GetByteArray(System.Drawing.Imaging.ImageFormat.Jpeg, 85);
        }

        #endregion

        #region 剪裁图像
        public void ClipImage(int x, int y, int width, int height)
        {
            // 新建BitMap对象，并设置主要属性
            bmpTemp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);
            bmpTemp.MakeTransparent(Color.White);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.Transparent);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, -x, -y, Convert.ToSingle(imgSource.Width), Convert.ToSingle(imgSource.Height));
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
        }

        public void ClipImageCenter(int width, int height)
        {
            int widthY = imgSource.Width;
            int heightY = imgSource.Height;
            int x, y;
            if (widthY > heightY)
            {
                y = 0;
                x = (widthY - heightY) / 2;
            }
            else
            {
                x = 0;
                y = (heightY - widthY) / 2;
            }
            ClipImage(x, y, width, height);
        }

        public void ClipImageCenter(double width, double height)
        {
            ClipImageCenter((int)width, (int)height);
        }

        public void ClipScaleImageCenterByWidth(double width, double height)
        {
            ClipScaleImageCenterByWidth((int)width, (int)height);
        }

        public void ClipScaleImageCenterByWidth(int width, int height)
        {
            int WIDTH = this.Image.Width;
            int HEIGHT = this.Image.Height;

            double B = WIDTH * 1.0 / HEIGHT;
            double b = width * 1.0 / height;

            if (b == B)
            {
                this.ScaleImageByFixSize(width, height, true);
            }

            double H = WIDTH / b;
            double Y = (HEIGHT - H) / 2;
            ClipImage(0, (int)Y, WIDTH, (int)H);



            ScaleImageByFixSize(width, height, true);
        }

        public void ClipScaleImageCenterByHeight(double width, double height)
        {
            ClipScaleImageCenterByHeight((int)width, (int)height);
        }

        public void ClipScaleImageCenterByHeight(int width, int height)
        {
            int WIDTH = this.Image.Width;
            int HEIGHT = this.Image.Height;

            double B = WIDTH * 1.0 / HEIGHT;
            double b = width * 1.0 / height;

            
            if (b == B)
            {
                this.ScaleImageByFixSize(width, height, true);
                return;
            }


            double W = b * HEIGHT;
            double X = (WIDTH - W) / 2;



            ClipImage((int)X, 0, (int)W, HEIGHT);


            ScaleImageByFixSize(width, height, true);

        }

        public void ClipScaleImageCenterByHeightWidth(int width, int height)
        {
            int WIDTH = this.Image.Width;
            int HEIGHT = this.Image.Height;

            double B = WIDTH * 1.0 / HEIGHT;
            double b = width * 1.0 / height;


            if (b == B)
            {
                this.ScaleImageByFixSize(width, height, true);
                return;
            }
            if (b > B)
            {
                double H = WIDTH / b;
                double Y = (HEIGHT - H) / 2;
                ClipImage(0, (int)Y, WIDTH, (int)H);

            }
            else
            {
                double W = b * HEIGHT;
                double X = (WIDTH - W) / 2;
                ClipImage((int)X, 0, (int)W, HEIGHT);
            }
            ScaleImageByFixSize(width, height, true);

        }

        public void ClipImageCenter()
        {
            int len = imgSource.Width > imgSource.Height ? imgSource.Height : imgSource.Width;
            ClipImageCenter(len, len);
        }
        #endregion

        #region 缩放图像

        public void ScaleImageByFixWidth(int destWidth)
        {
            int destHeight = destWidth * this.Image.Height / this.Image.Width;
            ScaleImageByFixSize(destWidth, destHeight, true);
        }

        public void ScaleImageByFixHeight(int destHeight)
        {
            int destWidth = destHeight * this.Image.Width / this.Image.Height;
            ScaleImageByFixSize(destWidth, destHeight, true);
        }

        #region 按指定比例缩放
        /// <summary>
        /// 按指定比例缩放。
        /// </summary>
        /// <param name="fltPercent">缩放比例，如1.3</param>
        public void ScaleImageByPercent(float fltPercent)
        {
            // 新尺寸
            int intNewWidth = Convert.ToInt32(imgSource.Width * fltPercent);
            int intNewHeight = Convert.ToInt32(imgSource.Height * fltPercent);
            // 新建BitMap对象，并设置主要属性
            bmpTemp = new Bitmap(intNewWidth, intNewHeight, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);
            bmpTemp.MakeTransparent(Color.White);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.Transparent);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0, Convert.ToSingle(intNewWidth), Convert.ToSingle(intNewHeight));

            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
        }
        #endregion

        #region 根据指定的尺寸缩放图象
        public void ScaleImageByFixSize(double intDestWidth, double intDestHeight, bool blnScratch)
        {
             ScaleImageByFixSize((int)intDestWidth, (int)intDestHeight, blnScratch);
        }
        /// <summary>
        ///  根据指定的尺寸缩放图象
        /// </summary>
        /// <param name="intDestWidth">缩放后的宽度</param> 
        /// <param name="intDestHeight">缩放后的高度</param> 
        /// <param name="blnScratch">如果指定高宽比与源图的比例不同，是否拉伸图象。</param>
        public void ScaleImageByFixSize(int intDestWidth, int intDestHeight, bool blnScratch)
        {
            if (!blnScratch)
            {
                // 如果不拉伸，则按比例缩放到最合适的大小。
                // 判断高宽比
                float fltHeightScale = Convert.ToSingle(intDestHeight) / Convert.ToSingle(imgSource.Height);
                float fltWidthScale = Convert.ToSingle(intDestWidth) / Convert.ToSingle(imgSource.Width);

                if (fltHeightScale <= fltWidthScale)
                {
                    ScaleImageByPercent(fltHeightScale);
                }
                else
                {
                    ScaleImageByPercent(fltWidthScale);
                }
            }
            else
            {
                // 新建BitMap对象，并设置主要属性
                bmpTemp = new Bitmap(intDestWidth, intDestHeight, PixelFormat.Format32bppArgb);
                bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

                // 建立Graphics对象，并设置主要属性
                Graphics g = Graphics.FromImage(bmpTemp);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.GammaCorrected;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;

                // 在画布上画图
                g.DrawImage(imgSource, 0, 0, Convert.ToSingle(intDestWidth), Convert.ToSingle(intDestHeight));

                // 释放资源
                imgSource.Dispose();
                imgSource = (Image)bmpTemp.Clone();
                g.Dispose();
                bmpTemp.Dispose();
            }
        }
        #endregion

        #endregion

        #region 将符合尺寸大小的图片直接加透明烙印
        /// <summary>
        /// 将符合尺寸大小的图片直接加透明烙印
        /// </summary>
        /// <param name="fileBgPath"></param>
        /// <param name="isTop"></param>
        /// <param name="tranf">透明度，如：0.5f（半透明）, 1 图像为不透明， 0 全透明</param>
        public void AddBackGroundToPic(string fileBgPath, float tranf)
        {
            bmpTemp = new Bitmap(imgSource.Size.Width, imgSource.Size.Height, PixelFormat.Format32bppArgb);

            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);

            Image imgbg = Image.FromFile(fileBgPath);
            imgbg = ImageManager.ChangeImage(imgbg, imgSource.Width, imgSource.Height);

            float[][] ptsArray ={ 
									new float[] {1, 0, 0, 0, 0},
									new float[] {0, 1, 0, 0, 0},
									new float[] {0, 0, 1, 0, 0},
									new float[] {0, 0, 0, 0.5f, 0}, //注意：此处为0.5f，图像为半透明
									new float[] {0, 0, 0, 0, 1}};

            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes imgAttributes = new ImageAttributes();
            //设置图像的颜色属性
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(this.imgSource, 0, 0);
            g.DrawImage(imgbg, new Rectangle(0, 0, imgbg.Size.Width, imgbg.Size.Height), 0, 0, imgbg.Size.Width, imgbg.Size.Height, GraphicsUnit.Pixel, imgAttributes);


            //g.DrawImage(imgSource,0,0,Convert.ToSingle(intNewWidth),Convert.ToSingle(intNewHeight));

            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
        }

        /// <summary>
        /// 将符合尺寸大小的图片直接加透明烙印
        /// </summary>
        /// <param name="fileBgPath"></param>
        /// <param name="isTop"></param>
        public void AddBackGroundToPic(string fileBgPath)
        {
            this.AddBackGroundToPic(fileBgPath, 0.3f);
        }
        #endregion

        #region 在图片上写字

        #region 在图片的指定位置上写字。
        /// <summary>
        /// 在图片的指定位置上写字。
        /// </summary>
        /// <param name="strTextContent">文本内容</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">风格</param>
        /// <param name="objColor">颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        public void WriteText(string strTextContent, string strFontName, FontStyle fontStyle, Color objColor, int x1, int y1, int x2, int y2)
        {
            WriteText(strTextContent, strFontName, fontStyle, objColor, x1, y1, x2, y2, Alignment.Left);
        }

        /// <summary>
        /// 在图片的指定位置上写字。
        /// </summary>
        /// <param name="strTextContent">文本内容</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">风格</param>
        /// <param name="objColor">颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="alignment">对齐方式</param>
        public void WriteText(string strTextContent, string strFontName, FontStyle fontStyle, Color objColor, int x1, int y1, int x2, int y2, Alignment alignment)
        {
            Write3DText(strTextContent, strFontName, fontStyle, objColor, x1, y1, x2, y2, 0, Direction.LeftDown, alignment);
        }
        #endregion

        #region 在图片的指定位置上写3D字。
        /// <summary>
        /// 在图片的指定位置上写3D字。
        /// </summary>
        /// <param name="strTextContent">文本内容</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">风格</param>
        /// <param name="objColor">颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="depth">3D字体宽度，以像素点为单位</param>
        /// <param name="direction">三维字体方向</param>
        public void Write3DText(string strTextContent, string strFontName, FontStyle fontStyle, Color objColor, int x1, int y1, int x2, int y2, int depth, Direction direction)
        {
            Write3DText(strTextContent, strFontName, fontStyle, objColor, x1, y1, x2, y2, depth, direction, Alignment.Left);
        }

        /// <summary>
        /// 在图片的指定位置上写3D字。
        /// </summary>
        /// <param name="strTextContent">文本内容</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">风格</param>
        /// <param name="objColor">颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="depth">3D字体宽度，以像素点为单位</param>
        /// <param name="direction">三维字体方向</param>
        /// <param name="alignment">文本对齐方式</param>
        public void Write3DText(string strTextContent, string strFontName, FontStyle fontStyle, Color objColor, int x1, int y1, int x2, int y2, int depth, Direction direction, Alignment alignment)
        {
            if (strTextContent == "" || strTextContent == null)
                return;

            // 格式化文本
            StringFormat sformat = new StringFormat();

            switch (alignment)
            {
                case Alignment.Center:
                    sformat.LineAlignment = StringAlignment.Center;
                    break;
                case Alignment.Left:
                    sformat.LineAlignment = StringAlignment.Near;
                    break;
                case Alignment.Right:
                    sformat.LineAlignment = StringAlignment.Far;
                    break;
            }

            bmpTemp = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0);

            // 定义画刷
            SolidBrush sb;

            // 创建所需对象
            Font objFont = new Font(strFontName, (float)16, fontStyle, GraphicsUnit.Pixel);

            #region 调整字体大小以适应指定区域

            float fltHeight = (float)y2 - (float)y1;
            float fltWidth = (float)x2 - (float)x1;
            SizeF sf;			// 当前大小
            int intStep = 5;		// 增加或者减少的步长，以象素为单位
            objFont = new Font(objFont.FontFamily.Name, fltHeight, objFont.Style, GraphicsUnit.Pixel);
            sf = g.MeasureString(strTextContent, objFont);
            if (sf.Width > fltWidth || sf.Height > fltHeight)
            {
                // 如果字太大了，就循环缩小，直到刚好符合为止
                while (sf.Width > fltWidth || sf.Height > fltHeight)
                {
                    // 将字体变小一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size - intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            else if (sf.Width < fltWidth && sf.Height < fltHeight)
            {
                // 如果字太小了（高和宽都达不到指定的区域）
                while (sf.Width < fltWidth && sf.Height < fltHeight)
                {
                    // 将字体变大一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size + intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            #endregion

            // 设置反锯齿
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region 根据深度值循环写3D字

            // 初始化灰度，下面将设置渐变色，使三维字能渐变
            /*byte red = 0;
            byte green = 0;
            byte blue = 0;
            byte alpha = 255;*/
            sb = new SolidBrush(Color.Black);//(Color.FromArgb(alpha,(int)red,(int)green,(int)blue));

            // 获取灰度增长阶级
            // byte step				=	(byte)(255 / depth);

            int intX = x1;
            int intY = y1;
            for (int i = 0; i < depth; i++)
            {
                // 设置写字的坐标
                if (direction == Direction.LeftDown || direction == Direction.LeftUp)
                    intX--;
                else
                    intX++;
                if (direction == Direction.LeftDown || direction == Direction.RightDown)
                    intY++;
                else
                    intY--;

                // 设置灰度
                /*red += step;
                green += step;
                blue += step;

                sb.Color = Color.FromArgb(alpha,(int)red,(int)green,(int)blue);*/
                g.DrawString(strTextContent, objFont, sb, intX, intY, sformat);
            }
            #endregion

            sb.Color = objColor;
            g.DrawString(strTextContent, objFont, sb, x1, y1, sformat);

            // 释放资源
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
            objFont.Dispose();
        }
        #endregion

        #region 在图上写镂空字
        /// <summary>
        /// 在图上写镂空字
        /// </summary>
        /// <param name="strTextContent">要写的文字</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="objBorderColor">边沿颜色</param>
        /// <param name="objFillColor">填充色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="width">镂空宽度</param>
        public void WriteOutLineText(string strTextContent, string strFontName, FontStyle fontStyle, Color objBorderColor, Color objFillColor, int x1, int y1, int x2, int y2, int width)
        {
            WriteOutLineText(strTextContent, strFontName, fontStyle, objBorderColor, objFillColor, x1, y1, x2, y2, width, Alignment.Left);
        }

        /// <summary>
        /// 在图上写镂空字
        /// </summary>
        /// <param name="strTextContent">要写的文字</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="objBorderColor">边沿颜色</param>
        /// <param name="objFillColor">填充色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="width">镂空宽度</param>
        /// <param name="alignment">对齐方式</param>
        public void WriteOutLineText(string strTextContent, string strFontName, FontStyle fontStyle, Color objBorderColor, Color objFillColor, int x1, int y1, int x2, int y2, int width, Alignment alignment)
        {
            if (strTextContent == "" || strTextContent == null)
                return;

            // 格式化文本
            StringFormat sformat = new StringFormat();

            switch (alignment)
            {
                case Alignment.Center:
                    sformat.LineAlignment = StringAlignment.Center;
                    break;
                case Alignment.Left:
                    sformat.LineAlignment = StringAlignment.Near;
                    break;
                case Alignment.Right:
                    sformat.LineAlignment = StringAlignment.Far;
                    break;
            }

            bmpTemp = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0);

            // 定义画刷
            SolidBrush sb = new SolidBrush(Color.White);

            // 创建所需对象
            Font objFont = new Font(strFontName, (float)16, fontStyle, GraphicsUnit.Pixel);

            #region 调整字体大小以适应指定区域

            float fltHeight = (float)y2 - (float)y1;
            float fltWidth = (float)x2 - (float)x1;
            SizeF sf;			// 当前大小
            int intStep = 5;		// 增加或者减少的步长，以象素为单位
            objFont = new Font(objFont.FontFamily.Name, fltHeight, objFont.Style, GraphicsUnit.Pixel);
            sf = g.MeasureString(strTextContent, objFont);
            if (sf.Width > fltWidth || sf.Height > fltHeight)
            {
                // 如果字太大了，就循环缩小，直到刚好符合为止
                while (sf.Width > fltWidth || sf.Height > fltHeight)
                {
                    // 将字体变小一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size - intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            else if (sf.Width < fltWidth && sf.Height < fltHeight)
            {
                // 如果字太小了（高和宽都达不到指定的区域）
                while (sf.Width < fltWidth && sf.Height < fltHeight)
                {
                    // 将字体变大一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size + intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            #endregion

            // 设置反锯齿
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region 根据深度值循环写镂空字

            sb.Color = objFillColor;//(Color.FromArgb(alpha,(int)red,(int)green,(int)blue));
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < width - 1; y++)
                {
                    g.DrawString(strTextContent, objFont, sb, new Point(x1 + x, y1 + y));
                    g.DrawString(strTextContent, objFont, sb, new Point(x1 - x, y1 - y));
                }
            }

            sb.Color = objBorderColor;
            g.DrawString(strTextContent, objFont, sb, new Point(x1, y1), sformat);
            #endregion

            // 释放资源
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
            objFont.Dispose();
        }
        #endregion

        #region 在图上写阴影字
        /// <summary>
        /// 在图上写阴影字
        /// </summary>
        /// <param name="strTextContent">要写的字</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="objFontColor">字体颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="distance">阴影距离</param>
        /// <param name="direction">阴影方向</param>
        public void WriteShadowText(string strTextContent, string strFontName, FontStyle fontStyle, Color objFontColor, int x1, int y1, int x2, int y2, int distance, Direction direction)
        {
            WriteShadowText(strTextContent, strFontName, fontStyle, objFontColor, x1, y1, x2, y2, distance, direction, Alignment.Left);
        }

        /// <summary>
        /// 在图上写阴影字
        /// </summary>
        /// <param name="strTextContent">要写的字</param>
        /// <param name="strFontName">字体名称</param>
        /// <param name="fontStyle">字体样式</param>
        /// <param name="objFontColor">字体颜色</param>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右上角x坐标</param>
        /// <param name="y2">右上角y坐标</param>
        /// <param name="distance">阴影距离</param>
        /// <param name="direction">阴影方向</param>
        /// <param name="alignment">对齐方式</param>
        public void WriteShadowText(string strTextContent, string strFontName, FontStyle fontStyle, Color objFontColor, int x1, int y1, int x2, int y2, int distance, Direction direction, Alignment alignment)
        {
            if (strTextContent == "" || strTextContent == null)
                return;

            // 格式化文本
            StringFormat sformat = new StringFormat();

            switch (alignment)
            {
                case Alignment.Center:
                    sformat.LineAlignment = StringAlignment.Center;
                    break;
                case Alignment.Left:
                    sformat.LineAlignment = StringAlignment.Near;
                    break;
                case Alignment.Right:
                    sformat.LineAlignment = StringAlignment.Far;
                    break;
            }

            bmpTemp = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0);

            // 定义画刷
            SolidBrush sb = new SolidBrush(Color.White);

            // 创建所需对象
            Font objFont = new Font(strFontName, (float)16, fontStyle, GraphicsUnit.Pixel);

            #region 调整字体大小以适应指定区域

            float fltHeight = (float)y2 - (float)y1;
            float fltWidth = (float)x2 - (float)x1;
            SizeF sf;			// 当前大小
            int intStep = 5;		// 增加或者减少的步长，以象素为单位
            objFont = new Font(objFont.FontFamily.Name, fltHeight, objFont.Style, GraphicsUnit.Pixel);
            sf = g.MeasureString(strTextContent, objFont);
            if (sf.Width > fltWidth || sf.Height > fltHeight)
            {
                // 如果字太大了，就循环缩小，直到刚好符合为止
                while (sf.Width > fltWidth || sf.Height > fltHeight)
                {
                    // 将字体变小一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size - intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            else if (sf.Width < fltWidth && sf.Height < fltHeight)
            {
                // 如果字太小了（高和宽都达不到指定的区域）
                while (sf.Width < fltWidth && sf.Height < fltHeight)
                {
                    // 将字体变大一点
                    objFont = new Font(objFont.FontFamily.Name, objFont.Size + intStep, objFont.Style, GraphicsUnit.Pixel);
                    sf = g.MeasureString(strTextContent, objFont);
                }
            }
            #endregion

            // 设置反锯齿
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region 根据方向写阴影字
            sb.Color = Color.FromArgb(100, Color.Black);
            // 设置写字的坐标
            int intX = x1;
            int intY = y1;
            if (direction == Direction.LeftDown || direction == Direction.LeftUp)
                intX = x1 - distance;
            else
                intX = x1 + distance;
            if (direction == Direction.LeftDown || direction == Direction.RightDown)
                intY = y1 + distance;
            else
                intY = y1 - distance;
            g.DrawString(strTextContent, objFont, sb, new Point(intX, intY), sformat);

            sb.Color = objFontColor;
            g.DrawString(strTextContent, objFont, sb, new Point(x1, y1), sformat);
            #endregion

            // 释放资源
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            g.Dispose();
            bmpTemp.Dispose();
            objFont.Dispose();
        }
        #endregion

        #endregion

        #region 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// <summary>
        /// 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// </summary>
        /// <param name="strImageFile">要贴的图像文件路径</param>
        /// <param name="intX">左上角x坐标</param>
        /// <param name="intY">左上角y坐标</param>
        public void PasteImage(string strImageFile, int intX, int intY)
        {
            PasteImage(strImageFile, intX, intY, Color.Empty);
        }

        /// <summary>
        /// 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// </summary>
        /// <param name="strImageFile">要贴的图像文件路径</param>
        /// <param name="intX">左上角x坐标</param>
        /// <param name="intY">左上角y坐标</param>
        /// <param name="objTransparentColor">透明色。</param>
        public void PasteImage(string strImageFile, int intX, int intY, Color objTransparentColor)
        {
            // 读取图像文件
            System.Drawing.Bitmap imgToBePasted = new Bitmap(Image.FromFile(strImageFile));

            // 设置透明色
            if (!objTransparentColor.IsEmpty)
            {
                imgToBePasted.MakeTransparent(objTransparentColor);
            }

            bmpTemp = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0);
            g.DrawImage(imgToBePasted, intX, intY);

            // 释放资源
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            imgToBePasted.Dispose();
            g.Dispose();
            bmpTemp.Dispose();
        }

        /// <summary>
        /// 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// </summary>
        /// <param name="imgToBePasted">要贴的图像</param>
        /// <param name="intX">左上角x坐标</param>
        /// <param name="intY">左上角y坐标</param>
        /// <param name="objTransparentColor">透明色。</param>
        public void PasteImage(System.Drawing.Bitmap imgToBePasted, int intX, int intY, Color objTransparentColor)
        {
            // 设置透明色
            if (!objTransparentColor.IsEmpty)
            {
                imgToBePasted.MakeTransparent(objTransparentColor);
            }

            bmpTemp = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format32bppArgb);
            bmpTemp.SetResolution(imgSource.HorizontalResolution, imgSource.VerticalResolution);

            // 建立Graphics对象，并设置主要属性
            Graphics g = Graphics.FromImage(bmpTemp);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.GammaCorrected;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在画布上画图
            g.DrawImage(imgSource, 0, 0);
            g.DrawImage(imgToBePasted, intX, intY);

            // 释放资源
            imgSource.Dispose();
            imgSource = (Image)bmpTemp.Clone();
            imgToBePasted.Dispose();
            g.Dispose();
            bmpTemp.Dispose();
        }


        /// <summary>
        /// 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// </summary>
        /// <param name="imgToBePasted">要贴的图像</param>
        /// <param name="intX">左上角x坐标</param>
        /// <param name="intY">左上角y坐标</param>
        /// <param name="objTransparentColor">透明色。</param>
        public void PasteImage(byte[] imgToBePasted, int intX, int intY, Color objTransparentColor)
        {
            var bitmap = ByteArrayToBitmap(imgToBePasted);
            PasteImage(bitmap, intX, intY, objTransparentColor);
        }

        /// <summary>
        /// 根据指定左上角坐标，在指定的图像上贴图。不改变尺寸将贴图贴在源图上。
        /// </summary>
        /// <param name="imgToBePasted">要贴的图像</param>
        /// <param name="intX">左上角x坐标</param>
        /// <param name="intY">左上角y坐标</param>
        /// <param name="objTransparentColor">透明色。</param>
        public void PasteImage(Image imgToBePasted, int intX, int intY, Color objTransparentColor)
        {
            var bitmap = new Bitmap(imgToBePasted);
            PasteImage(bitmap, intX, intY, objTransparentColor);
        }
        #endregion

        #region 用到的枚举
        /// <summary>
        /// 三维字体或阴影方向枚举
        /// </summary>
        public enum Direction : byte
        {
            RightUp,		// 右上角
            RightDown,		// 右下角
            LeftUp,			// 左上角
            LeftDown		// 左下角
        }

        /// <summary>
        /// 对齐枚举
        /// </summary>
        public enum Alignment : byte
        {
            Center,
            Right,
            Left
        }
        #endregion

        #region 释放资源 实现IDisposable
        // 释放资源
        public void Dispose()
        {
            if (bmpTemp != null)
            {
                bmpTemp.Dispose();
            }
            imgSource.Dispose();
        }
        #endregion

        #region 转换图片为指定大小和格式 ChangeImage
        /// <summary>
        /// 转换图片为指定大小和格式
        /// </summary>
        /// <param name="img">原图像</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        public static Image ChangeImage(Image img, int width, int height)
        {
            img = img.GetThumbnailImage(width, height, null, System.IntPtr.Zero);
            Bitmap bitmap = new Bitmap(img, width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制 原图片 对象
            graphics.DrawImage(img, new Rectangle(0, 0, width, height));
            return img;
        }
        #endregion

       
    }
}
