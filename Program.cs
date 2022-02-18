using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
