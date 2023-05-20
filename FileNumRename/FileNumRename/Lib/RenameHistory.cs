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

        #region load/save

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

        public void ClearOldLog()
        {
            int retention = 30;
            var borderTime = DateTime.Now.AddDays(retention * -1);
            Histories = Histories.Where(x => x.LastRenameTime > borderTime).ToList();
        }

        #endregion

        public void AddHistory(string hash, string fileName, DateTime now, string nowText)
        {
            var item = Histories.FirstOrDefault(x => x.Hash == hash);
            if (item == null)
            {
                Histories.Add(new RenameHistoryItem()
                {
                    Name = fileName,
                    Hash = hash,
                    LastRenameTime = now,
                    History = new(new string[1] { $"[{nowText}] {fileName}" }),
                });
            }
            else
            {
                //  "[yyyy/MM/dd HH:mm:ss] " で、文字数22。
                string lastName = item.History.Last().Substring(22);
                if (lastName != fileName)
                {
                    item.LastRenameTime = now;
                    item.History.Add($"[{nowText}] {fileName}");
                }
            }
        }
    }
}
