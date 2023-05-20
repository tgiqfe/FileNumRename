using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace IncrFileNum
{
    internal class FileSummary
    {
        /// <summary>
        /// ファイルへのフルパス
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ファイル名に含まれている数字部分
        /// </summary>
        public FileNameNumber[] Numbers { get; set; }

        /// <summary>
        /// コンソール表示上の行番号
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 出力した文字数
        /// </summary>
        public int OutputLength { get; set; }

        /// <summary>
        /// ファイル名に数字が含まれていたらtrue
        /// </summary>
        public bool Enabled { get; set; }

        public FileSummary(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Numbers = FileNameNumber.Load(System.IO.Path.GetFileNameWithoutExtension(path));
            if (Numbers?.Length > 0)
            {
                this.Enabled = true;
            }
        }

        public void WriteLine(StateView state)
        {
            if (state.Position < Numbers.Length)
            {
                var num = Numbers[state.Position];
                var nextNum = num.Number + state.Increase;
                string nextNumText = nextNum < 0 ?
                    "-" + nextNum.ToString().Substring(1).PadLeft(num.Length - 1, '0') :
                    nextNum.ToString().PadLeft(num.Length, '0');

                Console.Write(string.Format(" {0} [{1}/{2}] ({3}->{4}) | ",
                    Row.ToString().PadLeft(state.RowDigit, ' '),
                    state.Position + 1,
                    Numbers.Length,
                    num.Number.ToString().PadLeft(num.Length, '0'),
                    nextNumText));

                Console.Write(Name.Substring(0, num.Index));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Name.Substring(num.Index, num.Length));
                Console.ResetColor();
                Console.Write(Name.Substring(num.Index + num.Length));

                this.OutputLength = Console.CursorLeft;
                Console.WriteLine();
            }
            else
            {
                Console.Write(string.Format(" {0} [-/{1}] () | {2}",
                    Row.ToString().PadLeft(state.RowDigit, ' '),
                    Numbers.Length,
                    Name));
                this.OutputLength = Console.CursorLeft;
                Console.WriteLine();
            }
        }

        public string GetNewName(int position, int increase)
        {
            if (increase != 0 && position < Numbers.Length)
            {
                var num = Numbers[position];
                var nextNum = num.Number + increase;
                string nextnumText = nextNum < 0 ?
                    "-" + nextNum.ToString().Substring(1).PadLeft(num.Length - 1, '0') :
                    nextNum.ToString().PadLeft(num.Length, '0');

                StringBuilder sb = new();
                sb.Append(Name.Substring(0, num.Index));
                sb.Append(nextnumText);
                sb.Append(Name.Substring(num.Index + num.Length));
                return sb.ToString();
            }
            return null;
        }
    }
}
