using BM.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.DA
{
    public class EasyuiPageList<T>
    {
        public EasyuiPageList(IEnumerable<T> models, int pageSize, int pageIndex, int recordCount)
        {
            this.IsOK = true;
            this.rows = models;
            this.PageSize = pageSize;
            this.page = pageIndex;
            this.total = recordCount;
            this.PageCount = BM.DA.SplitPageHelper.GetPageCount(pageSize, recordCount);
            StartRecordIndex = (pageIndex - 1) * PageSize + 1;
            EndRecordIndex = recordCount > pageIndex * pageSize ? pageIndex * pageSize : recordCount;
        }

        public EasyuiPageList(IEnumerable<T> models, int pageSize, int pageIndex, int recordCount, int pageCount)
        {
            this.IsOK = true;
            this.rows = models;
            this.PageSize = pageSize;
            this.page = page;
            this.total = recordCount;
            this.PageCount = pageCount;
            StartRecordIndex = (pageIndex - 1) * PageSize + 1;
            EndRecordIndex = recordCount > pageIndex * pageSize ? pageIndex * pageSize : recordCount;
        }


        public EasyuiPageList(int pageSize)
        {
            this.IsOK = true;
            this.rows = new List<T>();
            this.PageSize = pageSize;
            this.page = 0;
            this.total = 0;
            this.PageCount = 0;
            this.StartRecordIndex = 0;
            this.EndRecordIndex = 0;
        }

        public EasyuiPageList(PageListModel<T> pageListModel)
        {
            this.IsOK = true;
            this.rows = pageListModel.Models;
            this.page = pageListModel.PageIndex;
            this.PageSize = pageListModel.PageSize;
            this.StartRecordIndex = pageListModel.StartRecordIndex;
            this.EndRecordIndex = pageListModel.EndRecordIndex;
            this.PageCount = pageListModel.PageCount;
            this.total = pageListModel.RecordCount;
        }

        public IEnumerable<T> rows { get; set; }

        /// <summary>
        /// pageIndex
        /// </summary>
        public int page { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        public int PageCount { get; private set; }
        public int StartRecordIndex { get; private set; }
        public int EndRecordIndex { get; private set; }
        public bool IsOK { get; set; }


    }
}
