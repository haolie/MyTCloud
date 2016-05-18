using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Tools;
using System.Threading;

namespace TCouldOCS
{
    public class Transferttool
    {

        private static string PathFile = "path.text";

        private static String _LocalPath = "";
        private  List<PairObject<string, string>> UploadWaiteList=new List<PairObject<string,string>>();
        private  Thread UploadTread = null;
        private  bool UploadWorking = false;
        private  Myfile myfile=null;
        private  bool hasErr = false;

        private  bool FileCutting = false;

        public  event EventHandler FileTranCompleted;

        public static string LocalPath
        {
            get
            {
                if (_LocalPath.Length == 0)
                {
                    _LocalPath = System.Environment.CurrentDirectory;
                }

                return _LocalPath;
            }
        }

        public Transferttool(Myfile file)
        {
            myfile = file;
        }

        public List<Myfile> GetFilePaths()
        {
            List<Myfile> files = new List<Myfile>();
            List<string> searchPaths = new List<string>();


            return null;
        }

        private List<string> GetSearchPaths()
        {
            List<string> paths = new List<string>();

            using (FileStream fs = File.OpenRead(""))
            {
            }
            paths.Add("");
            return paths;
        }

        private string BasePath = "";
        //文件分割 大于8M的文件分割成8*n+x 小于8分为 3+2
        public void StartFileCut()
        {
            int maxsize = 1024 * 6 * 1024;
            long writesize = 0;

            if (myfile.Size < maxsize)//dai xiu
                maxsize = (Convert.ToInt32(myfile.Size / 3)) * 2;

            byte[] buffer = new byte[1024];
            using (FileStream readfs = File.OpenRead(myfile.Path))
            {
                if (!Directory.Exists(LocalPath + "\\temp")) Directory.CreateDirectory(LocalPath + "\\temp");

                FileCutting = TCouldManeger.CreateFolder(Myfile.CreateFileKey(myfile.name, myfile.Size));
                if (!FileCutting) 
                {
                    Console.WriteLine(string.Format("create folder failed :{0}",myfile.Path));
                    if (FileTranCompleted != null) FileTranCompleted(this, EventArgs.Empty);
                    return; 
                }
                string baseDirectory = LocalPath + "\\temp\\" + myfile.name;
                BasePath = baseDirectory;
                if (!Directory.Exists(baseDirectory)) Directory.CreateDirectory(baseDirectory);
                int fileIndex = 0;
                string tempPath = "";
           
                hasErr = false;
               
                do
                {
                    writesize = 0;
                    tempPath =string.Format("{0}\\_{1}",baseDirectory,fileIndex.ToString());
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                    using (FileStream wf = File.Create(tempPath, buffer.Length))
                    {
                        do
                        {
                            int len = readfs.Read(buffer, 0, buffer.Length);
                            if (len > 0)
                                wf.Write(buffer, 0, buffer.Length);
                            writesize += len;

                            if (len < buffer.Length)
                            {
                                FileCutting = false;
                                break;
                            }
                        } while (writesize < maxsize);

                        UploadFile(Myfile.CreateFileKey(myfile.name, myfile.Size) + "\\" + fileIndex.ToString(), tempPath);

                    }

                    fileIndex++;

                } while (FileCutting);


            }
        }

        public void CreateFullFile(string sourceDriectory, string dist)
        {
            if (!Directory.Exists(sourceDriectory) || !Directory.Exists(dist)) return;

            string[] infos= Directory.GetFiles(sourceDriectory);
            if (infos.Length == 0) return;

            string filename=sourceDriectory.Substring( sourceDriectory.LastIndexOf("\\")+1);
            using (FileStream wf = File.Create(dist + "\\" + filename))
            {
                int buffersize = 1024 * 6;
                byte[] buffer = new byte[buffersize];
                foreach (string temppath in infos)
                {
                    using (FileStream rs = File.OpenRead(temppath))
                    {
                        do
                        {
                            int len = rs.Read(buffer, 0, buffersize);
                            if (len > 0)
                                wf.Write(buffer, 0, len);

                            if (len < buffersize) break;
                        } while (true);

                        rs.Close();
                    }
                }
                wf.Close();
            }

           
        }

