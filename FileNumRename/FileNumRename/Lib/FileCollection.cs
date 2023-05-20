using FileNumRename.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    internal class FileCollection : INotifyPropertyChanged
    {
        private const int DEF_CURSOR = 0;
        private const long DEF_INCREASE = 0;

        public string[] SourceFilePaths { get; set; }
        public int CursorLength { get; private set; }
        public long[] Increases { get; private set; }

        private int _cursor = DEF_CURSOR;
        public int Cursor
        {
            get { return _cursor; }
            set { _cursor = value; OnPropertyChanged(nameof(Cursor)); }
        }
        public long Increase
        {
            get { return Increases[Cursor]; }
            set { Increases[Cursor] = value; OnPropertyChanged(nameof(Increase)); }
        }
        public List<FileSummary> List { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="paths">
        /// 引数の1つ目がフォルダー ⇒ 引数1のフォルダー内の全ファイルを対象にする。引数2以降は無視
        /// 引数の1つ目がファイル ⇒ 引数の中からファイルのみを対象にする。フォルダーがあっても無視
        /// </param>
        public FileCollection(string[] paths)
        {
            if (paths.Length > 0)
            {
                if (Directory.Exists(paths[0]))
                {
                    Init(Directory.GetFiles(paths[0]));
                }
                else
                {
                    Init(paths.Where(x => File.Exists(x)).ToArray());
                }
            }
        }

        private void Init(string[] paths)
        {
            this.List = new(paths.Select((x, y) => new FileSummary(x, y)));
            this.SourceFilePaths = paths;
            this.CursorLength = List.Where(x => x.Enabled).Min(x => x.NameNumbers.Length);
            this.Increases = Enumerable.Repeat<long>(DEF_INCREASE, CursorLength).ToArray();

            List.ForEach(x =>
            {
                x.PreCheck(DEF_CURSOR, DEF_INCREASE);
                x.UpdateCursor();
                x.CheckStatus(SourceFilePaths);
            });
        }

        /// <summary>
        /// 増減値変更 (↑↓キー)
        /// </summary>
        /// <param name="increase"></param>
        public void UpdateIncrease(long increase)
        {
            if (List.All(x => x.PreCheck(Cursor, (Increase + increase))))
            {
                this.Increase += increase;
                List.ForEach(x =>
                {
                    x.UpdateIncrease();
                    x.CheckStatus(SourceFilePaths);
                });
            }
        }

        /// <summary>
        /// カーソル移動 (←→キー)
        /// </summary>
        /// <param name="cursor"></param>
        public void UpdateCursor(int cursor)
        {
            if ((Cursor == 0 && cursor < 0) || (Cursor == (CursorLength - 1) && cursor > 0))
            {
                return;
            }

            Cursor += cursor;
            if (List.All(x => x.PreCheck(Cursor, Increase)))
            {
                List.ForEach(x =>
                {
                    x.UpdateCursor();
                    x.CheckStatus(SourceFilePaths);
                });
            }
        }

        public void ToMaxIncrease()
        {
            var maxDiffNum = List.
                Where(x => x.Enabled).
                Min(x => x.NameNumbers[Cursor].Max - x.NameNumbers[Cursor].Number) + (Increase * -1);
            UpdateIncrease(maxDiffNum);
        }

        public void ToMinIncrease()
        {
            var minDiffNum = (List.
                Where(x => x.Enabled).
                Min(x => x.NameNumbers[Cursor].Number) + Increase) * -1;
            UpdateIncrease(minDiffNum);
        }

        public void ChangeFileName()
        {
            if(Increase == 0) { return; }

            if (Increase > 0)
            {
                //  プラス方向への変更
                List.Where(x => x.Enabled).
                    OrderByDescending(x => x.NameNumbers[Cursor].Number).
                    ToList().
                    ForEach(x => x.ChangeFileName());
            }
            else if (Increase < 0)
            {
                //  マイナス方向への変更
                List.Where(x => x.Enabled).
                    OrderBy(x => x.NameNumbers[Cursor].Number).
                    ToList().
                    ForEach(x => x.ChangeFileName());
            }

            Increase = DEF_INCREASE;
            SourceFilePaths = List.Select(x => x.DestinationPath).ToArray();
            List.ForEach(x =>
            {
                x.PreCheck(Cursor, DEF_INCREASE);
                x.UpdateCursor();
                Item.RenameHistory.AddHistory(x.Hash, x.FileName);
            });

            Item.RenameHistory.Save();
        }

        #region Inotify change

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
