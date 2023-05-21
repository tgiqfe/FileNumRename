using FileNumRename.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FileNumRename
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            List<string> paths = new();
            foreach (var arg in e.Args)
            {
                if (arg.Contains(";"))
                {
                    paths.AddRange(arg.Split(';'));
                }
                else
                {
                    paths.Add(arg);
                }
            }

            Item.Collection = new(paths.ToArray());
            Item.RenameHistory = RenameHistory.Load();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.RenameHistory.ClearOldLog();
            Item.RenameHistory.Save();
        }
    }
}
