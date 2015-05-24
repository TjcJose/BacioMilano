using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BM.IO
{
    /// <summary>
    /// 取余文件夹计算
    /// </summary>
    public class ModFolderCalculate : IFolderCalculate
    {
        private int mod;

        /// <summary>
        /// 取余文件夹计算
        /// </summary>
        /// <param name="mod">模数</param>
        public ModFolderCalculate(int mod)
        {
            this.mod = mod;
        }

        /// <summary>
        /// 取余文件夹计算
        /// </summary>
        public ModFolderCalculate()
        {
            this.mod = 1024;
        }


        #region IFolderCalculate Members

        public string GetFolder(int factor)
        {
            return (factor % this.mod).ToString();
           
        }

        public string GetFolder(long factor)
        {
            return (factor % this.mod).ToString();
        }

        public int GetFactor(string aimFileNameWithExt)
        {
            return Math.Abs(aimFileNameWithExt.GetHashCode()) % this.mod;
        }

        public int GetFactor(int i)
        {
            return Math.Abs(i) % this.mod;
        }

        public long GetFactor(long i)
        {
            return Math.Abs(i) % this.mod;
        }

        #endregion
    }
}
