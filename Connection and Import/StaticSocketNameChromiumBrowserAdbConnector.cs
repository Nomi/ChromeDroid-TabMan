using AdvancedSharpAdbClient;
using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class StaticSocketNameChromiumAdbConnector : IAdbConnector
    {
        public string AdbPath { get; }
        public string BrowserPackageName { get; }
        public string ForwardParameter_Remote { get; }
        public StaticSocketNameChromiumAdbConnector(string adbPath, string browserPackageName, string forwardParameter_Remote)
        {
            AdbPath = adbPath;
            BrowserPackageName = browserPackageName;
            ForwardParameter_Remote = forwardParameter_Remote;
        }

        public string StartAdbJsonListServer()
        {
            return ImportUtils.StartChromeAndroidJsonListServer(AdbPath, BrowserPackageName, ForwardParameter_Remote);
        }
    }
}
