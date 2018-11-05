using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4_PKG_Linker
{
    
    class LINKS
    {
        public Plink[] plinks { get; set; }
    }

    public class Plink
    {
        public string content_id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string icon_link { get; set; }
    }

    
/*
    public class LINKS
    {
        public Plink[] Property1 { get; set; }
    }

    public class Plink
    {
        public string CID { get; set; }
        public string Name { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
    }*/


}
