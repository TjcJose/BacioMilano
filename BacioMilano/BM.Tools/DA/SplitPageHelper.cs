using System;
using System.Collections.Generic;
using System.Text;

namespace BM.DA
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    public static class SplitPageHelper
    {
        #region SetDigitInfo 设置分页的数字信息
        /// <summary>
        /// 设置分页的数字信息
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">记录总数</param>
        /// <param name="pageIndex">输入的当前页</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="startPosition">分页显示数据起位置记录</param>
        /// <param name="endPosition">分页显示数据结束位置记录</param>
        /// <returns>返回实际显示当前页</returns>
        public static int SetDigitInfo(int pageSize, int recordCount, int pageIndex, out int pageCount, out int startPosition, out int endPosition)
        {
            pageCount = GetPageCount(pageSize, recordCount);
            startPosition = Math.Max((pageIndex - 1) * pageSize, 0);
            endPosition = Math.Min(pageIndex * pageSize - 1, recordCount - 1);
            return Math.Min(pageIndex, pageCount);
        }
        #endregion

        #region GetPageCount 得到总页数
        /// <summary>
        /// 设置分页的数字信息
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">记录总数</param>
        /// <returns>总页数</returns>
        public static int GetPageCount(int pageSize, int recordCount)
        {
            return (recordCount / pageSize) + (recordCount % pageSize > 0 ? 1 : 0);
        }
        #endregion

        #region GetTopNum 得到分页时所显示页的最大显示记录个数
        /// <summary>
        /// 得到分页时所显示页的最大显示记录个数
        /// </summary>
        /// <param name="currentPage">当前页数</param>
        /// <param name="pagesize">每页规定显示最大记录个数</param>
        /// <param name="count">数据总数</param>
        /// <returns>得到分页时所显示页的最大显示记录个数</returns>
        public static int GetTopNum(int currentPage, int pagesize, int count)
        {
            int topNum = currentPage * pagesize;
            if (count - topNum < 0)
            {
                topNum = pagesize - topNum + count;
                if (topNum < pagesize)
                {
                    topNum = count;
                }
            }
            return topNum;
        }
        #endregion

        #region SplitSortFields 拆分排序字段
        /// <summary>
        /// 拆分排序字段,并添加主键到排序字段中
        /// </summary>
        /// <param name="orderBy">排序:如field1 asc, field2 desc，但不包含 order by</param>
        /// <param name="fields">排序字段</param>
        /// <param name="ascDescs">升降序</param>
        public static string SplitSortFields(string orderBy, out List<string> fields, out List<string> ascDescs, string[] primaryFields)
        {
            fields = new List<string>();
            ascDescs = new List<string>();
            if (!String.IsNullOrWhiteSpace(orderBy))
            {
                string[] sortfields = orderBy.Split(',');
                char[] split = { ' ' };
                for (int i = 0; i < sortfields.Length; i++)
                {
                    var arr = sortfields[i].Split(split, StringSplitOptions.RemoveEmptyEntries);
                    fields.Add(arr[0]);
                    ascDescs.Add(arr[1]);
                }
            }
            if (primaryFields != null)
            {
                foreach (string primaryField in primaryFields)
                {
                    if (!fields.Contains(primaryField))
                    {
                        if (fields.Count == 0)
                        {
                            fields.Add(primaryField);
                            ascDescs.Add("desc");
                            orderBy = primaryField + " desc";
                        }
                        else
                        {
                            fields.Add(primaryField);
                            ascDescs.Add("desc");
                            orderBy += "," + primaryField + " desc";
                        }
                    }
                }
            }
            return orderBy;

        }
        #endregion
    }
}
