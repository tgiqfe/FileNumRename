using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileNumRename.Lib
{
    internal class RenameHistory
    {
        private const string HISTORY_FILE = "History.json";

        public List<RenameHistoryItem> Histories { get; set; }

        public RenameHistory()
        {
            this.Histories = new();
        }

        public static RenameHistory Load()
        {
            try
            {
                return JsonSerializer.Deserialize<RenameHistory>(File.ReadAllText(HISTORY_FILE));
            }
            catch { }
            return new RenameHistory();
        }

        public void Save()
        {
            File.WriteAllText(HISTORY_FILE,
                JsonSerializer.Serialize(
                    this,
                    new JsonSerializerOptions() { WriteIndented = true }));
        }

        public void AddHistory(string hash, string fileName)
        {
            var history = Histories.FirstOrDefault(x => x.Hash == hash);
            string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (history == null)
            {
                Histories.Add(new RenameHistoryItem()
                {
                    Name = fileName,
                    Hash = hash,
                    History = new(new string[1] { $"[{now}] {fileName}" }),
                });
            }
            else
            {
                //  "[yyyy/MM/dd HH:mm:ss] " で、文字数22。
                string lastName = history.History.Last().Substring(22);
                if (lastName != fileName)
                {
                    history.History.Add($"[{now}] {fileName}");
                }
            }
        }
    }
}
