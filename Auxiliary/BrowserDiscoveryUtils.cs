using AdvancedSharpAdbClient;
using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChromeDroid_TabMan.Auxiliary.ImportUtils;
using AdvancedSharpAdbClient.DeviceCommands;
using ChromeDroid_TabMan.Connection_and_Import;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

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

            return response.Split("\n").ToList();
        }

        public static void VerifyExistingSockets(List<BrowserComboItem> browserComboItemsToVerify, List<string> devToolsSocketsFound, string adbPath) //remember that C# passes objects and collections of objects as references.
        {
            for (int i = 0; i < browserComboItemsToVerify.Count; i++)
            {
                var browser = browserComboItemsToVerify[i];
                var currBD = browser.BrowserDetails;

                var socketFullName = browser.BrowserDetails.SocketNameFullOrPartial;
                if ((!currBD.IsSocketNameFull && currBD.PackageName != null) || currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName)//the || currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName condition fixes the condition where one of the other chromium browsers somehow gets the name default devtools socket before chrome.
                {

                    try
                    {
                        socketFullName = currBD.SocketNameFullOrPartial + "_" + ImportUtils.GetChromiumBrowserPid(adbPath, currBD.PackageName, false);
                    }
                    catch (PidNotParsedException pidEx)
                    {   //This mostly means the browser is not running, so we can safely mark the browser's sockets as not found.
                        browser.BrowserDetails = new BrowserDetailsStruct(currBD.BrowserName, currBD.PackageName, currBD.SocketNameFullOrPartial, currBD.IsSocketNameFull, DiscoveryStateEnum.NotFound);
                        continue;
                    }
                }
                if (devToolsSocketsFound.Any(s => (socketFullName == "localabstract:" + s.Replace("\r", "").Replace("\n", "").Replace("@", ""))))
                {
                    DiscoveryStateEnum discoveryState = DiscoveryStateEnum.Verified;
                    if (currBD.PackageName == ConfigHelper.ADB.Chrome_PackageName && socketFullName != ConfigHelper.ADB.Chrome_ForwardParameter_Remote)
                        discoveryState = DiscoveryStateEnum.RediscoveredAndFixed;
                    else if (!currBD.IsSocketNameFull)
                        discoveryState = DiscoveryStateEnum.RediscoveredAndFilledRestOfTheSocket;
                    browser.BrowserDetails = new BrowserDetailsStruct(currBD.BrowserName, currBD.PackageName, socketFullName, true, discoveryState);
                    devToolsSocketsFound.RemoveAll(s => (socketFullName == "localabstract:" + s.Replace("\r", "").Replace("\n", "").Replace("@", "")));
                }
                else if(currBD.PackageName==ConfigHelper.ADB.Chrome_PackageName && devToolsSocketsFound.Any(s=>s==ConfigHelper.ADB.Chrome_ForwardParameter_Remote))
                {
                    browser.BrowserDetails = new BrowserDetailsStruct(currBD.BrowserName, currBD.PackageName, ConfigHelper.ADB.Chrome_ForwardParameter_Remote, true, DiscoveryStateEnum.Verified);
                    //not removing this from devTools so that it can be verified later in the next stage.
                }
                else
                {
                    browser.BrowserDetails = new BrowserDetailsStruct(currBD.BrowserName, currBD.PackageName, currBD.SocketNameFullOrPartial, currBD.IsSocketNameFull, DiscoveryStateEnum.NotFound);
                }
            }
        }

        public static void DiscoverNewSocketsAndOrFixKnownBrowsersWithUnexpectedSocketNames(List<BrowserComboItem> existingBrowserComboList, List<string> devToolsSocketsFound, string adbPath_ProvideIfNewBrowserDetailsNeeded=null) //remember that C# passes objects and collections of objects as references.
        {
            string adbPath = adbPath_ProvideIfNewBrowserDetailsNeeded;
            HashSet<string> packageNameOfBrowsersThatCanBeFixed = existingBrowserComboList.Where(b => b.BrowserDetails.PackageName.Contains(ConfigHelper.ADB.EdgeAndBraveAndChrome_Base_ForwardParameterRemote__MissingPidAtEnd)).Select(b=>b.BrowserDetails.PackageName).ToHashSet();
            foreach (string currConnSocket in devToolsSocketsFound)
            {
                if (currConnSocket == string.Empty)
                    continue;
                string nSocket = currConnSocket.Replace("\r", "").Replace("\n", "").Replace("@", "");
                string currSocketFull = "localabstract:" + nSocket;
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

                        existingBrowserComboList[preexistingIndex] = new BrowserComboItem(currBD.BrowserName, currBD.PackageName, currSocketFull, true, discoveryState);
                        return;
                    }
                }
                BrowserComboItem browser = new BrowserComboItem(browserName, packageName, currSocketFull, true, DiscoveryStateEnum.Discovered);
                existingBrowserComboList.Add(browser);
            }
        }

        
        public static BrowserJsonVersionDTO GetBrowserDetails(string adbPath,string forwardParameter_Remote)
        {
            IAdbConnector adbConnector = new DiscoveredSocketOnlyAdbConnector(adbPath, forwardParameter_Remote);
            adbConnector.StartAdbJsonListServer();
            HttpClient httpClient = new();
            BrowserJsonVersionDTO response = httpClient.GetFromJsonAsync<BrowserJsonVersionDTO>(ConfigHelper.ADB.BrowserJsonVersionURL).Result;
            return response;
        }
    }
}
