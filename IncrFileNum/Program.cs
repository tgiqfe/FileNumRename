using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace IncrFileNum
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (true)
            if (args.Length > 0 && Directory.Exists(args[0]))
            {
                List<FileSummary> list = new();
                Directory.GetFiles(args[0]).ToList().ForEach(x =>
                {
                    var summary = new FileSummary(x);
                    if (summary.Enabled)
                    {
                        list.Add(summary);
                    }
                });

                var cursorPos = Console.GetCursorPosition();
                StateView state = new()
                {
                    List = list,
                    //StartCursorLeft = Console.CursorLeft,
                    StartCursorTop = Console.CursorTop,
                };

                bool during = true;
                while (during)
                {
                    Console.WriteLine(new string('=', Console.WindowWidth - 1));

                    int row = 0;
                    foreach (var summary in list)
                    {
                        summary.WriteLine(state, row++);
                    }
                    state.WriteLine();

                    during = Read(state);
                }
            }
        }

        /// <summary>
        /// コンソール入力したキーを読み取り
        /// </summary>
        /// <param name="state"></param>
        /// <returns>プロセスを続行するかどうか</returns>
        private static bool Read(StateView state)
        {
            bool reading = true;
            while (reading)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return false;
                    case ConsoleKey.Enter:
                        Increase(state);
                        return false;
                    case ConsoleKey.RightArrow:
                        if (state.Position < state.MaxPosition)
                        {
                            state.Position++;
                            reading = false;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (state.Position > 0)
                        {
                            state.Position--;
                            reading = false;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        state.Increase++;
                        reading = false;
                        break;
                    case ConsoleKey.DownArrow:
                        state.Increase--;
                        reading = false;
                        break;
                }
            }

            //  コンソール上にすでに出力済みの内容を消す
            int lastCursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, state.StartCursorTop);
            string spaceRow = new string(' ', Console.WindowWidth -1);
            for(int i = state.StartCursorTop; i < lastCursorTop; i++)
            {
                Console.WriteLine(spaceRow);
            }
            Console.SetCursorPosition(0, state.StartCursorTop);

            return true;
        }

        private static void Increase(StateView state)
        {
            try
            {
                if (state.Increase == 0) { return; }
                if (state.Increase > 0) { state.List.Reverse(); }

                for (int i = 0; i < state.List.Count; i++)
                {
                    string newName = state.List[i].GetNewName(state.Position, state.Increase);
                    FileSystem.RenameFile(state.List[i].Path, newName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //  Increase == 0の場合は何もしない
        }
    }
}
