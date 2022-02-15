using System;
using System.IO;
using System.Diagnostics;
using System.Net;

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
    public static void StartChromeAndroidJsonListServer()
    {
        if(nextStep!=NextStepImpUtil.StartADB)
        {
            //Implement error message here
            throw new NotImplementedException();
        }
        ProcessStartInfo procStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Minimal ADB and Fastboot\adb.exe");
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


    public static void GetURLtxtAndTITLEtxtFromJSON()
    {
        Process proc = new Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = false;
        proc.StartInfo.RedirectStandardInput = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.FileName = "cmd.exe";
        //proc.StartInfo.Arguments = String.Format("/C " + Environment.CurrentDirectory.Replace('\\','/') + "/jq/jq.exe .[].url _chromtabJSON.json > lolnono.txt");
        proc.StartInfo.Arguments = String.Format("/C jq.exe .[].url _chromtabJSON.json > CurrentListOfURLs.txt");
        proc.Start();
        proc.WaitForExit();

        //proc.StartInfo.Arguments = String.Format("/C " + Environment.CurrentDirectory.Replace('\\','/') + "/jq/jq.exe .[].url _chromtabJSON.json > lolnono.txt");
        proc.StartInfo.Arguments = String.Format("/C jq.exe .[].title _chromtabJSON.json > CurrentListOfTitles.txt");
        proc.Start();
        proc.WaitForExit();
        proc.Dispose();
        int i = 0;
    }
}