        public void UploadFile(string foldername, string file)
        {
            lock(UploadWaiteList){
                     UploadWaiteList.Add(new PairObject<string, string>(file, foldername));
            }

            if (!UploadWorking)
            {
                UploadTread = new Thread(PorcessUpload);
                UploadWorking = true;
                UploadTread.Start();
            }
        }

        private void PorcessUpload(object state)
        {
            while (true)
            {
                PairObject<string, string> file = null;

                lock (UploadWaiteList)
                {
                    if (UploadWaiteList.Count == 0)
                        break;

                    file = UploadWaiteList[0];
                    UploadWaiteList.Remove(file);
                }

              bool result= TCouldManeger.UpdataFile(file.SubValue, file.MainValue);
                
              if (result) File.Delete(file.MainValue);
                else
                  Console.WriteLine(string.Format("upload file failed :{0}", myfile.Path));
                 hasErr|=!result;

            }

            UploadWorking = false;
            DeleteFile();
            UploadTread.Abort();
        

        }

        private void DeleteFile()
        {
            if (FileCutting ) return;

            if (!hasErr)
            File.Delete(myfile.Path);
            Directory.Delete(BasePath);
            if (FileTranCompleted != null) FileTranCompleted(this, EventArgs.Empty);
        }

        //public static void  StartFileCutSingle(Myfile file)
        //{
        //    int maxsize = 1024 * 6 * 1024;
        //    long writesize = 0;

        //    if (file.Size < maxsize)//dai xiu
        //        maxsize = (Convert.ToInt32(file.Size / 3)) * 2;
        //    List<string> sfiles = new List<string>();
        //    byte[] buffer = new byte[1024];
        //    string baseDirectory ="";
        //    using (FileStream readfs = File.OpenRead(file.Path))         
        //    {
        //        if (!Directory.Exists(LocalPath + "\\temp")) Directory.CreateDirectory(LocalPath + "\\temp");

        //        FileCutting = TCouldManeger.CreateFolder(Myfile.CreateFileKey(file.name, file.Size));
        //        if (!FileCutting) return;
        //         baseDirectory = LocalPath + "\\temp\\" + file.name;
        //        BasePath = baseDirectory;
        //        if (!Directory.Exists(baseDirectory)) Directory.CreateDirectory(baseDirectory);
        //        int fileIndex = 0;
        //        string tempPath = "";

        //        hasErr = false;

        //        do
        //        {
        //            writesize = 0;
        //            tempPath = string.Format("{0}\\_{1}", baseDirectory, fileIndex.ToString());
        //            if (File.Exists(tempPath)) File.Delete(tempPath);
        //            sfiles.Add(tempPath);
        //            using (FileStream wf = File.Create(tempPath, buffer.Length))
        //            {
        //                do
        //                {
        //                    int len = readfs.Read(buffer, 0, buffer.Length);
        //                    if (len > 0)
        //                        wf.Write(buffer, 0, buffer.Length);
        //                    writesize += len;

        //                    if (len < buffer.Length)
        //                    {
        //                        FileCutting = false;
        //                        break;
        //                    }
        //                } while (writesize < maxsize);


        //            }

        //            fileIndex++;

        //        } while (FileCutting);
        //                    }

        //    foreach (string f in sfiles)
        //    {
        //        bool result = TCouldManeger.UpdataFile(Myfile.CreateFileKey(file.name, file.Size), f);
        //        if (result) File.Delete(f);
        //    }

        //    Directory.Delete(baseDirectory);
        //    File.Delete(file.Path);
           
        //}

    }
}
