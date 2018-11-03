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



}
