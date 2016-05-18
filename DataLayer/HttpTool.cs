using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tools;
using System.Threading;
using System.Xml;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace DataLayer
{
    public static class HttpTool
    {
        #region 成员变量

        //编码
        private static readonly Encoding Encoding = Encoding.UTF8;
        // 会话cookie
        private static string _cookie = "";

        private static readonly int BufferLen = 4096;

        private static int _requestTimeOut = 300000;
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public static int RequestTimeOut = 6000;
     

        #endregion


        #region 公共属性


        #endregion

        #region 公共方法

        /// <summary>
        /// Http请求
        /// </summary>
        public static void Request(RequestArgs args, bool isGZip = false)
        {

            if (args.Type == RequestType.Get)
            {
                try
                {

                    new Thread(() =>
                    {
                        try
                        {
                            RequestThread(args, isGZip);
                        }
                        catch (Exception e)
                        {
                            args.Exception = e;
                        }
                        RequestComplete( args);
                    }) { IsBackground = true, Name = "中心Request请求线程" }.Start();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Post(args, isGZip);
            }
        }



        /// <summary>
        /// 上传文件
        /// </summary>
        public static void UploadFile(RequestArgs args, string filePath)
        {

            new Thread(() =>
            {
                try
                {
                    UploadFileThread(args, filePath);
                    args.IsSuccess = true;
                }
                catch (Exception e)
                {
                    args.Exception = e;
                }
                RequestComplete(args);
            }
                ).Start();
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        public static void UploadFiles(RequestArgs args, string[] filePaths)
        {

            new Thread(() =>
            {
                try
                {
                    args.Ret = SxHttpWeb.UploadFiles(args.Uri, filePaths, false);
                    args.IsSuccess = true;
                }
                catch (Exception e)
                {
                    args.Exception = e;
                }
                RequestComplete( args);
            }
                ).Start();
        }

        /// <summary>
        /// 返回xml解析
        /// </summary>
        /// <param name="xml">xml对象</param>
        /// <param name="msg">返回信息</param>
        /// <param name="code">返回代码</param>
        /// <returns>code</returns>
        public static int CenterReturenXmlToState(string xml, ref string msg)
        {
            int code = -1;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            if (xmlDoc.ChildNodes.Count > 0)
            {
                XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("Response");
                if (xmlNodeList.Count > 0 && xmlNodeList[0].Attributes.Count > 0)
                {
                    if (xmlNodeList[0].Attributes["Code"] != null)
                    {
                        try
                        {
                            code = Int32.Parse(xmlNodeList[0].Attributes["Code"].Value);
                        }
                        catch (Exception e)
                        {
                        }
                        if (xmlNodeList[0].Attributes["Message"] != null)
                        {
                            msg = xmlNodeList[0].Attributes["Message"].Value;
                            if (msg == "")
                            {
                                if (xmlNodeList[0].ChildNodes.Count > 0 && xmlNodeList[0].ChildNodes[0].Attributes["Id"] != null)
                                {
                                    msg = xmlNodeList[0].ChildNodes[0].Attributes["Id"].Value;
                                }
                            }
                            // LogManager.Instanse.DebugInfo(msg);
                        }
                    }
                }
            }
            return code;
        }

        /// <summary>
        /// Gets the requst XML.
        /// 2014-07-11 03:35:04 by lyh
        /// </summary>
        /// <returns></returns>
        public static XmlElement GetRequstXML(string rep, out bool isSucess, out string info)
        {
            isSucess = false;
            info = "";

            if (string.IsNullOrEmpty(rep))
            {
                info = "未知错误";
                return null;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rep);

                string code = doc.DocumentElement.GetAttribute("Code");
                isSucess = true;
                if (code != "200")
                {
                    isSucess = false;
                    info = code;
                }
                else if (doc.DocumentElement.HasAttribute("Message"))
                {
                    info = doc.DocumentElement.GetAttribute("Message");
                }

                return doc.DocumentElement;
            }
            catch
            {
                info = "未知错误";
                return null;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 请求数据线程
        /// </summary>
        /// <param name="args"></param> 
        /// <param name="isGZip">是否压缩</param>
        private static void RequestThread(RequestArgs args, bool isGZip = false)
        {
            int maxTry = 1;
            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(new Uri(args.Uri));
                    req.Method = "GET";

                    //这里必须设置代理参数，否则请求会非常慢(http://holyhoehle.wordpress.com/2010/01/12/webrequest-slow/)
                    req.Proxy = null;

                    if (_cookie != "" && _cookie != null)
                    {
                        req.Headers["Cookie"] = _cookie;
                    }

                    req.Headers["Accept-Encoding"] = "gzip, deflate";

                    if (args.Timeout > 0)
                    {
                        req.Timeout = args.Timeout;
                    }

                    using (WebResponse wr = req.GetResponse())
                    {

                        using (Stream sm = wr.GetResponseStream())
                        {
                            if (isGZip)
                            {
                                GZipStream gz = new GZipStream(sm, CompressionMode.Decompress);

                                using (StreamReader sr = new StreamReader(gz, Encoding))
                                {
                                    string ret = sr.ReadToEnd();
                                    args.Ret = ret;
                                    isSuccess = true;
                                }
                            }
                            else
                            {
                                using (StreamReader sr = new StreamReader(sm, Encoding))
                                {
                                    string ret = sr.ReadToEnd();
                                    args.Ret = ret;
                                    isSuccess = true;
                                }
                            }

                        }
                        wr.Close();

                    }

                }
                catch (WebException we)
                {
                    args.IsSuccess = false;
                    if (we.Status == WebExceptionStatus.Timeout)
                    {
                        args.Ret = "请求超时";
                        if (args.IsCancel)
                        {
                            args.Ret = "取消请求";
                            break;
                            //throw new Exception("取消请求");
                        }
                        maxTry--;
                        if (maxTry == 0)
                        {
                            args.Ret = "连接服务器超时";
                            break;
                            //throw new Exception("连接服务器超时");
                        }
                        continue;
                    }
                    else
                    {
                        args.Exception = we;
                        break;
                    }
                }

                args.IsSuccess = isSuccess;
            }
        }

        /// <summary>
        /// 请求完成
        /// </summary>
        private static void RequestComplete( RequestArgs args)
        {
         
            //dispatcher.Invoke(new Action(() =>
            //{
            EventHandler<RequestArgs> handler = args.Handler;
            if (args is RequestArgsXML)
            {
                RequestArgsXML xmlArgs = args as RequestArgsXML;
                if (args.IsSuccess)
                {
                    bool isSucess = true;
                    string info = "";
                    xmlArgs.XML = GetRequstXML(xmlArgs.Ret, out isSucess, out info);
                    xmlArgs.Info = info;
                    xmlArgs.IsSuccess = isSucess;
                }
                else if (args.Exception != null)
                {
                    xmlArgs.Info = xmlArgs.Exception.Message;
                }

                if (xmlArgs.Handler != null)
                    xmlArgs.Handler(null, xmlArgs);
            }

            if (handler != null)
            {
                handler(null, args);
            }
            //}));
        }

        /// <summary>
        /// 上传数据请求
        /// </summary>
        /// <param name="args"></param>
        /// <param name="isGZip"></param>
        private static void Post(RequestArgs args, bool isGZip = false)
        {
            new Thread(() =>
            {
                try
                {
                    PostThread(args, isGZip);
                    args.IsSuccess = true;
                }
                catch (Exception e)
                {
                    args.Exception = e;
                    args.IsSuccess = false;
                }
                RequestComplete( args);

            }
                ).Start();
        }

        /// <summary>
        /// 上传请求线程
        /// </summary>
        private static void PostThread(RequestArgs args, bool isGZip = false)
        {
            args.Ret = SxHttpWeb.Post(args.Uri, ref _cookie, args.ContextType, isGZip, args.Timeout);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private static void UploadFileThread(RequestArgs args, string filePath)
        {
            args.Ret = SxHttpWeb.UploadFile(args.Uri, filePath, false);
        }

        private static bool _isDownloading = false;
        private static List<DownFileRequestArgs> _downArray = new List<DownFileRequestArgs>();


        static object _obj = new object();

        /// <summary>
        /// 图片下载
        /// </summary>
        /// <param name="args"></param>
        public static void DownloadImag(DownFileRequestArgs args)
        {

            new Thread(() =>
            {
                WebRequest req = HttpWebRequest.Create(new Uri(args.Uri));
                req.Proxy = null;
                WebResponse wr = req.GetResponse();
                Stream sm = wr.GetResponseStream();

                //当前只是下载图片，所以只判定image类型，以后有新增类型则需要添加
                if (wr.ContentType == "image/jpeg" || wr.ContentType == "image/jpeg;charset=UTF-8")
                //if (true)
                {
                    Console.WriteLine("---------------" + args.FilePath + "-------------" + DateTime.Now.ToLongTimeString());
                    if (!File.Exists(args.FilePath))
                    {
                        DirectoryTool.CreateFileEx(args.FilePath);
                    }
                    //else
                    //{
                    //    File.Delete(args.FilePath);
                    //}

                    FileStream fs = new FileStream(args.FilePath, FileMode.Create, FileAccess.ReadWrite);

                    Byte[] buffer = new Byte[BufferLen];
                    int readBytes = 0;
                    while ((readBytes = sm.Read(buffer, 0, BufferLen)) > 0)
                    {
                        fs.Write(buffer, 0, readBytes);
                    }

                    fs.Flush();
                    fs.Close();

                    args.Ret = "1";
                    args.IsSuccess = true;
                }
                else
                {
                    using (StreamReader sr = new StreamReader(sm, Encoding))
                    {
                        string ret = sr.ReadToEnd();
                        args.Ret = ret;
                    }
                }
                wr.Close();
                RequestComplete(args);

            }) { IsBackground = true }.Start();

        }
        /// <summary>
        /// 下载文件
        /// </summary>
        private static void DownloadFileThread(DownFileRequestArgs args)
        {
            WebRequest req = HttpWebRequest.Create(new Uri(args.Uri));
            req.Proxy = null;
            WebResponse wr = req.GetResponse();
            Stream sm = wr.GetResponseStream();

            //当前只是下载图片，所以只判定image类型，以后有新增类型则需要添加
            if (wr.ContentType == "image/jpeg")
            //if (true)
            {

                if (!File.Exists(args.FilePath))
                {
                    DirectoryTool.CreateFileEx(args.FilePath);
                }

                FileStream fs = new FileStream(args.FilePath, FileMode.Create, FileAccess.ReadWrite);

                Byte[] buffer = new Byte[BufferLen];
                int readBytes = 0;
                while ((readBytes = sm.Read(buffer, 0, BufferLen)) > 0)
                {
                    fs.Write(buffer, 0, readBytes);
                }

                fs.Flush();
                fs.Close();

                args.Ret = "1";
                args.IsSuccess = true;
            }
            else
            {
                using (StreamReader sr = new StreamReader(sm, Encoding))
                {
                    string ret = sr.ReadToEnd();
                    args.Ret = ret;
                }
            }

            wr.Close();
        }

        #endregion
    }
}
