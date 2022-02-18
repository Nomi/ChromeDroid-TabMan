using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

enum NextStepImpUtil
{
    StartADB,
    ConnectToDevice,
    Autumn,
    Winter
}
public static class ImportUtilities
{
    private static System.Diagnostics.Process proc=null;
    private static NextStepImpUtil nextStep = NextStepImpUtil.StartADB;
    public static void StartChromeAndroidJsonListServer(string adbPath)
    {
        if(nextStep!=NextStepImpUtil.StartADB)
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
        procStartInfo.Arguments= "-d forward tcp:9222 localabstract:chrome_devtools_remote";
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
        
        using (FileStream fs = new FileStream("_chromtabJSON.json", FileMode.Create))
        {
            using (StreamWriter w = new StreamWriter(fs, System.Text.Encoding.UTF8))
            {
                w.Write(text);
                w.Flush();
            }
        }
    }


    public static void GetURLtxtAndTITLEtxtFromJSON(string jsonPath="")
    {
        bool usingDefaultPath = false;
        if (jsonPath == "")
        {
            jsonPath = "\"" + System.AppContext.BaseDirectory + "_chromtabJSON.json\"";
            usingDefaultPath = true;
        }

        Process proc = new Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = false;
        proc.StartInfo.RedirectStandardInput = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.FileName = "cmd.exe";
        //proc.StartInfo.Arguments = String.Format("/C " + Environment.CurrentDirectory.Replace('\\','/') + "/jq/jq.exe .[].url _chromtabJSON.json > lolnono.txt");
        string jqLocation = System.AppContext.BaseDirectory + @"\_redist\jq\";//jq.exe"; //"\""+System.AppContext.BaseDirectory + @"\_redist\jq\jq.exe" + "\"";
        proc.StartInfo.WorkingDirectory = jqLocation;
        proc.StartInfo.Arguments = String.Format("/C jq.exe .[].url \"" + jsonPath +"\" > \"" + System.AppContext.BaseDirectory+ "CurrentListOfURLs.txt\"");
        proc.Start();
        proc.WaitForExit();

        //proc.StartInfo.Arguments = String.Format("/C " + Environment.CurrentDirectory.Replace('\\','/') + "/jq/jq.exe .[].url _chromtabJSON.json > lolnono.txt");
        proc.StartInfo.Arguments = String.Format("/C jq.exe .[].title \"" + jsonPath + "\" > \"" + System.AppContext.BaseDirectory + "CurrentListOfTitles.txt\"");
        proc.Start();
        proc.WaitForExit();
        proc.Dispose();

        if(usingDefaultPath)
        {
            ///The following is to rename old .json to .json.bak:
            // Source file to be renamed  
            //string sourceFile = "_chromtabJSON.json";
            // Creating FileInfo  
            System.IO.FileInfo file = new System.IO.FileInfo(jsonPath);//(sourceFile);
            // Checking if file exists. 
            if (file.Exists)
            {
                // Move file with a new name. Hence renamed.  
                file.MoveTo("_chromtabJSON.json.bak");
                //Console.WriteLine("File Renamed.");
            }
        }

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
                        if (DialogResult.Cancel == MessageBox.Show("No file selected.", "Warning:", MessageBoxButtons.RetryCancel))
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
                    if (DialogResult.Cancel == MessageBox.Show("No file selected.", "Warning:", MessageBoxButtons.RetryCancel))
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
