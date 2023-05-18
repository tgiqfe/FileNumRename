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
            Item.Collection = new(new string[]
            {
                @"D:\Sample_10_0001_98.txt",
                @"D:\Sample_10_0002_98.txt",
                @"D:\Sample_10_0003_98.txt",
                @"D:\Sample_10_0004_98.txt",
                @"D:\Sample_10_0005_98.txt",
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
