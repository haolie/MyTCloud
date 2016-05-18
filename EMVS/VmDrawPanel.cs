using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EMVS
{
    public  class VmDrawPanel:Panel
    {
        private Point _zPoint;
        private float _vdde = 1.5f;
        private float maxper = 10;
        List<VMDrawPoint> points;
       
     

        public VmDrawPanel() 
        {
            SizeChanged += new EventHandler(_panel_SizeChanged);
            _zPoint = new Point(Width / 2, Height / 2);
            points = new List<VMDrawPoint>();

        }

        public void FillPoint(List<VMDrawPoint> points)
        {
            if (this.InvokeRequired)
            {
                Tools.SingleParameterInvoker<List<VMDrawPoint>> invoker = new Tools.SingleParameterInvoker<List<VMDrawPoint>>(FillPoint);
                this.Invoke(invoker, points);
            }

       



            this.points.AddRange(points);
            Invalidate();
        }

        void _panel_SizeChanged(object sender, EventArgs e)
        {
         
    
        }

        public void DrawCoress(Graphics g) 
        {
            _zPoint = new Point(Width / 2, Height / 2);
         
                g.FillRectangle(Brushes.White, 0, 0, Width,Height);
                using (Pen p = new Pen(Brushes.Red,2))
                {
                    g.DrawLine(p, new Point(_zPoint.X, 0), new Point(_zPoint.X, Height));
                    g.DrawLine(p, new Point(0, _zPoint.Y), new Point(Width, _zPoint.Y));
                }
            
        
        }

        private Point GetVmPoint(VMDrawPoint p)
        {
            Console.WriteLine(p.Info.Dde);
            float tx = p.Info.Dde / _vdde;
            if (tx > 1) tx = 1;
            if (tx < -1) tx = -1;

            tx = (_zPoint.X * tx) + _zPoint.X;

            float ty = p.Info.Per /maxper;
            if (ty > 1) ty = 1;
            if (ty < -1) ty = -1;

            ty = _zPoint.Y - (_zPoint.Y * ty);
            return new Point(Convert.ToInt32(tx), Convert.ToInt32(ty));
        }

        public void DrawPoint(Graphics g)
        {
         
                foreach(VMDrawPoint p in points)
                {
                    Point point= GetVmPoint(p);
                    Pen pen=Pens.Yellow;
                    if(p.Info.Per>0)pen=Pens.Red;
                    if (p.Info.Per < 0) pen = Pens.Green;
                    g.DrawEllipse(pen, point.X, point.Y, 2, 2);
                }
             
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawCoress(e.Graphics);
            DrawPoint(e.Graphics);
        }
    }
}
