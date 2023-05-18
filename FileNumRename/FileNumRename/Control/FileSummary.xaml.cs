﻿using FileNumRename.Lib;
using FontAwesome6;
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

        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string ParentPath { get; private set; }
        public NameNumber[] NameNumbers { get; private set; }

        public string NamePartsPre { get; private set; }
        public string NamePartsSuf { get; private set; }
        public string NumberSrc { get; private set; }
        public string NumberDst { get; private set; }

        public EFontAwesomeIcon StatusIcon { get; set; }
        public string StatusText { get; set; }


        public FileSummary(string path, int index)
        {
            Index = index + 1;
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
            this.ParentPath = Path.GetDirectoryName(path);
            this.NameNumbers = NameNumber.Deploy(FileName);
            //this.StatusIcon = EFontAwesomeIcon.Regular_PenToSquare;
            //this.StatusText = "Setting...";

            //UpdateCursor(defCursor, defIncrease);

            InitializeComponent();
            this.DataContext = this;
        }

        private long _destinationNumber = -1;
        private NameNumber _number = null;

        public bool PreCheck(int cursor, long increase)
        {
            _number = NameNumbers[cursor];
            _destinationNumber = _number.Number + increase;
            return _destinationNumber >= 0 && _destinationNumber <= _number.Max;
        }

        public void UpdateIncrease()
        {
            this.NumberDst = _destinationNumber.ToString().PadLeft(_number.Length, '0');
            OnPropertyChanged(nameof(NumberDst));
        }

        public void UpdateCursor()
        {
            this.NamePartsPre = FileName.Substring(0, _number.Position);
            this.NamePartsSuf = FileName.Substring(_number.Position + _number.Length);
            this.NumberSrc = _number.Number.ToString().PadLeft(_number.Length, '0');
            this.NumberDst = _destinationNumber.ToString().PadLeft(_number.Length, '0');

            OnPropertyChanged(nameof(NamePartsPre));
            OnPropertyChanged(nameof(NamePartsSuf));
            OnPropertyChanged(nameof(NumberSrc));
            OnPropertyChanged(nameof(NumberDst));
        }

        /*
        public void UpdateIncrease(int cursor, long increase)
        {
            var number = NameNumbers[cursor];
            long dstNum = number.Number + increase;

            if ((increase == 0) ||
                (increase > 0 && dstNum <= number.Max) ||
                (increase < 0 && dstNum >= 0))
            {
                this.NumberDst = dstNum.ToString().PadLeft(number.Length, '0');
                OnPropertyChanged(nameof(NumberDst));
            }
        }

        public void UpdateCursor(int cursor, long increase)
        {
            var number = NameNumbers[cursor];
            this.NamePartsPre = FileName.Substring(0, number.Position);
            this.NamePartsSuf = FileName.Substring(number.Position + number.Length);
            this.NumberSrc = number.Number.ToString().PadLeft(number.Length, '0');
            this.NumberDst = (number.Number + increase).ToString().PadLeft(number.Length, '0');

            OnPropertyChanged(nameof(NamePartsPre));
            OnPropertyChanged(nameof(NamePartsSuf));
            OnPropertyChanged(nameof(NumberSrc));
            OnPropertyChanged(nameof(NumberDst));
        }
        */

        public void CheckStatus(string[] srcFilePaths)
        {
            string dstFilePath = Path.Combine(ParentPath, NamePartsPre + NumberDst + NamePartsSuf);
            if(FilePath.Equals(dstFilePath, StringComparison.OrdinalIgnoreCase))
            {
                StatusIcon = EFontAwesomeIcon.Regular_NoteSticky;
                StatusText = "Not Change.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));
                return;
            }
            
            if (!srcFilePaths.Contains(dstFilePath) && File.Exists(dstFilePath))
            {
                StatusIcon = EFontAwesomeIcon.Solid_Xmark;
                StatusText = "File already exists.";
                OnPropertyChanged(nameof(StatusIcon));
                OnPropertyChanged(nameof(StatusText));
                return;
            }
            StatusIcon = EFontAwesomeIcon.Regular_PenToSquare;
            StatusText = "Setting...";
            OnPropertyChanged(nameof(StatusIcon));
            OnPropertyChanged(nameof(StatusText));
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
