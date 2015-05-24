using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.Tools.Web
{
    /// <summary>
    /// Web上传数据描述信息
    /// </summary>
    public sealed class WebUploadData
    {
        public string FileName { get; internal set; }
        public int TotalSize { get; internal set; }
        public int LoadedSize { get; internal set; }

        public DateTime BeginTime { get; internal set; }
       
        public DateTime EndTime { get; internal set; }
  
        public int ReaminSize
        {
            get
            {
                return this.TotalSize - this.LoadedSize;
            }
        }

        public bool IsFinish
        {
            get
            {
                return this.TotalSize == this.LoadedSize;
            }
        }

        public bool CanDel
        {
            get
            {
                if (this.IsFinish && this.EndTime.AddMinutes(20) < DateTime.Now)
                    return true;

                if (this.BeginTime.AddDays(1) < DateTime.Now)
                {//下载时间超过一天，可删除
                    return true;
                }
                return false;
            }
            set
            {
                this.EndTime = DateTime.Now.AddMilliseconds(-1);
            }
        }

        public bool Cancel
        {
            get;
            set;
        }

    }
}
