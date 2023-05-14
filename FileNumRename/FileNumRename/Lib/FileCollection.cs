using FileNumRename.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    internal class FileCollection
    {
        private const int DEF_CURSOR = 0;
        private const long DEF_INCREASE = 1;

        public string[] SourceFilePaths { get; set; }
        public int MaxCursor { get; private set; }
        //public long MaxIncrease { get; private set; }
        public long[] Increases { get; private set; }

        public int Cursor { get; set; }
        public long Increase
        {
            get { return Increases[Cursor]; }
            set { Increases[Cursor] = value; }
        }
        public List<FileSummary> List { get; set; }



        public FileCollection(string[] paths)
        {
            this.List = new(paths.Select((x, y) => new FileSummary(x, y, DEF_CURSOR, DEF_INCREASE)));

            this.SourceFilePaths = paths;
            this.MaxCursor = List.Min(x => x.NameNumbers.Length);
            //this.MaxIncrease = List.Min(x => x.NameNumbers[Cursor].Max);
            this.Increases = Enumerable.Repeat<long>(1, MaxCursor).ToArray();
        }

        public void UpdateIncrease(long increase)
        {
            if (increase > 0)
            {
                Increase++;
                List.ForEach(x =>
                {
                    x.UpdateIncrease(Cursor, Increase);
                    x.CheckStatus(SourceFilePaths);
                });
            }
            else if (increase < 0)
            {
                Increase--;
                List.ForEach(x =>
                {
                    x.UpdateIncrease(Cursor, Increase);
                    x.CheckStatus(SourceFilePaths);
                });
            }
        }

        public void UpdateCursor(int cursor)
        {
            if (cursor > 0 && this.Cursor < MaxCursor)
            {
                Cursor++;
                List.ForEach(x =>
                {
                    x.UpdateCursor(Cursor, Increase);
                    x.CheckStatus(SourceFilePaths);
                });
            }
            else if (cursor < 0 && this.Cursor > 0)
            {
                Cursor--;
                List.ForEach(x =>
                {
                    x.UpdateCursor(Cursor, Increase);
                    x.CheckStatus(SourceFilePaths);
                });
            }
        }
    }
}
