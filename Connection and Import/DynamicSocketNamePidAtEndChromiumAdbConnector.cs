using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChromeDroid_TabMan.Auxiliary.ImportUtilities;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class DynamicSocketNamePidAtEndChromiumAdbConnector : IAdbConnector
    {
        public string AdbPath { get; }
        public string BrowserPackageName { get; }
        public string ForwardParameter_Remote { get; }
        public DynamicSocketNamePidAtEndChromiumAdbConnector(string adbPath, string browserPackageName, string baseForwardParameterRemote_MissingPidAtEnd)
        {
            AdbPath = adbPath;
            BrowserPackageName = browserPackageName;
            string chromiumDevToolsSocketName = baseForwardParameterRemote_MissingPidAtEnd + "_" + ImportUtilities.GetChromiumBrowserPid(AdbPath,BrowserPackageName);
            ForwardParameter_Remote = chromiumDevToolsSocketName;
        }

        public string StartAdbJsonListServer()
        {
            return ImportUtilities.StartChromeAndroidJsonListServer(AdbPath, BrowserPackageName, ForwardParameter_Remote);
        }
        
    }
}
