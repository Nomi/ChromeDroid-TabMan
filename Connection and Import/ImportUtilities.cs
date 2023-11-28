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
using ChromeDroid_TabMan.Models;
using System.Linq;
using ChromeDroid_TabMan.Data;


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
        public static void StartChromeAndroidJsonListServer(string adbPath)
        {
            if (nextStep != NextStepImpUtil.StartADB)
            {
                //Implement error message here
                throw new NotImplementedException();
            }
            ProcessStartInfo procStartInfo;
            if (adbPath == string.Empty) //this if/else is just for testing right now. It can't be triggered normally, so it's fine. Will remove it some day.
                procStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Minimal ADB and Fastboot\adb.exe");
            else
                procStartInfo = new ProcessStartInfo(adbPath);
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.RedirectStandardInput = true;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            //procStartInfo.FileName = "adb.exe";
            procStartInfo.Arguments = "-d forward tcp:9222 localabstract:chrome_devtools_remote";
            //procStartInfo.WorkingDirectory = "C:\\Program Files(x86)\\Minimal ADB and Fastboot\\";
            proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            //string result= proc.StandardOutput.ReadLine() + "\n";
            nextStep = NextStepImpUtil.ConnectToDevice;
            proc.Dispose();
        }

        //public static void ConnectToDevice()
        //{
        //    if (nextStep != NextStepImpUtil.ConnectToDevice)
        //    {
        //        //Implement error message here
        //        throw new NotImplementedException();
        //    }
        //    proc.StandardInput.WriteLine("cd \"C:\\Program Files(x86)\\Minimal ADB and Fastboot\"");
        //    proc.StandardInput.WriteLine("./adb.exe devices");
        //    string result = "";
        //    int i = 0;
        //    while(i!=5)//!proc.StandardOutput.EndOfStream)
        //    {
        //        result += proc.StandardOutput.ReadLine()+ "\n";
        //        result += proc.StandardOutput.ReadLine() + "\n";
        //        i++;
        //    }
        //    result.ToLower();
        //}

        public static void DownloadTabListJSON()
        {
            //if(nextStep==NextStepImpUtil.StartADB)
            //{
            //    //Implement error message here
            //    throw new NotImplementedException();
            //}
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9222/json/list");
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
            // Creating FileInfo  
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(ConfigHelper.FileNamesAndPaths.JsonFileName);
            // Checking if file exists. 
            if (fileInfo.Exists)
            {
                // Move file with a new name. Hence renamed.  
                fileInfo.MoveTo(ConfigHelper.FileNamesAndPaths.PrevJsonNewFileName);
                //Console.WriteLine("File Renamed.");
            }
            File.WriteAllText(ConfigHelper.FileNamesAndPaths.JsonFileName, text, System.Text.Encoding.UTF8);
        }

        public static List<BasicTabInf> LoadJson(string jsonPath="")
        {

            //bool usingDefaultPath = false;
            if (jsonPath == "")
            {
                // "\"" was needed for passing as argument to JQ, not needed anymore but keeping as it is.
                jsonPath = "\"" + System.AppContext.BaseDirectory + ConfigHelper.FileNamesAndPaths.JsonFileName + "\""; //"\"" + System.AppContext.BaseDirectory + +ConfigHelper.FileNamesAndPaths.JsonFileName+ "\"";
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
