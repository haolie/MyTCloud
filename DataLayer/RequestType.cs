using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    /// <summary>
    /// 中心Http请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Get请求
        /// </summary>
        Get,
        /// <summary>
        /// Post请求
        /// </summary>
        Post,
        /// <summary>
        /// 下载文件
        /// </summary>
        DownloadFile,
        /// <summary>
        /// 
        /// </summary>
        UploadFile
    }
}
