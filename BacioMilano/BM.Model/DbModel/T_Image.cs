using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.DbModel
{
    public partial  class T_Image
    {
        public string ImageSizeStr
        {
            get
            {
                return BM.IO.FileSizeConvert.FormatBytes((long)this.ImageSize.Value);
            }
        }

        public string ImageUrl
        {
            get;set;
        }
    }
}
