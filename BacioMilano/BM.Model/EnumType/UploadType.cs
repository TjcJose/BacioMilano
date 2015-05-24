using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    public enum UploadType
    {
        [Description("Images")]
        ImageType = 1,

        [Description("Categorys")]
        CategoryType = 2,

        [Description("Articles")]
        ArticleType = 3,

        [Description("Sites")]
        SiteType = 4,
        
        [Description("ReplyMsgNews")]
        ReplyMsgNewType = 5,

        [Description("Products")]
        ProductType = 6
    }
}
