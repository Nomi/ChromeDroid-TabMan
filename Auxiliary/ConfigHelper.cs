using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Auxiliary
{
    public static class ConfigHelper
    {
        public static string ExportFilesPartialDefaultName { get { return "recoveredTabs(" + DateTime.Now.ToString("dd-mm-yy__HH-mm-ss")+")";} }
        public static void InitializeConfig()
        {
            FileNamesAndPaths.InitializeConfig();
            Logging.InitializeConfig();
            Database.InitializeConfig();
        }
        public static class FileNamesAndPaths
        {
            public static string OutputPathDefaultExportDirectory { get; private set; } = System.AppContext.BaseDirectory + @"Exports\";
            public static string BookmarksDefaultFileName { get { return "Bookmarks -" + ExportFilesPartialDefaultName + ".html"; } }
            public static string ListDefaultFileName { get { return "LIST -" + ExportFilesPartialDefaultName + ".html"; } }
            public static void InitializeConfig()
            {
                JsonFileName = "_chromtabJSON.json";
                PrevJsonNewFileName = JsonFileName + ".bak";
                CurrentListOfURLsTxtFileName = "CurrentListOfURLs.txt";
                CurrentListOfTitlesTxtFileName = "CurrentListOfTitles.txt";
            }
            public static string JsonFileName { get; private set; }
            public static string PrevJsonNewFileName { get; private set; }
            public static string CurrentListOfURLsTxtFileName { get; private set; }
            public static string CurrentListOfTitlesTxtFileName { get; private set; }
        }
        public static class Logging
        {
            public static void InitializeConfig()
            {
                EnablePrintingInTabsListProcessingFromTxts = true;
            }
            public static bool EnablePrintingInTabsListProcessingFromTxts { get; private set; } //= false;
        }
        public static class Database
        {
            public static string DbFileName {   get { return "SQLite DB -" + ExportFilesPartialDefaultName + ".sqlite3.db";}  }
            public static void InitializeConfig() 
            {
                //DbPath = "_LastTabs.db";
            }
            //public static string DbPath { get; private set; }
        }
        public static class ADB
        {
            public static string HostURL { get; private set; } = "127.0.0.1:62001";
            public static string ChromePackageName { get; private set; } = "com.android.chrome";
            public static string ForwardParameter_Local { get; private set; } = "tcp:9222";
            public static string ForwardParameter_Remote { get; private set; } = "localabstract:chrome_devtools_remote";
            public static string TabsJsonListURL { get; private set; } = "http://localhost:9222/json/list";
        }
    }
}
