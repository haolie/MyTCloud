using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    /// <summary> 
    /// 名称：中心Http请求接口参数
    /// 作者：Li
    /// 日期：2014-1-15
    /// </summary>
    public class RequestArgs : CommonRequestArgs
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestArgs(string s, RequestType t, EventHandler<RequestArgs> e, object o, int timeout = 5000, string contextType = "application/x-www-form-urlencoded")
            : base(o)
        {
            Uri = s;
            ////如果是post则默认contextType = "application/xml;charset=UTF-8"
            //if (t == RequestType.Post && contextType == "application/x-www-form-urlencoded")
            //{
            //    contextType = "application/xml;charset=UTF-8";
            //}
            Type = t;
            Handler = e;
            Timeout = timeout;
            ContextType = contextType;
        }
        /// <summary>
        /// http请求context类型
        /// </summary>
        public string ContextType
        {
            get;
            set;
        }

        /// <summary>
        /// 请求URI
        /// </summary>
        public string Uri
        {
            get;
            set;
        }

        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 回调事件
        /// </summary>
        public EventHandler<RequestArgs> Handler
        {
            get;
            set;
        }



    }
}
