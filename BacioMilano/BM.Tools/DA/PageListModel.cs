using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.DA
{
    public class PageListModel<T>
    {
        public PageListModel(IEnumerable<T> models, int pageSize, int pageIndex, int recordCount)
        {
            this.IsOK = true;
            this.Models = models;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.RecordCount = recordCount;
            this.PageCount = BM.DA.SplitPageHelper.GetPageCount(pageSize, recordCount);
            StartRecordIndex = (pageIndex - 1) * PageSize + 1;
            EndRecordIndex = RecordCount > pageIndex * pageSize ? pageIndex * pageSize : RecordCount;
        }

        public PageListModel(IEnumerable<T> models, int pageSize, int pageIndex, int recordCount, int pageCount)
        {
            this.IsOK = true;
            this.Models = models;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.RecordCount = recordCount;
            this.PageCount = pageCount;
            StartRecordIndex = (pageIndex - 1) * PageSize + 1;
            EndRecordIndex = RecordCount > pageIndex * pageSize ? pageIndex * pageSize : RecordCount;
        }


        public PageListModel(int pageSize)
        {
            this.IsOK = true;
            this.Models = new List<T>();
            this.PageSize = pageSize;
            this.PageIndex = 0;
            this.RecordCount = 0;
            this.PageCount = 0;
            this.StartRecordIndex = 0;
            this.EndRecordIndex = 0;
        }

        public IEnumerable<T> Models { get; set; }


        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
        public int PageCount { get; private set; }
        public int StartRecordIndex { get; private set; }
        public int EndRecordIndex { get; private set; }
        public bool IsOK { get; set; }

      
    }
}
