using FileNumRename.Lib;
using FontAwesome6;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FileNumRename.Control
{
    /// <summary>
    /// FileSummary.xaml の相互作用ロジック
    /// </summary>
    public partial class FileSummary : UserControl, INotifyPropertyChanged
    {
        public int Index { get; private set; }

        public string ParentPath { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get { return Path.Combine(ParentPath, FileName); } }
        public NameNumber[] NameNumbers { get; private set; }

        public string NamePartsPre { get; private set; }
        public string NamePartsSuf { get; private set; }
        public string NumberSrc { get; private set; }
        public string NumberDst { get; private set; }

        public string DestinationName
        {
            get { return NamePartsPre + NumberDst + NamePartsSuf; }
        }
        public string DestinationPath
        {
            get { return Path.Combine(ParentPath, NamePartsPre + NumberDst + NamePartsSuf) ; }
        }
        
        public EFontAwesomeIcon StatusIcon { get; set; }
        public string StatusText { get; set; }


        public FileSummary(string path, int index)
        {
            Index = index + 1;
            this.FileName = Path.GetFileName(path);
            this.ParentPath = Path.GetDirectoryName(path);
            this.NameNumbers = NameNumber.Deploy(FileName);

            InitializeComponent();
            this.DataContext = this;
        }

        private NameNumber _number = null;
        private long _destNum = -1;

        public bool PreCheck(int cursor, long increase)
        {
            _number = NameNumbers[cursor];
            _destNum = _number.Number + increase;
            return _destNum >= 0 && _destNum <= _number.Max;
        }

        public void UpdateIncrease()
        {
            this.NumberDst = _destNum.ToString().PadLeft(_number.Length, '0');
            OnPropertyChanged(nameof(NumberDst));
        }

        public void UpdateCursor()
        {
            this.NamePartsPre = FileName.Substring(0, _number.Position);
            this.NamePartsSuf = FileName.Substring(_number.Position + _number.Length);
            this.NumberSrc = _number.Number.ToString().PadLeft(_number.Length, '0');
            this.NumberDst = _destNum.ToString().PadLeft(_number.Length, '0');

            OnPropertyChanged(nameof(NamePartsPre));
            OnPropertyChanged(nameof(NamePartsSuf));
            OnPropertyChanged(nameof(NumberSrc));
            OnPropertyChanged(nameof(NumberDst));
        }

        public void CheckStatus(string[] srcFilePaths)
        {
            //  ファイル名変更無しの場合
            if (FilePath.Equals(DestinationPath, StringComparison.OrdinalIgnoreCase))
            {
                StatusIcon = EFontAwesomeIcon.Regular_NoteSticky;
                StatusText = "Not Change.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));
                return;
            }

            //  変更後のファイル名がすでに存在する場合
            if (!srcFilePaths.Contains(DestinationPath) && File.Exists(DestinationPath))
            {
                StatusIcon = EFontAwesomeIcon.Solid_Xmark;
                StatusText = "File already exists.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));
                return;
            }

            //  上記の問題に該当せず。編集中。
            StatusIcon = EFontAwesomeIcon.Regular_PenToSquare;
            StatusText = "Setting...";
            OnPropertyChanged(nameof(StatusIcon));
            OnPropertyChanged(nameof(StatusText));
        }

        public void ChangeFileName()
        {
            try
            {
                FileSystem.RenameFile(this.FilePath, DestinationName);

                //  名前変更に成功
                StatusIcon = EFontAwesomeIcon.Solid_SquareCheck;
                StatusText = "Rename success.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));

                this.FileName = DestinationName;
                this.NameNumbers = NameNumber.Deploy(FileName);
            }
            catch
            {
                //  名前変更に失敗
                StatusIcon = EFontAwesomeIcon.Solid_Xmark;
                StatusText = "Failed to rename.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));
            }
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
