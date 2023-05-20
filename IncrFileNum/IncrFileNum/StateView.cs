using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncrFileNum
{
    internal class StateView
    {
        public List<FileSummary> SummaryList { get; set; }
        public int Position { get; set; }
        public int Increase { get; set; }

        public int StartCursorTop { get; set; }

        private int _rowDigit = -1;
        public int RowDigit
        {
            get
            {
                if (_rowDigit < 0)
                {
                    _rowDigit = SummaryList.Count.ToString().Length;
                }
                return _rowDigit;
            }
        }

        private int _maxPosition = -1;
        public int MaxPosition
        {
            get
            {
                if (_maxPosition < 0)
                {
                    _maxPosition = SummaryList.Max(x => x.Numbers.Length) - 1;
                }
                return _maxPosition;
            }
        }

        public void WriteLine()
        {
            Console.WriteLine(new string('=', Console.WindowWidth - 1));
            Console.WriteLine();
            Console.WriteLine($"  [Position: {Position + 1}] [Increase: {Increase}]");
            Console.WriteLine("    ←: Position to left.  ⇒: Position to right.");
            Console.WriteLine("    ↑: Increase.          ↓: Decrease.");
            Console.WriteLine("    Esc: Process end.      Enter: Rename start.");
        }
    }
}
