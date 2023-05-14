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
            //Item.Collection = new(e.Args);
            Item.Collection = new(new string[]
            {
                @"D:\Sample_0001.txt",
                @"D:\Sample_0002.txt",
                @"D:\Sample_0003.txt",
                @"D:\Sample_0004.txt",
                @"D:\Sample_0005.txt",
            });
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
