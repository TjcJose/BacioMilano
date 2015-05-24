using System;
using System.Data;

namespace BM.DA
{
    /// <summary>
    /// 列数据读取DataReader的实现
    /// </summary>
    public class DataReaderUse : IColumnUseReader
    {
        private IDataReader reader;

        /// <summary>
        /// IDataReader列数据读取DataReader的实现
        /// </summary>
        /// <param name="reader">DataReader 对象</param>
        public DataReaderUse(IDataReader reader)
        {
            this.reader = reader;
        }
        #region IColumnReader Members

        /// <summary>
        /// 得到列名称
        /// </summary>
        /// <param name="i">索引号</param>
        /// <returns>列名称</returns>
        public string GetName(int i)
        {
            return this.reader.GetName(i);
        }

        /// <summary>
        /// 得到某索引值
        /// </summary>
        /// <param name="i">索引号</param>
        /// <returns>索引值</returns>
        public object GetValue(int i)
        {
            return this.reader.GetValue(i);
        }

        /// <summary>
        /// 列长度
        /// </summary>
        public int Length
        {
            get { return this.reader.FieldCount; }
        }

        #endregion

        #region IColumnUseReader 成员

        /// <summary>
        /// 关闭读取对象
        /// </summary>
        public void Close()
        {
            this.reader.Close();
        }

        #endregion
    }
}
