using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    public class NameNumber
    {
        public long Number { get; private set; }

        public int Position { get; private set; }

        public int Length { get; private set; }

        public long Max { get; private set; }

        private static Regex pattern = new Regex(@"\d+");

        public static NameNumber[] Deploy(string fileName)
        {
            return pattern.Matches(fileName).Select(x => new NameNumber()
            {
                Number = long.Parse(x.Value),
                Position = x.Index,
                Length = x.Length,
                Max = long.Parse(new string('9', x.Length)),
            }).ToArray();
        }
    }
}
