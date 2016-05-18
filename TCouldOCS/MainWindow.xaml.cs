using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCouldOCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int index = 0;
        List<string> allFiles = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            //string ts= Myfile.CreateFileKey("asdfasdasdasdf", 33333);
            //Myfile file = new Myfile();
            //FileInfo info = new FileInfo("C:\\Users\\youhao\\Downloads\\npp.6.9.Installer_5200000106997031026.exe");
            //file.name = info.Name;
            //file.Path = info.FullName;
            //file.Size = info.Length;
            //Transferttool.StartFileCut(file);
            //Transferttool.CreateFullFile(@"F:\TCouldOCS\TCouldOCS\bin\Debug\temp\不亦乐乎.mp4", @"F:\TCouldOCS\TCouldOCS\bin\Debug");
            //string str=   TCouldManeger.CloudAPI.CreateFolder(TCouldManeger.Bucket, TCouldManeger.BucketFolder+"\\bylf");
            // StartTempTrans();
            //string vs=  TCouldManeger.CloudAPI.UploadFile(TCouldManeger.Bucket, "winnerHanmer\\不亦乐乎.mp4", @"F:\TCouldOCS\TCouldOCS\bin\Debug\temp\不亦乐乎.mp4\0");
        }

        private void StartTempTrans()
        {
            //List<string> directs = new List<string>();
            //using (StreamReader fs = File.OpenText(""))
            //{
            //    while (true)
            //    {
            //        string path = fs.ReadLine();
            //        if (path == null) break;
            //        if (path.Length == 0) continue;
            //        directs.Add(path);
            //    }
            //}

            GetFiles(allFiles, "D:\\2013");
            //Transferttool.FileTranCompleted+=Transferttool_FileTranCompleted;
            TrasnQue();
            //foreach (string f in allFiles)
            //{
            //    Myfile file = new Myfile();
            //    FileInfo info = new FileInfo(f);
            //    file.name = info.Name;
            //    file.Path = info.FullName;
            //    file.Size = info.Length;
            //    Transferttool.StartFileCut(file);
            //}

        }

        void tool_FileTranCompleted(object sender, EventArgs e)
        {
            Transferttool tool = sender as Transferttool;
            tool.FileTranCompleted -= tool_FileTranCompleted;
            TrasnQue();
        }

        private void GetFiles(List<string> files, string path)
        {
            if (!Directory.Exists(path)) return;
            string[] strs = Directory.GetFiles(path);
            if (strs.Length > 0)
                files.AddRange(strs);
            strs = Directory.GetDirectories(path);
            foreach (string s in strs) GetFiles(files, s);

        }

        private void TrasnQue()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                Tools.NullParameterInvoker invoker = new Tools.NullParameterInvoker(TrasnQue);
                this.Dispatcher.BeginInvoke(invoker);
                return;
            }



            if (allFiles.Count == 0 || allFiles.Count <= index) return;
            string f = allFiles[index];
            btn.Content = string.Format("{0}/{1}", index.ToString(), allFiles.Count.ToString());
            index++;
            Myfile file = new Myfile();
            FileInfo info = new FileInfo(f);
            file.name = info.Name;
            file.Path = info.FullName;
            file.Size = info.Length;
            Transferttool tool = new Transferttool(file);
            tool.FileTranCompleted += tool_FileTranCompleted;
            tool.StartFileCut();
        }

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartTempTrans();
        }

    }
}
