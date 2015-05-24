using BM.Model.DbModel;
using BM.Model.EnumType;
using BM.Tools.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BM.Fw
{
    public static class BllUpload
    {
        //public static object lockObj = new object();

        //private static Timer timer;

        //static BllUpload()
        //{
        //    //定时删除过期的上传信息
        //    timer = new Timer(1000 * 60 * 60);
        //    timer.Elapsed += new ElapsedEventHandler(delegate(object obj, ElapsedEventArgs e)
        //    {
        //        List<string> ls = new List<string>();
        //        var t = WebUpload.UploadDataInfos.GetEnumerator();
        //        while (t.MoveNext())
        //        {
        //            if (t.Current.Value.CanDel)
        //            {
        //                ls.Add(t.Current.Key);
        //            }
        //        }

        //        foreach (string k in ls)
        //        {
        //            WebUpload.UploadDataInfos.Remove(k);
        //            DeleteTempFolder(k);
        //        }
        //    });
        //    timer.Start();
        //}

        //public static void Upload(string originalFilePath, long userId, UploadType uploadType)
        //{
            //PubFileMove filemove = new PubFileMove(uploadType.ToString());
            //var f = new FileInfo(originalFilePath);

            //var entity = new UUploadPub();
            //entity.Title = title;
            //entity.UserId = userId;
            //entity.UploadType = (int)uploadType;
            //entity.FileExt = Path.GetExtension(fileNameExtWith);
            //entity.FileNameNoExt = Path.GetFileNameWithoutExtension(fileNameExtWith);
            //entity.UDescription = description;
            //entity.FileSize = f.Length;
            //entity.FilePath = filemove.Move(originalFilePath, ConfigHelper.GetUploadConfig().DirPub, fileNameExtWith);
            //entity.FilePath = entity.FilePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory, "").Replace('\\', '/');
            //entity.UploadDate = DateTime.Now;
            //entity.UploadId = null;
            //Uploadpub.Insert(entity);

            //return entity;
        //}

        //public static string GetTempFolder(string folderName, bool isCreate)
        //{
        //    string folder = String.Format(@"{0}\upload", System.AppDomain.CurrentDomain.BaseDirectory);
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    folder = String.Format(@"{0}\temp", folder);
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    folder = String.Format(@"{0}\folderName", folder);
        //    if (!Directory.Exists(folder) && isCreate)
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    return folder;
        //}

        //public static void DeleteTempFolder(string folderName)
        //{
        //    string folder = String.Format(@"{0}\upload", System.AppDomain.CurrentDomain.BaseDirectory);
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    folder = String.Format(@"{0}\temp", folder);
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }

        //    folder = String.Format(@"{0}\folderName", folder);
        //    if(Directory.Exists(folder))
        //    {
        //        Directory.Delete(folder, true);
        //    }
        //}

        public static string GetUpload_Folder(long userId, long type, UploadType uploadType)
        {
            var config = ConfigHelper.Config_Instance;

            string folder = String.Format(@"{0}", config.UploadFolder);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            folder = String.Format(@"{0}\{1}", folder, BM.Util.EnumHelper.GetDescription<UploadType>((int)uploadType));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            folder = String.Format(@"{0}\{1}", folder, userId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            folder = String.Format(@"{0}\{1}", folder, type);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public static string GetUpload_Folder(long userId,  UploadType uploadType)
        {
            var config = ConfigHelper.Config_Instance;

            string folder = String.Format(@"{0}", config.UploadFolder);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            folder = String.Format(@"{0}\{1}", folder, BM.Util.EnumHelper.GetDescription<UploadType>((int)uploadType));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            folder = String.Format(@"{0}\{1}", folder, userId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public static string GetUpload_Image(T_Image image)
        {
            return string.Format(@"{0}\{1}{2}", GetUpload_Folder(image.UserId.Value, BM.Model.EnumType.UploadType.ImageType), image.ImageId.Value, image.ImageExt);
        }

        public static string GetUpload_Image_Url(T_Image image)
        {
            return string.Format(@"{0}/upload/images/{1}/{2}{3}", BM.Tools.Web.UrlInfo.UrlBase, image.UserId.Value, image.ImageId.Value, image.ImageExt);
        }

        public static string GetUpload_Category_Image(long platId, long categoryId)
        {
            return string.Format(@"{0}\{1}.png", GetUpload_Folder(platId, BM.Model.EnumType.UploadType.CategoryType), categoryId);
        }

        public static string GetUpload_Category_Image_Url(long platId, long categoryId)
        {
            return string.Format(@"{0}/upload/categorys/{1}/{2}.png", BM.Tools.Web.UrlInfo.UrlBase, platId, categoryId);
        }




        public static string GetUpload_Product_Image(long productId)
        {
            return string.Format(@"{0}\{1}.png", GetUpload_Folder(productId, BM.Model.EnumType.UploadType.ProductType), productId);
        }

        public static string GetUpload_Product_Image_Url(long productId)
        {
            return string.Format(@"{0}/upload/products/{1}/{2}.png", BM.Tools.Web.UrlInfo.UrlBase, productId, productId);
        }



        public static string GetUpload_ReplyMsgNew_Image(long userId, int eventType, int itemId)
        {
            return string.Format(@"{0}\{1}.png", GetUpload_Folder(userId, eventType, BM.Model.EnumType.UploadType.ReplyMsgNewType), itemId);
        }

        public static string GetUpload_ReplyMsgNew_Image_Url(long userId, int eventType, int itemId)
        {
            return string.Format(@"{0}/upload/replymsgnews/{1}/{2}/{3}.png", BM.Tools.Web.UrlInfo.UrlBase, userId, eventType, itemId);
        }



        public static string GetUpload_Article_Image(long userId, long articleId)
        {
            return string.Format(@"{0}\{1}.png", GetUpload_Folder(userId, BM.Model.EnumType.UploadType.ArticleType), articleId);
        }

        public static string GetUpload_Article_Image_Url(long userId, long articleId)
        {
            return string.Format(@"{0}/upload/articles/{1}/{2}.png", BM.Tools.Web.UrlInfo.UrlBase, userId, articleId);
        }

        public static string GetUpload_Article_Image2(long userId, long articleId)
        {
            return string.Format(@"{0}\{1}x.png", GetUpload_Folder(userId, BM.Model.EnumType.UploadType.ArticleType), articleId);
        }

        public static string GetUpload_Article_Image2_Url(long userId, long articleId)
        {
            return string.Format(@"{0}/upload/articles/{1}/{2}x.png", BM.Tools.Web.UrlInfo.UrlBase, userId, articleId);
        }

        public static string GetUpload_Site_Image(long userId)
        {
            return string.Format(@"{0}\{1}.png", GetUpload_Folder(userId, BM.Model.EnumType.UploadType.SiteType), userId);
        }

        public static string GetUpload_Site_Image_Url(long userId)
        {
            return string.Format(@"{0}/upload/sites/{1}/{2}.png", BM.Tools.Web.UrlInfo.UrlBase, userId, userId);
        }
    }
}
