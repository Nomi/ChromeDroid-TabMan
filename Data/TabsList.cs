using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using JQ.Net;
//using JQ.Net.JQ;

namespace ChromeDroid_TabMan.Data
{
    //Singleton:
    public class TabsList
    {
        private static TabsList instance = null;
        public enum PathType
        {
            URL_txt = 1,
            Title_txt = 2
        }
        public bool TabsProcessed { get; private set; }
        public int TabCount { get { return Tabs.Count; } }
        public List<TabInf> Tabs { get; private set; } //Contains TabInf for each Tab.


        private readonly string urlSrcPath = ConfigHelper.FileNamesAndPaths.CurrentListOfURLsTxtFileName;
        private readonly string titleSrcPath = ConfigHelper.FileNamesAndPaths.CurrentListOfTitlesTxtFileName;
        



        //public readonly ArrayList baseURLs = new ArrayList();
        public List<string> BaseURLs { get; private set; }
        private TabsList()
        {
            ResetTabList();
        }
        public static TabsList GetInstance()
        {
            if(instance == null)
               instance = new TabsList();
            return instance;
        }
        public void ResetTabList()
        {
            TabsProcessed = false;
            BaseURLs = new();
            Tabs = new();
        }
        public void Process(List<BasicTabInf> basicTabInfs = null)
        {
            if(basicTabInfs==null)
            {
                ProcessTabsFromTxts();
                return;
            }

            Tabs = new();
            BaseURLs = new();
            HashSet<string> baseUrlsEncountered = new();
            for(int i=0;i<basicTabInfs.Count;i++)
            {
                BasicTabInf bti = basicTabInfs[i];
                Tabs.Add(new TabInf(bti.url, bti.lastKnownTitle, i + 1));
                if (!baseUrlsEncountered.Contains(Tabs[i].BaseWebsite))
                {
                    baseUrlsEncountered.Add(Tabs[i].BaseWebsite);
                    BaseURLs.Add(Tabs[i].BaseWebsite);
                }
            }
            return;
        }

        public string ExportToSqliteDB(string outputFile="")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').EndsWith(".html"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.Database.DbFileName;
            
            TabsDbContext tabsDbContext = new(outputFile);
            System.IO.File.Delete(outputFile);
            tabsDbContext.Database.EnsureCreated();
            tabsDbContext.Tabs.AddRange(TabsList.GetInstance().Tabs);
            tabsDbContext.SaveChanges();

