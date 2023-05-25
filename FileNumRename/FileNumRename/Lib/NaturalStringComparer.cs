using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    internal class NaturalStringComparer :
        System.Collections.IComparer,
        System.Collections.Generic.IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x.ToString(), y.ToString());
        }
    }
}
