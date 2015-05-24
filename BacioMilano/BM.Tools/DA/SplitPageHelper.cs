using System;
using System.Collections.Generic;
using System.Text;

namespace BM.DA
{
    /// <summary>
    /// ��ҳ������
    /// </summary>
    public static class SplitPageHelper
    {
        #region SetDigitInfo ���÷�ҳ��������Ϣ
        /// <summary>
        /// ���÷�ҳ��������Ϣ
        /// </summary>
        /// <param name="pageSize">��ҳ��С</param>
        /// <param name="recordCount">��¼����</param>
        /// <param name="pageIndex">����ĵ�ǰҳ</param>
        /// <param name="pageCount">��ҳ��</param>
        /// <param name="startPosition">��ҳ��ʾ������λ�ü�¼</param>
        /// <param name="endPosition">��ҳ��ʾ���ݽ���λ�ü�¼</param>
        /// <returns>����ʵ����ʾ��ǰҳ</returns>
        public static int SetDigitInfo(int pageSize, int recordCount, int pageIndex, out int pageCount, out int startPosition, out int endPosition)
        {
            pageCount = GetPageCount(pageSize, recordCount);
            startPosition = Math.Max((pageIndex - 1) * pageSize, 0);
            endPosition = Math.Min(pageIndex * pageSize - 1, recordCount - 1);
            return Math.Min(pageIndex, pageCount);
        }
        #endregion

        #region GetPageCount �õ���ҳ��
        /// <summary>
        /// ���÷�ҳ��������Ϣ
        /// </summary>
        /// <param name="pageSize">��ҳ��С</param>
        /// <param name="recordCount">��¼����</param>
        /// <returns>��ҳ��</returns>
        public static int GetPageCount(int pageSize, int recordCount)
        {
            return (recordCount / pageSize) + (recordCount % pageSize > 0 ? 1 : 0);
        }
        #endregion

        #region GetTopNum �õ���ҳʱ����ʾҳ�������ʾ��¼����
        /// <summary>
        /// �õ���ҳʱ����ʾҳ�������ʾ��¼����
        /// </summary>
        /// <param name="currentPage">��ǰҳ��</param>
        /// <param name="pagesize">ÿҳ�涨��ʾ����¼����</param>
        /// <param name="count">��������</param>
        /// <returns>�õ���ҳʱ����ʾҳ�������ʾ��¼����</returns>
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

        #region SplitSortFields ��������ֶ�
        /// <summary>
        /// ��������ֶ�,����������������ֶ���
        /// </summary>
        /// <param name="orderBy">����:��field1 asc, field2 desc���������� order by</param>
        /// <param name="fields">�����ֶ�</param>
        /// <param name="ascDescs">������</param>
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
