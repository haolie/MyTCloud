using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCloud.CosApi.Api;
using QCloud.CosApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QCloud.CosApi
{
    class Program
    {
        const int APP_ID = 10028297;
        const string SECRET_ID = "AKIDmmGfV59KBtcPB1kQCh2ElU6ytlUQDAUC";
        const string SECRET_KEY = "g2fvgK5hpAIk2KEYPUfmBelxwUhqs2iY";
        static void Main(string[] args)
        {
            try
            {
                var result = "";
                var bucketName = "sparkmoon";
                var cos = new CosCloud(APP_ID, SECRET_ID, SECRET_KEY);
                var start = DateTime.Now.ToUnixTime();
                //result = cos.GetFolderList(bucketName, "/", 20, "", 0, FolderPattern.Both);
                //result = cos.CreateFolder(bucketName, "/sdk/");
                //result = cos.UploadFile(bucketName, "/sdk/xx.txt", @"D:\aa.txt");
                //result = cos.UpdateFile(bucketName, "/sdk/xx.txt", "test file");
                //result = cos.GetFileStat(bucketName, "/sdk/xx.txt");
                //result = cos.UpdateFolder(bucketName, "/sdk/", "test folder");
                //result = cos.GetFolderStat(bucketName, "/sdk/");
                //result = cos.DeleteFile(bucketName, "/sdk/xx.txt");
                //result = cos.DeleteFolder(bucketName, "/sdk/");
                result = cos.SliceUploadFile(bucketName, "/红警II共和国之辉(简体中文版).rar", "F:\\[SHD143].txt", 512 * 1024);
                var end = DateTime.Now.ToUnixTime();
                Console.WriteLine(result);
                Console.WriteLine("总用时：" + (end - start) + "毫秒");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
