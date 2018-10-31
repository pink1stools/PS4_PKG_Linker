using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4_PKG_Linker
{
    class Meta
    {

        public int revision { get; set; }
        public int formatVersion { get; set; }
        public string npTitleId { get; set; }
        public string console { get; set; }
        public Name[] names { get; set; }
        public Icon[] icons { get; set; }
        public int parentalLevel { get; set; }
        public string pronunciation { get; set; }
        public string contentId { get; set; }
        public string backgroundImage { get; set; }
        public string bgm { get; set; }
        public string category { get; set; }
        public string psVr { get; set; }
        public string neoEnable { get; set; }
        public bool error { get; set; }
    }

    public class Name
    {
        public string name { get; set; }
        public string lang { get; set; }
    }

    public class Icon
    {
        public string icon { get; set; }
        public string type { get; set; }
    }


}
