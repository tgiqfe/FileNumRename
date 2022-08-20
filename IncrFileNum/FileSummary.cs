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

        //  [課題]position指定する際に、格納しているNumber配列より大きい数が指定した場合の処理
        //        同フォルダー内に、数字出現箇所数が違うファイルが混在した場合の対策が必要
        public void WriteLine(StateView state, int row)
        {
            int position = state.Position >= Numbers.Length ? Numbers.Length - 1 : state.Position;

            var num = Numbers[position];
            var nextNum = num.Number + state.Increase;
            string nextNumText = nextNum < 0 ?
                "-" + nextNum.ToString().Substring(1).PadLeft(num.Length - 1, '0') :
                nextNum.ToString().PadLeft(num.Length, '0');

            Console.Write(string.Format(" {0} [{1}/{2}] ({3}->{4}) | ",
                row.ToString().PadLeft(state.RowDigit, ' '),
                position + 1,
                Numbers.Length,
                num.Number.ToString().PadLeft(num.Length, '0'),
                nextNumText));

            Console.Write(Name.Substring(0, num.Index));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Name.Substring(num.Index, num.Length));
            Console.ResetColor();
            Console.WriteLine(Name.Substring(num.Index + num.Length));
        }

        //  [課題]position指定する際に、格納しているNumber配列より大きい数が指定した場合の処理
        //        同フォルダー内に、数字出現箇所数が違うファイルが混在した場合の対策が必要
        public string GetNewName(int position, int increase)
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
    }
}
