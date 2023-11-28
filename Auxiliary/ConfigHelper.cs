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
        public static void InitializeConfig()
        {
            FileNamesAndPaths.InitializeConfig();
        }
        public static class FileNamesAndPaths
        {
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
    }
}
