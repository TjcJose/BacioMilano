using System;

namespace BM.DA
{
    /// <summary>
    /// 数据读取接口
    /// </summary>
    public interface IColumnUseReader
    {
        /// <summary>
        /// 获得列名称
        /// </summary>
        /// <param name="i">列序号</param>
        /// <returns>列名称</returns>
        string GetName(int i);

        /// <summary>
        /// 获得列值
        /// </summary>
        /// <param name="i">列序号</param>
        /// <returns>列值</returns>
        object GetValue(int i);

        /// <summary>
        /// 获取列长度
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 关闭读取对象
        /// </summary>
        void Close();
    }
}
