using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    internal class RenameHistoryItem
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public List<string> History { get; set; }

        
    }
}
