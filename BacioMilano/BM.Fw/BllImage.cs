using BM.DA;
using BM.GDI;
using BM.Model.DbModel;
using BM.Model.EnumType;
using BM.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Data;
using BM.Model.VModel;

namespace BM.Fw
{
    public static class BllImage
    {
        internal static MappingBase<T_Image> map = SingletonHelper<ModelDAL<T_Image>>.Instance.Mapping;

        public static PageListModel<T_Image> SelectSplit(ImageSearchModel opts, long? userIdS, int pageSize, int pageIndex)
        {
            List<object> ls = new List<object>();

            StringBuilder sb = new StringBuilder("1=1");

            if (userIdS != null)
            {
                sb.AppendFormat(@" and #{0} = @{0}", T_Image_Description.UserId);
                ls.Add(userIdS);
            }
           
            if (opts.ImageNameS != null)
            {
                sb.AppendFormat(@" and #{0} like @{0}", T_Image_Description.ImageName);
                ls.Add("%" + opts.ImageNameS + "%");
            }

            int pageCount, recordCount;
            var datas = map.SelectSplit(null, sb.ToString(), ls.ToArray(), false, pageIndex, pageSize, out pageCount, out recordCount);
            foreach (var data in datas)
            {
                String filePath = BllUpload.GetUpload_Image(data);
                if (System.IO.File.Exists(filePath))
                {
                    data.ImageUrl = BllUpload.GetUpload_Image_Url(data);
                }
                else
                {
                    data.ImageUrl = "";
                }
            }

            if (datas.Count == 0)
            {
                return new PageListModel<T_Image>(pageSize);
            }
            return new PageListModel<T_Image>(datas, pageSize, pageIndex, recordCount, pageCount);
        }

        public static T_Image GetById(long id)
        {
            return map.GetEntityById(id);
        }



        public static T_Image Add(long userId, string imageName, HttpPostedFileBase imageFile)
        {
            string folder = BllUpload.GetUpload_Folder(userId, UploadType.ImageType);
            T_Image entity = new T_Image();
            entity.UserId = userId;
            entity.ImageName = imageName;
            entity.ImageExt = Path.GetExtension(imageFile.FileName);
            entity.ImageSize = imageFile.ContentLength;
            entity.ImageId = BM.Util.ObjectConvert.GetLongValue(map.InsertReturnIdentity(entity));
            if (entity.ImageId > 0)
            {
                try
                {
                    string filePath = String.Format(@"{0}\{1}{2}", folder, entity.ImageId.Value, entity.ImageExt);
                    imageFile.SaveAs(filePath);
                    ImageManager m = new ImageManager(filePath);
                    entity.ImageWidth = m.Image.Width;
                    entity.ImageHeight = m.Image.Height;
                    m.Dispose();
                    map.Update(entity);
                }
                catch 
                {
                    map.DeleteById(entity.ImageId);
                    return null;
                }
            }
            return entity;
        }

        public static T_Image Modify(long imageId, string imageName, HttpPostedFileBase imageFile)
        {
            var entity = BllImage.GetById(imageId);
            if (imageFile == null || imageFile.ContentLength == 0)
            {
                entity.ImageName = imageName;
                entity.Update();
            }
            else
            {
                string folder = BllUpload.GetUpload_Folder(entity.UserId.Value, UploadType.ImageType);
                string filePath = String.Format(@"{0}\{1}{2}", folder, entity.ImageId.Value, entity.ImageExt);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    imageFile.SaveAs(filePath);
                    ImageManager m = new ImageManager(filePath);
                    entity.ImageWidth = m.Image.Width;
                    entity.ImageHeight = m.Image.Height;
                    m.Dispose();
                    entity.ImageExt = Path.GetExtension(imageFile.FileName);
                    entity.ImageSize = imageFile.ContentLength;
                    entity.ImageName = imageName;
                    map.Update(entity);
                }
            }
            return entity;
        }

        public static bool Delete(long imageId, long userId)
        {
            using (var conn = map.CreateConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var ts = conn.BeginTransaction())
                {
                    T_Image image = map.GetEntityById(imageId, conn, ts);
                    if (image != null)
                    {

                        if (map.DeleteById(imageId, conn, ts) > 0)
                        {
                            ts.Commit();

                            string filePath = BllUpload.GetUpload_Image(image);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
    }
}
