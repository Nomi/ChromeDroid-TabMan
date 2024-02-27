using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal interface IChromiumDevToolsConnector
    {
        string AdbPath { get; }
        string BrowserPackageName { get; }
        string ForwardParameter_Remote { get; }
        public string StartAdbJsonListServer();
    }
}
