using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DataLayer;

namespace EMVS
{
    public partial class Form1 : Form
    {
        WebBrowser browser;
        List<VMDrawPoint> allPints = new List<VMDrawPoint>();
        string token = "";
        int pageindex = 1;
        int count = 70;
        DateTime time = DateTime.Now;
        VmDrawPanel panel;

        private float ddeaddup = 0;
        private float ddeadddown = 0;
        private float ddeDledown = 0;
        private float ddeDleup = 0;

        private event EventHandler<RequestArgs> TokenArrived;
        private event EventHandler<RequestArgs> InfoArrived;
        private event EventHandler<RequestArgs> CurPerArrived;

        public Form1()
        {
            InitializeComponent();
            panel = new VmDrawPanel();
            this.Controls.Add(panel);
            panel.Height = this.Height - 30;
            panel.Dock = DockStyle.Bottom;
            // browser = new WebBrowser();
            //browser.Dock = DockStyle.Fill;
            //this.Controls.Add(browser);
            //browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
            //browser.Navigate("http://www.iwencai.com/stockpick/search?typed=1&preParams=&ts=1&f=1&qs=result_rewrite&selfsectsn=&querytype=&searchfilter=&tid=stockpick&ss=1&w=601766 dee");
           // browser.Navigate(" http://www.iwencai.com/stockpick/search?typed=1&preParams=&ts=1&f=1&qs=result_rewrite&selfsectsn=&querytype=&searchfilter=&tid=stockpick&w=%E5%87%80%E9%87%8F");

            TokenArrived += new EventHandler<RequestArgs>(Form1_TokenArrived);
            InfoArrived += new EventHandler<RequestArgs>(Form1_InfoArrived);
            CurPerArrived += new EventHandler<RequestArgs>(Form1_CurPerArrived);
       
        

       
        }

        void Form1_CurPerArrived(object sender, RequestArgs e)
        {
            List<VMInfo> infos = VMAnalysis.GetVMInfosB(e.Ret);
            foreach (VMInfo v in infos)
            {
                VMDrawPoint lv = allPints.Find(f => f.Info.Code == v.Code);
                if (lv == null) continue;

                if (!v.Useable) 
                {
                    allPints.Remove(lv);
                    continue;
                };
            
               
                lv.Info.Price = v.Price;
                lv.Info.Per = v.Per;
               
                
            }
           
            if (infos.Count < count)
            {
                pageindex = 1;
                panel.FillPoint(allPints);
                return;

            }
            pageindex++;
            GetInfo(CurPerArrived);
        }

        void Form1_InfoArrived(object sender, RequestArgs e)
        {
            List<VMInfo> infos = VMAnalysis.GetVMInfos(e.Ret);
            foreach (VMInfo vi in infos)
            {
                if (!vi.Useable) continue;

                VMDrawPoint vmd = new VMDrawPoint();
                vmd.Info = vi;
                vmd.CurPer = vi.Per;
                allPints.Add(vmd);
            }
            if (infos.Count < count) 
            {
                pageindex = 1;
                //GetCurPrice(DateTime.Now.AddDays(-1));
                panel.FillPoint(allPints);

                foreach (VMDrawPoint p in allPints)
                {
                    if (p.Info.Dde > 0)
                    {
                        if (p.Info.Per > 0) ddeaddup++;
                        else if (p.Info.Per < 0)
                            ddeadddown++;

                    }
                    else
                    {
                        if (p.Info.Per > 0) ddeDleup++;
                        else if (p.Info.Per < 0)
                            ddeDledown++;
                    }
                }

                SetLable();

                return;
            }

            
            
            pageindex++;
            GetInfo(InfoArrived);
        }

        private void SetLable()
        {
            if (InvokeRequired)
            {
                Tools.NullParameterInvoker invoker = new Tools.NullParameterInvoker(SetLable);
                this.Invoke(invoker);
                return;

            }

            topLLabe.Text = string.Format("{0}  {1}", ddeDleup.ToString(), (ddeDleup * 100 / allPints.Count));
            topRLabe.Text = string.Format("{0}  {1}", ddeaddup.ToString(), (ddeaddup * 100 / allPints.Count));
            bottomLLabe.Text = string.Format("{0}  {1}", ddeDledown.ToString(), (ddeDledown * 100 / allPints.Count));
            bottomRLabe.Text = string.Format("{0}  {1}", ddeadddown.ToString(), (ddeadddown * 100 / allPints.Count));
        }

        void Form1_TokenArrived(object sender, RequestArgs e)
        {
            token = VMAnalysis.GetToken(e.Ret);
            if (string.IsNullOrEmpty(token)) throw new Exception();
            if (allPints.Count == 0)
            {
                GetInfo(InfoArrived);
            }
            else 
            {
                GetInfo(CurPerArrived);
            }
            
        }

        private void GetCurPrice(DateTime time) 
        {
            string url = string.Format("http://www.iwencai.com/stockpick/search?typed=1&preParams=&ts=1&f=1&qs=result_rewrite&selfsectsn=&querytype=&searchfilter=&tid=stockpick&w={0}", time.ToString("yyyy.MM.dd"));
            RequestArgs arg = new RequestArgs(url, RequestType.Get, TokenArrived, time, 10000);
            HttpTool.Request(arg, true);
        }

        private void GetToken(DateTime time)
        {
            string url = string.Format("http://www.iwencai.com/stockpick/search?typed=1&preParams=&ts=1&f=1&qs=result_rewrite&selfsectsn=&querytype=&searchfilter=&tid=stockpick&w=%E5%87%80%E9%87%8F+{0}", time.ToString("yyyy.MM.dd"));
            RequestArgs arg = new RequestArgs(url, RequestType.Get, TokenArrived, time, 10000);
            HttpTool.Request(arg, true);
        }

        private void GetInfo( EventHandler<RequestArgs> e) 
        {
            string url = string.Format("http://www.iwencai.com/stockpick/cache?token={0}&p={1}&perpage={2}&showType=[%22%22,%22%22,%22onTable%22,%22onTable%22,%22onTable%22,%22onTable%22]", token, pageindex, count);
            RequestArgs arg = new RequestArgs(url, RequestType.Get, e, null, 10000);
            HttpTool.Request(arg, true);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            GetToken(DateTime.Now.AddDays(-1));
        }

        

    }

}
