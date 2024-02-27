using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Receivers;
using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChromeDroid_TabMan.Auxiliary.ImportUtils;
using ChromeDroid_TabMan.Connection_and_Import;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.PortableExecutable;
using System.IO.Pipes;
using ChromeDroid_TabMan.Auxiliary.Exceptions;

namespace ChromeDroid_TabMan.Auxiliary
{
    public static class BrowserDiscoveryUtils
    {
        public static List<BrowserComboItem> GetDefaultBrowserComboItems()
        {
            return new List<BrowserComboItem>
            {
                new BrowserComboItem("Chrome",ConfigHelper.ADB.Chrome_PackageName,ConfigHelper.ADB.Chrome_ForwardParameter_Remote,true,DiscoveryStateEnum.NotSearchedFor),
                new BrowserComboItem("Opera",ConfigHelper.ADB.Opera_PackageName,ConfigHelper.ADB.Opera_ForwardParameter_Remote,true,DiscoveryStateEnum.NotSearchedFor),
                new BrowserComboItem("SamsungInternet",ConfigHelper.ADB.SamsungInternet_PackageName,ConfigHelper.ADB.SamsungInternet_ForwardParameter_Remote,true,DiscoveryStateEnum.NotSearchedFor),
                new BrowserComboItem("Edge",ConfigHelper.ADB.Edge_PackageName,ConfigHelper.ADB.EdgeAndBraveAndChrome_Base_ForwardParameterRemote__MissingPidAtEnd,false,DiscoveryStateEnum.NotSearchedFor),
                new BrowserComboItem("Brave",ConfigHelper.ADB.Brave_PackageName,ConfigHelper.ADB.EdgeAndBraveAndChrome_Base_ForwardParameterRemote__MissingPidAtEnd,false,DiscoveryStateEnum.NotSearchedFor)
            };
        }

        public static List<string> GetDevToolsSockets(string adbPath)
        {
            ClientAndDevice_Adb clientAndDevice_Adb = ImportUtils.ConnectAndGetAdbClientAndDevice(adbPath);
            AdbClient client = clientAndDevice_Adb.client;
            DeviceData device = clientAndDevice_Adb.device;

            ConsoleOutputReceiver cOR = new();
            client.ExecuteShellCommand(device, @"ss -a 2>/dev/null| grep devtools| cut -F 5", cOR);
            string response = cOR.ToString();

            List<string> result = response.Split("\n").Select(s => s.Replace("\r", "").Replace("\n", "").Replace("@", "")).ToList();
            result.RemoveAll(s => (s == string.Empty));
            return result;
        }
        public static async Task<List<string>> GetDevToolsSocketsNamesAsync(AdbConnection adbConnection)
        {
            AdbClient client = adbConnection.client;
            DeviceData device = adbConnection.device;

            ConsoleOutputReceiver cOR = new();
            await client.ExecuteShellCommandAsync(device, @"ss -a 2>/dev/null| grep devtools| cut -F 5", cOR);
            string response = cOR.ToString();

            List<string> result = response.Split("\n").Select(s => s.Replace("\r", "").Replace("\n", "").Replace("@", "")).ToList();
            result.RemoveAll(s => (s == string.Empty));
            return result;
        }

        public static async Task VerifyExistingSocketsAsync(List<BrowserComboItem> browserComboItemsToVerify, List<string> devToolsSocketsFound, AdbConnection adbConnection) //remember that C# passes objects and collections of objects as references.
        {
            List<Task> runningTasks = new List<Task>();
            for (int i = 0; i < browserComboItemsToVerify.Count; i++)
            {
                var browser = browserComboItemsToVerify[i];
                runningTasks.Add(verifyBrowserDevToolsSocketAsync(browser, devToolsSocketsFound, adbConnection));
            }
            await Task.WhenAll(runningTasks);
            return; //Task.CompletedTask
        }
        private static async Task verifyBrowserDevToolsSocketAsync(BrowserComboItem browser, List<string> devToolsSocketsFound, AdbConnection adbConnection)
        {
            var currBD = browser.BrowserDetails;
            var socketFullName = browser.BrowserDetails.Socket.SocketConnectionStr;
            var socketNameOnly = browser.BrowserDetails.Socket.Name;
            if ((!currBD.Socket.IsSocketNameComplete && currBD.PackageName != null) || currBD.PackageName == ConfigHelper.ADB.Chrome_PackageName)//the || currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName condition fixes the condition where one of the other chromium browsers somehow gets the name default devtools socket before chrome.
            {

                try
                {
                    string pid = (await ImportUtils.GetChromiumBrowserPidAsync(adbConnection, currBD.PackageName, false));
                    socketNameOnly += "_" + pid;
                    socketFullName += "_" + pid;
                }
                catch (PidNotParsedException pidEx)
                {   //This mostly means the browser is not running, so we can safely mark the browser's sockets as not found.
                    browser.BrowserDetails.DiscoveryState = DiscoveryStateEnum.NotFound;
                    return;
                }
            }
            if (devToolsSocketsFound.Any(s => (socketNameOnly == s)))
            {
                DiscoveryStateEnum discoveryState = DiscoveryStateEnum.Verified;
                if (currBD.PackageName == ConfigHelper.ADB.Chrome_PackageName && socketFullName != ConfigHelper.ADB.Chrome_ForwardParameter_Remote)
                    discoveryState = DiscoveryStateEnum.RediscoveredAndFixed;
                else if (!currBD.Socket.IsSocketNameComplete)
                    discoveryState = DiscoveryStateEnum.RediscoveredAndFilledRestOfTheSocket;

                browser.BrowserDetails.Socket.Name = socketNameOnly;
                browser.BrowserDetails.Socket.IsSocketNameComplete = true;
                browser.BrowserDetails.DiscoveryState = discoveryState;
                devToolsSocketsFound.RemoveAll(s => (socketNameOnly == s));
            }
            else if (currBD.PackageName == ConfigHelper.ADB.Chrome_PackageName && devToolsSocketsFound.Any(s => s == ConfigHelper.ADB.Chrome_DevToolRemote_String))
            {
                browser.BrowserDetails.DiscoveryState = DiscoveryStateEnum.Verified;
                //not removing this from devTools so that it can be verified later in the next stage.
            }
            else
            {
                browser.BrowserDetails.DiscoveryState = DiscoveryStateEnum.NotFound;
            }
        }

