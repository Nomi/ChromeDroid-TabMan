using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class AdbTabsJsonFetcher : ITabsJsonFetcher
    { 
        public IChromiumDevToolsConnector _ChromiumDevToolsConnector { get; }
        public AdbTabsJsonFetcher(IChromiumDevToolsConnector adbConnector)
        { 
            _ChromiumDevToolsConnector = adbConnector;
        }

        public string FetchTabsJson()
        {
            string jsonListTabsUrl = _ChromiumDevToolsConnector.StartAdbJsonListServer();
            return ImportUtils.DownloadTabListJSON(tabsJsonUrl: jsonListTabsUrl, outputJsonFileName: ConfigHelper.FileNamesAndPaths.OutputJsonFileName);
        }
    }
}
