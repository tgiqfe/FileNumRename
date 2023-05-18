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
            /*
            Item.Collection = new(new string[]
            {
                @"D:\Sample_10_0001_99.txt",
                @"D:\Sample_10_0002_99.txt",
                @"D:\Sample_10_0003_99.txt",
                @"D:\Sample_10_0004_99.txt",
                @"D:\Sample_10_0005_99.txt",
            });
            */
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
