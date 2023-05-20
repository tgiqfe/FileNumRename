using FileNumRename.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileNumRename
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Item.Collection = new(e.Args);
            Item.RenameHistory = RenameHistory.Load();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.RenameHistory.ClearOldLog();
            Item.RenameHistory.Save();
        }
    }
}
