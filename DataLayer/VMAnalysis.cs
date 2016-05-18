using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class VMAnalysis
    {
        public static string GetToken(string vs) 
        {
            int index = vs.IndexOf("token");
            if (index < 0) return "";
            vs = vs.Substring(index);
            vs = vs.Substring(vs.IndexOf(":"));
            vs = vs.Substring(0, vs.IndexOf(","));
            vs = vs.Replace(":", "");
            return vs.Replace("\"", "");
        }

        public static List<VMInfo> GetVMInfos(string vs)
        {
            List<VMInfo> infos = new List<VMInfo>();
            vs = vs.Substring(vs.IndexOf("extresProcess"));
            vs = vs.Substring(0, vs.IndexOf("oriIndexID"));
            vs = vs.Substring(vs.IndexOf("result"));
            vs = vs.Substring(vs.IndexOf(":") + 1);
            vs = vs.Replace("[[", "");
            vs = vs.Replace("]],\"", "");
            vs = vs.Replace("[", "");
            string[] moys = vs.Split(']');
            foreach (string m in moys)
            {
                string tm = m;
                tm = tm.Replace("\"","");
                if (m.StartsWith(",")) tm = tm.Substring(1);
                string[] values = tm.Split(',');
                VMInfo mo = new VMInfo();
                mo.Code = values[0];
                mo.Name = values[1];
            
                if (tm.IndexOf("--")<0) 
                {
                    mo.Per = float.Parse(values[2]);
                    mo.Price = float.Parse(values[3]);
                    mo.Dde = float.Parse(values[4]);
                    mo.Useable = true;
                  
                }
                else
                {
                    mo.Per = 0;
                    mo.Price = 0;
                    mo.Dde = 0;
                    mo.Useable = false;
                }
           
                infos.Add(mo);
            }

            return infos;
        }

        public static List<VMInfo> GetVMInfosB(string vs)
        {
            List<VMInfo> infos = new List<VMInfo>();
            vs = vs.Substring(vs.IndexOf("extresProcess"));
            vs = vs.Substring(0, vs.IndexOf("oriIndexID"));
            vs = vs.Substring(vs.IndexOf("result"));
            vs = vs.Substring(vs.IndexOf(":") + 1);
            vs = vs.Replace("[[", "");
            vs = vs.Replace("]],\"", "");
            vs = vs.Replace("[", "");
            string[] moys = vs.Split(']');
            foreach (string m in moys)
            {
                string tm = m;
                tm = tm.Replace("\"", "");
                if (m.StartsWith(",")) tm = tm.Substring(1);
                string[] values = tm.Split(',');
                VMInfo mo = new VMInfo();
                mo.Code = values[0];
                mo.Name = values[1];

                if (tm.IndexOf("--") < 0)
                {
                    mo.Per = float.Parse(values[2]);
                    mo.Price = float.Parse(values[3]);
                    mo.Useable = true;

                }
                else
                {
                    mo.Per = 0;
                    mo.Price = 0;
                    mo.Dde = 0;
                    mo.Useable = false;
                }

                infos.Add(mo);
            }

            return infos;
        }
    }
}
