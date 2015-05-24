using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BM.Net
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public partial class T_DataHead
    {
        public T_DataHead()
        {
            iLen = 0;
            iType = 0;
            iBeginSign = UseBeginSign;
            iUnique = 0;
            iId = 0;
        }

        /// <summary>
        /// 开始标记
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string iBeginSign;

        private long _iLen;
        /// <summary>
        /// 后面数据的长度, 不包括此结构
        /// </summary>
        public long iLen
        {
            get
            {
                return IPAddressHelper.NetworkOrHostOrder(this._iLen);
            }
            set
            {
                this._iLen = IPAddressHelper.NetworkOrHostOrder(value);
            }
        }

        private int _iType;
        /// <summary>
        /// 传输的类型
        /// </summary>
        public int iType
        {
            get
            {
                return IPAddressHelper.NetworkOrHostOrder(this._iType);
            }
            set
            {
                this._iType = IPAddressHelper.NetworkOrHostOrder(value);
            }
        }

        private long _Id;
        /// <summary>
        /// 传输的ID
        /// </summary>
        public long iId
        {
            get
            {
                return IPAddressHelper.NetworkOrHostOrder(this._Id);
            }
            set
            {
                this._Id = IPAddressHelper.NetworkOrHostOrder(value);
            }
        }

        private int _iUnique;
        /// <summary>
        /// 传输的ID
        /// </summary>
        public int iUnique
        {
            get
            {
                return IPAddressHelper.NetworkOrHostOrder(this._iUnique);
            }
            set
            {
                this._iUnique = IPAddressHelper.NetworkOrHostOrder(value);
            }
        }
    }

    public partial class T_DataHead
    {
        public const int HeadLen = 34;
        public const string UseBeginSign = "124@44-+I"; 
        public static T_DataHead Instance()
        {
            T_DataHead h = new T_DataHead();
            h.iLen = 0;
            h.iType = 0;
            h.iBeginSign = UseBeginSign;
            h.iUnique = 0;
            return h;
        }
    }
}
