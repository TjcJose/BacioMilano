using System;
using System.Data;

namespace BM.DA
{
    /// <summary>
    /// 列数据读取DataRow的实现
    /// </summary>
    public class DataRowUseReader : IColumnUseReader
    {
        private DataRow dr;

        /// <summary>
        /// 列数据读取DataRow的实现
        /// </summary>
        /// <param name="dr">DataRow 对象</param>
        public DataRowUseReader(DataRow dr)
        {
            this.dr = dr;
        }
        #region IColumnReader Members

        /// <summary>
        /// 得到列名称
        /// </summary>
        /// <param name="i">索引号</param>
        /// <returns>列名称</returns>
        public string GetName(int i)
        {
            return this.dr.Table.Columns[i].ColumnName;
        }

        /// <summary>
        /// 得到某索引值
        /// </summary>
        /// <param name="i">索引号</param>
        /// <returns>索引值</returns>
        public object GetValue(int i)
        {
            return this.dr[i];
        }

        /// <summary>
        /// 列长度
        /// </summary>
        public int Length
        {
            get { return this.dr.ItemArray.Length; }
        }

        #endregion

        #region IColumnUseReader 成员

        /// <summary>
        /// 关闭读取对象
        /// </summary>
        public void Close()
        {

        }

        #endregion
    }
}
