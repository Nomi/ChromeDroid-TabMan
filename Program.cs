using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChromeDroid_TabMan.Auxiliary;
using System.IO;

namespace ChromeDroid_TabMan
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// My additions:
            ConfigHelper.InitializeConfig();
            Directory.CreateDirectory(ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory);

            /// Pre-existing code:
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //TabsList tabbys = new TabsList(false);
            MainForm form1 = new MainForm();//can pass tabblist bby reference??
            Application.Run(form1); 
            // REPLACE STUFF WITH DATABINDING SOMEHOW?????
        }
    }
}
