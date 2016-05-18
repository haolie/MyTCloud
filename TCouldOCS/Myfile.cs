using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tools;

namespace TCouldOCS
{
    public class Myfile
    {
        public Myfile()
        { 
        }

        public string key { get; set; }

        public bool IsInCould { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public string name { get; set; }

        public static string CreateFileKey(string name, long size) 
        {
            name = name.Replace(".map4", "");

            return SxSecrity.MD5Encrypt(string.Format("{0}{1}", name, size.ToString()));
        }

    }
}
