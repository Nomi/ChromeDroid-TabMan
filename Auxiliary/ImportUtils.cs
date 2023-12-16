using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
//using JQ.Net.JQ;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;
using ChromeDroid_TabMan.Data;
using AdvancedSharpAdbClient;
using ChromeDroid_TabMan.Connection_and_Import;
using ChromeDroid_TabMan.DTOs;
using System.Net.Http;
using AdvancedSharpAdbClient.DeviceCommands;
using System.Threading.Tasks;


//#define _USE_JQ


namespace ChromeDroid_TabMan.Auxiliary
{
    public struct ClientAndDevice_Adb
    {
        public AdbClient client;
        public DeviceData device;
    }
    public static class ImportUtils
    {
        public static ClientAndDevice_Adb ConnectAndGetAdbClientAndDevice(string adbPath)
        {
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                StartServerResult result = server.StartServer(adbPath, false); //(@"C:\adb\adb.exe", false);
                if (result != StartServerResult.Started)
                {
                    Console.WriteLine("Can't start adb server");
                }
            }

            AdbClient client;
            DeviceData device;

            client = new AdbClient();
            client.Connect(ConfigHelper.ADB.HostURL);
            device = client.GetDevices().FirstOrDefault(); // Get first connected device

            ClientAndDevice_Adb clientAndDevice_Adb = new ClientAndDevice_Adb();
            clientAndDevice_Adb.client = client;
            clientAndDevice_Adb.device = device;
            return clientAndDevice_Adb;
        }
        public static string GetChromiumBrowserPid(string adbPath, string browserPackageName, bool startBrowserAutomatically = true)
        {
            ClientAndDevice_Adb clientAndDevice_Adb = ImportUtils.ConnectAndGetAdbClientAndDevice(adbPath);
            AdbClient client = clientAndDevice_Adb.client;
            DeviceData device = clientAndDevice_Adb.device;

            if(startBrowserAutomatically)
                client.StartApp(device, browserPackageName);

            ConsoleOutputReceiver cOR = new ConsoleOutputReceiver();
            client.ExecuteShellCommand(device, "pidof " + browserPackageName, cOR);

            cOR.Flush();

            int pid;
            try
            {
                pid = int.Parse(cOR.ToString()); //could possibly have used TryParse in a better setup/context.
            }
            catch(System.FormatException e)
            {
                throw new PidNotParsedException();
            }

            return pid.ToString();
        }
        public static async Task<string> GetChromiumBrowserPidAsync(AdbConnection adbConnection, string browserPackageName, bool startBrowserAutomatically = true)
        {
            AdbClient client = adbConnection.client;
            DeviceData device = adbConnection.device;

            if (startBrowserAutomatically)
                await client.StartAppAsync(device, browserPackageName);

            ConsoleOutputReceiver cOR = new ConsoleOutputReceiver();
            await client.ExecuteShellCommandAsync(device, "pidof " + browserPackageName, cOR);

            cOR.Flush();

            int pid;
            try
            {
                pid = int.Parse(cOR.ToString()); //could possibly have used TryParse in a better setup/context.
            }
            catch (System.FormatException e)
            {
                throw new PidNotParsedException();
            }

            return pid.ToString();
        }
        public static string StartChromeAndroidJsonListServer(string adbPath, string browserPackageName, string browserRemoteForwardParameter, bool startBrowserAutomatically = true)
        {
            ClientAndDevice_Adb clientAndDevice_Adb = ConnectAndGetAdbClientAndDevice(adbPath);
            AdbClient client = clientAndDevice_Adb.client;
            DeviceData device = clientAndDevice_Adb.device;

            if(startBrowserAutomatically)
                client.StartApp(device, browserPackageName);

            string forwardParamLocal = ConfigHelper.ADB.ForwardParameter_Local;
            client.CreateForward(device, forwardParamLocal, browserRemoteForwardParameter, true);//procStartInfo.Arguments = " - d forward tcp:9222 localabstract:chrome_devtools_remote";
            return ConfigHelper.ADB.TabsJsonListURL;
        }

        public static string DownloadTabListJSON(string tabsJsonUrl = "", string outputJsonFileName = "")
        {
            if (tabsJsonUrl == "")
                tabsJsonUrl = ConfigHelper.ADB.TabsJsonListURL;
            if (outputJsonFileName == "")
                tabsJsonUrl = ConfigHelper.FileNamesAndPaths.OutputJsonFileName;

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, tabsJsonUrl);
            string text;
            var response = httpClient.SendAsync(request).Result;

            using (var sr = new StreamReader(response.Content.ReadAsStream()))//response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            ///The following is to rename old .json to .json.bak:
            // Source file to be renamed  
            //string sourceFile = ConfigHelper.JsonFileName;
            string prevOutPutJsonFileName = outputJsonFileName + ConfigHelper.FileNamesAndPaths.BackUpExtensionWithDot;
            // Creating FileInfo  
            FileInfo fileInfo = new FileInfo(outputJsonFileName);
            // Checking if file exists. 
            if (fileInfo.Exists)
            {
                // Move file with a new name. Hence renamed.  
                File.Delete(prevOutPutJsonFileName);
                fileInfo.MoveTo(prevOutPutJsonFileName);
                //Console.WriteLine("File Renamed.");
            }
            File.WriteAllText(outputJsonFileName, text, System.Text.Encoding.UTF8);

            return outputJsonFileName;
        }

        public static List<BasicTabInf> LoadJson(string jsonPath = "")
        {

            //bool usingDefaultPath = false;
            if (jsonPath == "")
            {
                // "\"" was needed for passing as argument to JQ, not needed anymore.
                jsonPath = AppContext.BaseDirectory + ConfigHelper.FileNamesAndPaths.OutputJsonFileName; //"\"" + System.AppContext.BaseDirectory + +ConfigHelper.FileNamesAndPaths.JsonFileName+ "\"";
                //usingDefaultPath = true;
            }

            string jsonText = File.ReadAllText(jsonPath);

            return System.Text.Json.JsonSerializer.Deserialize<List<BasicTabInf>>(jsonText); //JsonConvert.DeserializeObject<>(jsonText);
        }
        public static void GetURLtxtAndTITLEtxtFromJSON(List<BasicTabInf> basicTabInfs)
        {
            File.WriteAllLines(ConfigHelper.FileNamesAndPaths.CurrentListOfURLsTxtFileName, basicTabInfs.Select(x => x.url));//select preserves order.
            File.WriteAllLines(ConfigHelper.FileNamesAndPaths.CurrentListOfTitlesTxtFileName, basicTabInfs.Select(x => x.lastKnownTitle));//select preserves order.
        }
    }
}
