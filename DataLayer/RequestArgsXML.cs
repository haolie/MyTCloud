using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataLayer
{
    /// <summary>
    /// 名称：中心Http请求接口参数(XML返回)
    /// 2014-07-11 03:22:59by lyh
    /// </summary>
    public class RequestArgsXML : RequestArgs
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
        public RequestArgsXML(string s, RequestType t, EventHandler<RequestArgsXML> e, object o, int timeout = 0, string contextType = "application/x-www-form-urlencoded")
            : base(s,t,null,o,timeout,contextType)
        {
            Handler = e;
        }

        /// <summary>
        /// Gets or sets the info.
        /// 2014-07-11 03:27:45 by lyh
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 返回結果
        /// 2014-07-11 03:29:12 by lyh
        /// </summary>
        public XmlElement XML { get; set; }

        /// <summary>
        /// 回调事件
        /// </summary>
        public new EventHandler<RequestArgsXML> Handler
        {
            get;
            set;
        }

    }
}
