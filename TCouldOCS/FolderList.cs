using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCouldOCS
{
    public class FolderList
    {
        public int code { get; set; }
        public string message { get; set; }

        public FolderList_data data { get; set; }


    }

    public class FolderList_data
    {
        public bool has_more { get; set; }

        public string context { get; set; }

        public string dircount { get; set; }

        public string filecount { get; set; }

        public List<FolderList_data_info> infos { get; set; }
    }

    public class FolderList_data_info
    {
        public string name { get; set; }
        public string biz_attr { get; set; }
        public string ctime { get; set; }
        public string mtime { get; set; }

        public long filesize { get; set; }

        public long filelen { get; set; }

        public string sha { get; set; }

        public string access_url { get; set; }
    }
}
