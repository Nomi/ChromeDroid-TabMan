using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.ConnectionAndImport;
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
        public IAdbConnector _adbConnector { get; }
        public AdbTabsJsonFetcher(IAdbConnector adbConnector)
        { 
            _adbConnector = adbConnector;
        }

        public string FetchTabsJson()
        {
            string jsonListTabsUrl = _adbConnector.StartAdbJsonListServer();
            return ImportUtilities.DownloadTabListJSON(tabsJsonUrl: jsonListTabsUrl, outputJsonFileName: ConfigHelper.FileNamesAndPaths.OutputJsonFileName);
        }
    }
}
