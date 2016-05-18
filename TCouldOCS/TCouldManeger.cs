using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QCloud.CosApi.Api;
using Newtonsoft.Json;

namespace TCouldOCS
{
    public class TCouldManeger
    {
        private static CosCloud _CloudAPI=null;
        private static int Appid = 10028297;
        private static string Secretid = "AKIDmmGfV59KBtcPB1kQCh2ElU6ytlUQDAUC";
        private static string SecretKey = "g2fvgK5hpAIk2KEYPUfmBelxwUhqs2iY";
        public  static string Bucket = "sparkmoon";
        public static string BucketFolder = "winnerHanmer";

        

        public static CosCloud CloudAPI
        {
            get
            {
                if (_CloudAPI == null)
                    _CloudAPI = new CosCloud(Appid, Secretid, SecretKey);

                return _CloudAPI;
            }
        }

        public static bool CreateFolder(string name)
        {
            string result=CloudAPI.CreateFolder(TCouldManeger.Bucket, TCouldManeger.BucketFolder + "\\" + name);
            var temp = JsonConvert.DeserializeObject<FolderList>(result);
            bool s = result.IndexOf("\"code\":0,\"message\":\"SUCCESS\",") >= 0;
            return s;
        }

        public static bool UpdataFile(string cloudPath, string path)
        {
            string result = CloudAPI.SliceUploadFileFirstStep(Bucket, BucketFolder + "\\" + cloudPath, path,512*1024);
            return result.IndexOf("\"code\":0,\"message\":\"SUCCESS\",") >= 0;
        }
     
    }
}
