using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4_PKG_Linker
{
    class JSON
    {
        public long originalFileSize { get; set; }
        public string packageDigest { get; set; }
        public int numberOfSplitFiles { get; set; }
        public Piece[] pieces { get; set; }
    }

    public class Piece
    {
        public string url { get; set; }
        public long fileOffset { get; set; }
        public long fileSize { get; set; }
        public string hashValue { get; set; }
    }
}