            return outputFile;
        }
        public string ExportToGroupedListHTML(string outputFile="")
        {
            string title = "recoveredTabs (" + DateTime.Now.ToString() + ")";

            if (outputFile.Length == 0 || !outputFile.Trim('"').EndsWith(".html"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.ListDefaultFileName;

            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<H1><strong>----------------- TOTAL TABS RECOVERED= {0} -----------------</strong></H1>", TabCount);
                    //NEED TO MAKE MORE EFFICIENT!! (Not need, but should!)
                    foreach (var baseurl in BaseURLs)
                    {
                        w.Write("<H1><img width=\"20\" height=\"20\" src=\"https://{0}/favicon.ico\">", (baseurl as string));
                        w.Write(baseurl as string);
                        w.WriteLine("</H1>");
                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in Tabs)//NOTE TO SELF: TO DO : INEFFECCIENT AF, FIND DIFF METHOD
                        {
                            if ((tab as TabInf).BaseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("<li><a href={0}>{1}</a></li>", (tab as TabInf).URL, (tab as TabInf).LastKnownTitle);
                                //w.WriteLine("<li><a href={0}>{1}</a></li>",(tab as TabInf).url, (tab as TabInf).lastKnownTitle);
                            }
                        }
                        w.WriteLine("<b>Count: {0}.</b>", count);
                        w.WriteLine("<UL>");
                        w.Write(sw.ToString());
                        w.WriteLine("</UL>");

                        //Console.WriteLine(item.ToString());
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Written to file: " + outputFile + " .");
            Console.ForegroundColor = ConsoleColor.White;
            return outputFile;
        }

        public string ExportToNetscapeBookmarksHTML(string outputfile = "", bool sort_baseURLs=false)
        {
            string title = "recoveredTabs (" + DateTime.Now.ToString() + ")";
            if (outputfile.Length == 0 || !outputfile.Trim('"').EndsWith(".html"))
                outputfile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.BookmarksDefaultFileName;

            using (FileStream fs = new FileStream(outputfile, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<!DOCTYPE NETSCAPE-Bookmark-file-1>");
                    w.WriteLine("<!-- This is an automatically generated file.");
                    w.WriteLine("     It will be read and overwritten.");
                    w.WriteLine("     DO NOT EDIT! -->");
                    w.WriteLine("<META HTTP-EQUIV=\"Content - Type\" CONTENT=\"text / html; charset = UTF - 8\">\n" +
                        "        <TITLE> Bookmarks </TITLE>\n" +
                        "        <H1> Bookmarks </H1>");

                    w.WriteLine("<DL><p>");
                    w.Write("   <DT><H3 ADD_DATE=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\"LAST_MODIFIED=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\">");
                    w.WriteLine(title + "</H3>");
                    w.WriteLine("   <DL><p>");
                    //NEED TO MAKE MORE EFFICIENT!! (Technically, not needed but should do so!)
                    if(sort_baseURLs)
                    {
                        BaseURLs.Sort();
                    }
                    foreach (var baseurl in BaseURLs)
                    {
                        w.Write("       <DT><H3 ADD_DATE=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\"LAST_MODIFIED=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\">");
                        w.WriteLine((baseurl as string).Replace(".","_") + "</H3>");

                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in Tabs)//NOTE TO SELF: TO DO : INEFFECCIENT AF, FIND DIFF METHOD
                        {
                            if ((tab as TabInf).BaseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("          <DT><A HREF={0} ADD_DATE={2}>{1}</A>", (tab as TabInf).URL, (tab as TabInf).LastKnownTitle, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
                                //w.WriteLine("<li><a href={0}>{1}</a></li>",(tab as TabInf).url, (tab as TabInf).lastKnownTitle);
                            }
                        }
                        w.WriteLine("       <DL><p>");
                        w.Write(sw.ToString());
                        w.WriteLine("       </DL><p>");
                        //Console.WriteLine(item.ToString());
                    }
                    w.WriteLine("   </DL><p>");
                    w.WriteLine("</DL><p>");
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Written to file: " + outputfile + " .");
            Console.ForegroundColor = ConsoleColor.White;
            return outputfile;
        }


        
        private void ProcessTabsFromTxts()//uses .txt files containing URL and titles respectively for constructing the list. This has basically been deprecated.
        {
            bool printEnabled = ConfigHelper.Logging.EnablePrintingInTabsListProcessingFromTxts;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            int urlsRead = 0, baseURLsRead = 0, titlesRead = 0, unprocessedLines = 0;
            using (StreamReader titleReader = File.OpenText(this.titleSrcPath))
            {
                using (StreamReader reader = File.OpenText(this.urlSrcPath))
                {
                    string line = String.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string lastKnownTitle = titleReader.ReadLine();
                        titlesRead++;
                        TabInf tab = new TabInf(line, "title-to-be-implemented", 1+TabCount);//++TabCount); //ADD TITLE HERE SOMEHOW!!
                        tab.LastKnownTitle = lastKnownTitle;
                        Tabs.Add(tab);
                        //Might be able to get rid of the following if condition if I make a viable comparison operator,etc for sorting/grouping
                        if (line.Contains("://")) //Note to self: TO DO? : replace with try catch then inform user some entries were not normal URLs?
                        {
                            urlsRead++;
                            if (!this.BaseURLs.Contains(tab.BaseWebsite))
                            {
                                baseURLsRead++;
                                this.BaseURLs.Add(tab.BaseWebsite);
                                if(printEnabled)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("--new BaseURL found!--");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            if(printEnabled)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("[BaseURL: "+tab.BaseWebsite+"]");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(line);
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("[Last Known Title: {0}]", tab.LastKnownTitle);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            unprocessedLines++;
                            //if (!this.baseURLs.Contains(tab.baseWebsite))
                            //{
                            //    this.baseURLs.Add(tab.baseWebsite);
                            //}
                            if (printEnabled)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("|--UNPROCESSED LINE:--| {0} [Last Known Title: {1}]", line,lastKnownTitle);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    if(printEnabled)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Total Lines Read: {0} |||| Lines read successfully: {1} |||| Total unique BaseURLs: {2}", urlsRead + unprocessedLines, urlsRead, baseURLsRead);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    //tabsProcessed = true;
                }
            }
            TabsProcessed = true;
        }
        private static string ReadPath(PathType type)
        {
            string _path = "";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Enter path of " + type.ToString() + " file:" );
            Console.ForegroundColor = ConsoleColor.White;
            _path = Console.ReadLine();
            //char[] charsToTrim = { '"' };
            _path=_path.Trim('"');
            while (!File.Exists(_path) || (!_path.ToLower().EndsWith(".txt")))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Incorrect input! Try again! {0} Enter path of " + type.ToString() + " file: {1}", Environment.NewLine, Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.White;
                _path = Console.ReadLine();
            }
            return _path;
        }
    }
}
