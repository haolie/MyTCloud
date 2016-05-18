using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataLayer
{
    /// <summary>
    ///CLR版本:4.0.30319.18051
    ///   名称:CommonRequestArgs
    ///   作者:lijun		
    ///   日期:6/24/2014 9:49:01 AM
    ///   描述:
    /// </summary>
   public class CommonRequestArgs:EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
       public CommonRequestArgs(object arg)
        {
            Arg = arg;
        } 
       /// <summary>
       /// 回调对象
       /// </summary>
       public object Arg { get; set; }

       /// <summary>
       /// 返回对象
       /// </summary>
       public virtual string Ret { get;  set; } 

       /// <summary>
       /// 异常信息
       /// </summary>
       public Exception Exception
       {
           get;
           set;
       }

       /// <summary>
       /// 是否取消请求
       /// </summary>
       public bool IsCancel
       {
           get;
           set;
       }
       /// <summary>
       /// 是否成功
       /// </summary>
       public bool IsSuccess
       {
           get;
           set;

       }


       /// <summary>
       /// 超时时间
       /// </summary>
       public int Timeout
       {
           get;
           set;
       }

    
    }
}
