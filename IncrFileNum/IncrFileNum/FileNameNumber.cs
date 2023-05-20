using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IncrFileNum
{
    /// <summary>
    /// ファイル名に含まれている数字部分
    /// </summary>
    internal class FileNameNumber
    {
        /// <summary>
        /// ファイル名に含まれている数字
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// ファイル名の数字部分の位置。先頭からの文字数
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 数字部分の桁数
        /// </summary>
        public int Length { get; set; }

        private static Regex pat_number = new Regex(@"\-?\d+");

        public static FileNameNumber[] Load(string fileName)
        {
            return pat_number.Matches(fileName).Select(x => new FileNameNumber()
            {
                Number = long.Parse(x.Value),
                Index = x.Index,
                Length = x.Length,
            }).ToArray();
        }

        /*
        public string GetNumber()
        {
            if (this.Number >= (this.Length * 10))
            {
                this.Length = this.Number.ToString().Length;
            }
            return this.Number.ToString().PadLeft(this.Length, '0');
        }
        */
    }
}
