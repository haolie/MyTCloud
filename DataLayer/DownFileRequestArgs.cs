using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    /// <summary>
    /// 下载文件请求事件
    /// 2013-06-09 01:37:13by lyh
    /// </summary>
    public class DownFileRequestArgs : RequestArgs
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        /// <param name="e">The e.</param>
        /// <param name="o">The o.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="contextType">Type of the context.</param>
        /// <param name="filePath">The file path.</param>
        public DownFileRequestArgs(string s, RequestType t, EventHandler<RequestArgs> e, object o, int timeout = 0, string contextType = "application/x-www-form-urlencoded",string filePath="")
            :base(s,t,e,o,timeout ,contextType )

        {
            FilePath = filePath;
        }

        /// <summary>
        /// 文件下载路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        private byte[] _fileBuffer;
        /// <summary>
        /// 文件数组
        /// </summary>
        public byte[] FileBuffer
        {
            get { return _fileBuffer; }
            set { _fileBuffer = value; }
        }

    }
}
