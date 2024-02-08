using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class DiscoveredSocketOnlyAdbConnector : IAdbConnector
    {
        public string AdbPath { get; }
        public string BrowserPackageName => "[NOT POSSIBLE TO FIND PACKAGE NAME FOR DISCOVERED BROWSERS]";//=> throw new Exception("Discovered browsers don't have a package name.");
        public string ForwardParameter_Remote { get; }
        public DiscoveredSocketOnlyAdbConnector(string adbPath, string forwardParameter_Remote)
        {
            AdbPath = adbPath;
            ForwardParameter_Remote = forwardParameter_Remote;
        }

        public string StartAdbJsonListServer()
        {
            return ImportUtils.StartChromeAndroidJsonListServer(AdbPath, BrowserPackageName, ForwardParameter_Remote,false);
        }
    }
}