        //The following cannot be async!
        public static void DiscoverNewSocketsAndOrFixKnownBrowsersWithUnexpectedSocketNames(List<BrowserComboItem> existingBrowserComboList, List<string> devToolsSocketsFound, string adbPath_ProvideIfNewBrowserDetailsNeeded=null) //remember that C# passes objects and collections of objects as references.
        {
            string adbPath = adbPath_ProvideIfNewBrowserDetailsNeeded;
            HashSet<string> packageNameOfBrowsersThatCanBeFixed = existingBrowserComboList.Where(b => b.BrowserDetails.PackageName.Contains(ConfigHelper.ADB.EdgeAndBraveAndChrome_Base_ForwardParameterRemote__MissingPidAtEnd)).Select(b=>b.BrowserDetails.PackageName).ToHashSet();
            foreach (string currSocketName in devToolsSocketsFound)
            {
                string currSocketFull = ConfigHelper.ADB.LocalAbstractString + ":" + currSocketName;
                string browserName = currSocketFull;
                string packageName = null;
                if(adbPath!=null) //this means that we are supposed to fill in the details for each newly discovered device.
                {
                    var res = BrowserDiscoveryUtils.GetBrowserDetails(adbPath, currSocketFull);
                    browserName = res.OnlyNameBrowser;
                    packageName = res.AndroidPackageName;
                    int preexistingIndex = existingBrowserComboList.FindIndex(b => (b.BrowserDetails.PackageName == packageName));
                    if (-1!=preexistingIndex) //fixing old browsers.
                    {
                        var currBD = existingBrowserComboList[preexistingIndex].BrowserDetails;
                        DiscoveryStateEnum discoveryState = DiscoveryStateEnum.RediscoveredAndFilledRestOfTheSocket;

                        //if (packageNameOfBrowsersThatCanBeFixed.Contains(currBD.PackageName))
                        //{
                        if (currBD.PackageName == ConfigHelper.ADB.Chrome_PackageName) 
                        {
                            //verifying chrome when it gets a different socket than Chrome default.
                            if (currSocketFull != ConfigHelper.ADB.Chrome_ForwardParameter_Remote)
                                discoveryState = DiscoveryStateEnum.RediscoveredAndFixed;
                            else if(currSocketFull == ConfigHelper.ADB.Chrome_ForwardParameter_Remote)
                                discoveryState = DiscoveryStateEnum.Verified;
                        }
                        else if (currBD.PackageName == ConfigHelper.ADB.Brave_PackageName || currBD.PackageName == ConfigHelper.ADB.Edge_PackageName) //verifying edge or brave when they get the default Chrome socket.
                        {
                            if (currSocketFull == ConfigHelper.ADB.Chrome_ForwardParameter_Remote)
                                discoveryState = DiscoveryStateEnum.RediscoveredAndFixed;
                        }
                        //}

                        existingBrowserComboList[preexistingIndex].BrowserDetails.DiscoveryState = discoveryState;
                        return;
                    }
                }
                SocketInfo socket = new SocketInfo(currSocketFull, true, true);
                BrowserInfo browser = new BrowserInfo(browserName, packageName, socket, DiscoveryStateEnum.Discovered);
                BrowserComboItem browserCI = new BrowserComboItem(browser);
                existingBrowserComboList.Add(browserCI);
            }
        }

        
        public static BrowserJsonVersionDTO GetBrowserDetails(string adbPath,string forwardParameter_Remote)
        {
            IChromiumDevToolsConnector adbConnector = new DiscoveredSocketOnlyDevToolsConnector(adbPath, forwardParameter_Remote);
            adbConnector.StartAdbJsonListServer();
            HttpClient httpClient = new();
            BrowserJsonVersionDTO response = httpClient.GetFromJsonAsync<BrowserJsonVersionDTO>(ConfigHelper.ADB.BrowserJsonVersionURL).Result;
            return response;
        }
    }
}
