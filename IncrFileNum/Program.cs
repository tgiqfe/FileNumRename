using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace IncrFileNum
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && Directory.Exists(args[0]))
            {
                int row = 1;
                List<FileSummary> list = new();
                Directory.GetFiles(args[0]).ToList().ForEach(x =>
                {
                    var summary = new FileSummary(x);
                    if (summary.Enabled)
                    {
                        summary.Row = row++;
                        list.Add(summary);
                    }
                });

                var cursorPos = Console.GetCursorPosition();
                StateView state = new()
                {
                    SummaryList = list,
                    //StartCursorLeft = Console.CursorLeft,
                    StartCursorTop = Console.CursorTop,
                };

                bool during = true;
                while (during)
                {
                    Console.WriteLine(new string('=', Console.WindowWidth - 1));

                    //int row = 0;
                    foreach (var summary in list)
                    {
                        summary.WriteLine(state);
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
            string spaceRow = new string(' ', Console.WindowWidth - 1);
            for (int i = state.StartCursorTop; i < lastCursorTop; i++)
            {
                Console.WriteLine(spaceRow);
            }
            Console.SetCursorPosition(0, state.StartCursorTop);

            return true;
        }

        private static void Increase(StateView state)
        {
            if (state.Increase > 0) { state.SummaryList.Reverse(); }

            int resultPosLeft = state.SummaryList.Max(x => x.OutputLength) + 1;
            if (resultPosLeft > Console.WindowWidth - 9)
            {
                resultPosLeft = Console.WindowWidth - 9;
            }
            int lastCursorTop = Console.CursorTop;

            for (int i = 0; i < state.SummaryList.Count; i++)
            {
                string newName = state.SummaryList[i].GetNewName(state.Position, state.Increase);
                if (newName == null)
                {
                    Console.SetCursorPosition(
                        resultPosLeft,
                        state.StartCursorTop + state.SummaryList[i].Row);
                    Console.Write("[Skip]");
                }
                if (newName != null)
                {
                    try
                    {
                        FileSystem.RenameFile(state.SummaryList[i].Path, newName);
                        Console.SetCursorPosition(
                            resultPosLeft,
                            state.StartCursorTop + state.SummaryList[i].Row);
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Success");
                        Console.ResetColor();
                        Console.Write("]");
                    }
                    catch (Exception e)
                    {
                        Console.SetCursorPosition(
                            resultPosLeft,
                            state.StartCursorTop + state.SummaryList[i].Row);
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");

                        Console.SetCursorPosition(0, lastCursorTop);
                        Console.WriteLine();
                        Console.WriteLine(e);
                        return;
                    }
                }
            }

            Console.SetCursorPosition(0, lastCursorTop);
        }
    }
}
