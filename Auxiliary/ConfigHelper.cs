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
        public static bool SortGroupsInGroupedHtmlAndNetscapeBookmarksAlphabetically = true;
        public static string UnidentifiedBaseUrlString => "**Unidentiffied BaseURLs**";
        public static string ExportFilesPartialDefaultName { get { return "recoveredTabs(" + DateTime.Now.ToString("dd-mm-yy__HH-mm-ss")+")";} }
        public static void InitializeConfig()
        {
            FileNamesAndPaths.InitializeConfig();
            Logging.InitializeConfig();
            Database.InitializeConfig();
        }
        public static class FileNamesAndPaths
        {
            public static string OutputPathDefaultExportDirectory => System.AppContext.BaseDirectory + @"Exports\";
            public static string BookmarksGroupedDefaultFileName => "Bookmarks_Grouped - " + ExportFilesPartialDefaultName + ".html";
            public static string BookmarksDefaultFileName => "Bookmarks - " + ExportFilesPartialDefaultName + ".html";
            public static string ListDefaultFileName => "LIST - " + ExportFilesPartialDefaultName + ".html";
            public static string CSVDefaultFileName => "CSV - " + ExportFilesPartialDefaultName + ".csv";
            public static void InitializeConfig()
            {
                OutputJsonFileName = "_chromtabJSON.json";
                PrevOutputJsonNewFileName = OutputJsonFileName + ".bak";
                CurrentListOfURLsTxtFileName = "CurrentListOfURLs.txt";
                CurrentListOfTitlesTxtFileName = "CurrentListOfTitles.txt";
            }
            public static string OutputJsonFileName { get; private set; }
            public static string BackUpExtensionWithDot => ".bak";
            public static string PrevOutputJsonNewFileName { get; private set; }
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
            public static string DbFileExtensionWithDot => ".sqlite3.db";
            public static string DbFileName => "SQLite DB - " + ExportFilesPartialDefaultName + DbFileExtensionWithDot;
            public static void InitializeConfig() 
            {
                //DbPath = "_LastTabs.db";
            }
            //public static string DbPath { get; private set; }
        }
        public static class ADB
        {
            public static string HostURL => "127.0.0.1:62001";
            public static string Chrome_PackageName => "com.android.chrome";
            public static string SamsungInternet_PackageName => "com.sec.android.app.sbrowser";
            public static string Opera_PackageName => "com.opera.browser";
            public static string Edge_PackageName => "com.microsoft.emmx";
            public static string Brave_PackageName => "com.brave.browser";
            public static string LocalHostString => "localhost";
            public static string ForwardParameter_Local_PortOnly => "9223";
            public static string ForwardParameter_Local => "tcp:"+ForwardParameter_Local_PortOnly;
            public static string SubUrlPathForTabsJsonList => "/json/list";
            public static string TabsJsonListURL => "http://" + LocalHostString + ":" + ForwardParameter_Local_PortOnly + SubUrlPathForTabsJsonList;
            public static string Chrome_ForwardParameter_Remote => "localabstract:chrome_devtools_remote";
            public static string SamsungInternet_ForwardParameter_Remote => "localabstract:Terrace_devtools_remote";
            public static string Opera_ForwardParameter_Remote => "localabstract:com.opera.browser.devtools";
            private static string EdgeAndBrave_ExceptionMessage_For_ForwardParameter_Remote => "For Edge and Brave, the ForwardParameter_Remote is the concatenation of \"localabstract:chrome_devtools_remote_\" with the process ID of the process of the browser instance at that time (can be found using pidof command).";
            //public static string Edge_ForwardParameter_Remote => throw new Exception(EdgeAndBrave_ExceptionMessage_For_ForwardParameter_Remote);
            //public static string Brave_ForwardParameter_Remote => throw new Exception(EdgeAndBrave_ExceptionMessage_For_ForwardParameter_Remote);
            public static string EdgeAndBrave_Base_ForwardParameterRemote__MissingPidAtEnd => Chrome_ForwardParameter_Remote;
        }
    }
}
