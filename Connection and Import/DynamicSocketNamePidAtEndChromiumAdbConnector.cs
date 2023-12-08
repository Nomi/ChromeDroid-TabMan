using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.ConnectionAndImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChromeDroid_TabMan.ConnectionAndImport.ImportUtilities;

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
            string chromiumDevToolsSocketName = baseForwardParameterRemote_MissingPidAtEnd + "_" + GetChromiumBrowserPid();
            ForwardParameter_Remote = chromiumDevToolsSocketName;
        }

        public string StartAdbJsonListServer()
        {
            return ImportUtilities.StartChromeAndroidJsonListServer(AdbPath, BrowserPackageName, ForwardParameter_Remote);
        }
        private string GetChromiumBrowserPid()
        {
            ClientAndDevice_Adb clientAndDevice_Adb = ImportUtilities.ConnectAndGetAdbClientAndDevice(AdbPath);
            AdbClient client = clientAndDevice_Adb.client;
            DeviceData device = clientAndDevice_Adb.device;
            
            client.StartApp(device, BrowserPackageName);

            ConsoleOutputReceiver cOR = new ConsoleOutputReceiver();
            client.ExecuteShellCommand(device, "pidof " + BrowserPackageName,cOR);

            cOR.Flush();
            int pid = int.Parse(cOR.ToString()); //not try parse because right now, I want it to throw an exception if string is invalid.



            return pid.ToString();
        }
    }
}
