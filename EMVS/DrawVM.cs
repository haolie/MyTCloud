using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace EMVS
{
    public class DrawVM
    {
        private Bitmap map;
        private Point _zPoint;
        private float _vdde = 10;
        private float maxper = 10;

        public DrawVM(int width,int height) 
        {
            this.map =new Bitmap(width,height);
            _zPoint = new Point(width / 2, height / 2);
            

        }


        public void DrawCoress() 
        {
            using (Graphics g = Graphics.FromImage(map))
            {
                g.FillRectangle(Brushes.White, 0, 0, map.Width, map.Height);
                using (Pen p = new Pen(Brushes.Red,10))
                {
                    g.DrawLine(p, new Point(_zPoint.X, 0), new Point(_zPoint.X, map.Height));
                    g.DrawLine(p, new Point(0, _zPoint.Y), new Point(map.Width, _zPoint.Y));
                }
            }
        
        }

        private Point GetVmPoint(VMDrawPoint p)
        {
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

        public  void FillPoint(List<VMDrawPoint> points)
        {

            using (Graphics g =Graphics.FromImage(map))
            {
                foreach(VMDrawPoint p in points)
                {
                    Point point= GetVmPoint(p);
                    Pen pen=Pens.Yellow;
                    if(p.CurPer>0)pen=Pens.Red;
                    if(p.CurPer<0)pen=Pens.Green;
                    g.DrawEllipse(pen, point.X, point.Y, 2, 2);
                }
             
            }
        }
    }
}
