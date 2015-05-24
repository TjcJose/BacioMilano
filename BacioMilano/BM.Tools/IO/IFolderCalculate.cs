using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.IO
{
    /// <summary>
    /// 文件夹计算接口
    /// </summary>
    public interface IFolderCalculate
    {
        /// <summary>
        /// 取模得到放置文件夹
        /// </summary>
        /// <param name="factor">计算因子</param>
        /// <returns>放置的文件夹名称</returns>
        string GetFolder(int factor);

        /// <summary>
        /// 取模得到放置文件夹
        /// </summary>
        /// <param name="factor">计算因子</param>
        /// <returns>放置的文件夹名称</returns>
        string GetFolder(long factor);

        /// <summary>
        /// 取得计算因子
        /// </summary>
        /// <param name="aimFileNameWithExt">目标文件名称</param>
        /// <returns>计算因子</returns>
        int GetFactor(string aimFileNameWithExt);

        /// <summary>
        /// 取得计算因子
        /// </summary>
        /// <param name="i">产生因子整数</param>
        /// <returns>计算因子</returns>
        int GetFactor(int i);

        /// <summary>
        /// 取得计算因子
        /// </summary>
        /// <param name="i">产生因子整数</param>
        /// <returns>计算因子</returns>
        long GetFactor(long i);
    }
}
