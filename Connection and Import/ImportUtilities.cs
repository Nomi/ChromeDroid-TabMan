using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
//using JQ.Net.JQ;
using System.Collections.Generic;
using System.Text.Json;
using ChromeDroid_TabMan.Auxiliary;
using Newtonsoft.Json;
using System.Linq;
using ChromeDroid_TabMan.Data;
using AdvancedSharpAdbClient;
using ChromeDroid_TabMan.Connection_and_Import;
using ChromeDroid_TabMan.DTOs;


//#define _USE_JQ


namespace ChromeDroid_TabMan.ConnectionAndImport
{

    public static class ImportUtilities
    {
        private enum NextStepImpUtil
        {
            StartADB,
            ConnectToDevice
        };
        private static System.Diagnostics.Process proc = null;
        private static NextStepImpUtil nextStep = NextStepImpUtil.StartADB;

        public struct ClientAndDevice_Adb
        {
            public AdbClient client;
            public DeviceData device;
        }
        public static ClientAndDevice_Adb ConnectAndGetAdbClientAndDevice(string adbPath)
        {
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
        public static string StartChromeAndroidJsonListServer(string adbPath,string browserPackageName,string browserRemoteForwardParameter)
        {
            ClientAndDevice_Adb clientAndDevice_Adb = ImportUtilities.ConnectAndGetAdbClientAndDevice(adbPath);
            AdbClient client = clientAndDevice_Adb.client;
            DeviceData device = clientAndDevice_Adb.device;

            client.StartApp(device, browserPackageName);

            string forwardParamLocal = ConfigHelper.ADB.ForwardParameter_Local;
            client.CreateForward(device,forwardParamLocal,browserRemoteForwardParameter,true);//procStartInfo.Arguments = " - d forward tcp:9222 localabstract:chrome_devtools_remote";
            return ConfigHelper.ADB.TabsJsonListURL;
        }

        public static string DownloadTabListJSON(string tabsJsonUrl="", string outputJsonFileName="")
        {
            if (tabsJsonUrl == "")
                tabsJsonUrl = ConfigHelper.ADB.TabsJsonListURL;
            if (outputJsonFileName == "")
                tabsJsonUrl = ConfigHelper.FileNamesAndPaths.OutputJsonFileName;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(tabsJsonUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            string text;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            ///The following is to rename old .json to .json.bak:
            // Source file to be renamed  
            //string sourceFile = ConfigHelper.JsonFileName;
            string prevOutPutJsonFileName = outputJsonFileName + ConfigHelper.FileNamesAndPaths.BackUpExtensionWithDot;
            // Creating FileInfo  
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(outputJsonFileName);
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

        public static List<BasicTabInf> LoadJson(string jsonPath="")
        {

            //bool usingDefaultPath = false;
            if (jsonPath == "")
            {
                // "\"" was needed for passing as argument to JQ, not needed anymore.
                jsonPath =  System.AppContext.BaseDirectory + ConfigHelper.FileNamesAndPaths.OutputJsonFileName; //"\"" + System.AppContext.BaseDirectory + +ConfigHelper.FileNamesAndPaths.JsonFileName+ "\"";
                //usingDefaultPath = true;
            }
            
            string jsonText = File.ReadAllText(jsonPath);

            return System.Text.Json.JsonSerializer.Deserialize<List<BasicTabInf>>(jsonText); //JsonConvert.DeserializeObject<>(jsonText);
        }
        public static void GetURLtxtAndTITLEtxtFromJSON(List<BasicTabInf> basicTabInfs)
        {
            File.WriteAllLines(ConfigHelper.FileNamesAndPaths.CurrentListOfURLsTxtFileName, basicTabInfs.Select(x=>x.url));//select preserves order.
            File.WriteAllLines(ConfigHelper.FileNamesAndPaths.CurrentListOfTitlesTxtFileName, basicTabInfs.Select(x => x.lastKnownTitle));//select preserves order.
        }


        public static string GetADBPathDialog()
        {
            //var fileContent = string.Empty;
            var filePath = string.Empty;
            bool IsCancelled = false;
            while (filePath == string.Empty && !IsCancelled)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "ADB Executable (adb.exe)|adb.exe|All Executables (*.exe)|*.exe";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;


                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file

                        filePath = openFileDialog.FileName;
                        if (filePath == string.Empty)
                        {
                            if (DialogResult.Cancel == MessageBox.Show("No file selected.", "Notice", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation))
                            {
                                IsCancelled = true;
                                return "-1";
                            }
                        }


                        ////Read the contents of the file into a stream
                        //var fileStream = openFileDialog.OpenFile();

                        //using (StreamReader reader = new StreamReader(fileStream))
                        //{
                        //    fileContent = reader.ReadToEnd();
                        //}
                    }
                    else
                    {
                        if (DialogResult.Cancel == MessageBox.Show("No file selected.", "Notice", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation))
                        {
                            IsCancelled = true;
                            return "-1";
                        }
                    }
                }
            }

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            MessageBox.Show("Selected executable: " + filePath, "ADB Executable Selected!" + filePath, MessageBoxButtons.OK);

            return filePath;
        }
    }
}
