using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan
{
    public enum PathType
    {
        URL_txt=1,
        Title_txt=2
    }
    class TabsList
    {
        public int TabCount { get; private set; }
        private bool tabsProcessed = false;
        private string urlSrcPath = "CurrentListOfURLs.txt";
        private string titleSrcPath = "CurrentListOfTitles.txt";
        //public readonly ArrayList tabs = new ArrayList(); //Contains TabInf for each Tab.
        public readonly List<TabInf> tabs = new List<TabInf>(); //Contains TabInf for each Tab.
        //public readonly ArrayList baseURLs = new ArrayList();
        public readonly List<string> baseURLs = new List<string>();
        public TabsList(bool PrintEnabled)
        {
            //PathType type = PathType.URL_txt;
            //this.urlSrcPath = ReadPath(type);
            //type = PathType.Title_txt;
            //this.titleSrcPath = ReadPath(type);
            //this.titleSrcPath = "C:\\Users\\Noman\\Desktop\\ChromeAndroidTabs-TITLES.txt";
            //this.urlSrcPath = "C:\\Users\\Noman\\Desktop\\ChromeAndroidTabs-URL.txt";
            this.TabCount = 0;
            ProcessTabs(PrintEnabled);
        }
        
        //public TabsList()
        //{
        //    string fileName = "_chromtabJSON.json";
        //    string jsonString = File.ReadAllText("lol.json");//fileName);
        //    BasicTabInf t1 = JsonSerializer.Deserialize(jsonString, basic))
        //    string wow = t1.lastKnownTitle;
            
        //}

        public string ExportToHTML(string outputfile="")
        {
            string title = "recoveredTabs (" + DateTime.Now.ToString() + ")";
            string outputBaseDIR = string.Empty;
            if (outputfile.Length == 0 || !outputfile.Trim('"').EndsWith(".html"))
            {
                outputfile = "LIST - " + title + ".html";
                outputfile = outputfile.Replace("/", "-");
                outputfile = outputfile.Replace(":", "-");
                outputBaseDIR = System.AppContext.BaseDirectory + @"Exports\";
                //if(!Directory.Exists(outputBaseDIR)) //turns out, don't need this for Directory.CreateDirectory.
                //{
                //    Directory.CreateDirectory(outputBaseDIR);
                //}
                Directory.CreateDirectory(outputBaseDIR);
                outputfile = outputBaseDIR + outputfile;
            }
            using (FileStream fs = new FileStream(outputfile, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    //NEED TO MAKE MORE EFFICIENT!! (Not need, but should!)
                    foreach (var baseurl in baseURLs)
                    {
                        w.Write("<H1><img width=\"20\" height=\"20\" src=\"https://{0}/favicon.ico\">", (baseurl as string));
                        w.Write(baseurl as string);
                        w.WriteLine("</H1>");
                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in tabs)//NOTE TO SELF: TO DO : INEFFECCIENT AF, FIND DIFF METHOD
                        {
                            if ((tab as TabInf).baseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("<li><a href={0}>{1}</a></li>", (tab as TabInf).url, (tab as TabInf).lastKnownTitle);
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
            Console.WriteLine("Written to file: " + outputfile + " .");
            Console.ForegroundColor = ConsoleColor.White;
            return outputfile;
        }

        public string ExportToNetscapeBookmarksHTML(string outputfile = "", bool sort_baseURLs=false)
        {
            string title = "recoveredTabs (" + DateTime.Now.ToString() + ")";
            string outputBaseDIR = string.Empty;
            if (outputfile.Length == 0 || !outputfile.Trim('"').EndsWith(".html"))
            {
                outputfile = "Bookmarks - " + title+ ".html";
                outputfile = outputfile.Replace("/", "-");
                outputfile = outputfile.Replace(":", "-");
                outputBaseDIR = System.AppContext.BaseDirectory + @"Exports\";
                //if(!Directory.Exists(outputBaseDIR)) //turns out, don't need this for Directory.CreateDirectory.
                //{
                //    Directory.CreateDirectory(outputBaseDIR);
                //}
                Directory.CreateDirectory(outputBaseDIR);
                outputfile = outputBaseDIR+ outputfile;
            }
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
                        baseURLs.Sort();
                    }
                    foreach (var baseurl in baseURLs)
                    {
                        w.Write("       <DT><H3 ADD_DATE=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\"LAST_MODIFIED=\"" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "\">");
                        w.WriteLine((baseurl as string).Replace(".","_") + "</H3>");

                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in tabs)//NOTE TO SELF: TO DO : INEFFECCIENT AF, FIND DIFF METHOD
                        {
                            if ((tab as TabInf).baseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("          <DT><A HREF={0} ADD_DATE={2}>{1}</A>", (tab as TabInf).url, (tab as TabInf).lastKnownTitle, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
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

        private void ProcessTabs(bool PrintEnabled)
        {
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
                        TabInf tab = new TabInf(line, "title-to-be-implemented",++TabCount); //ADD TITLE HERE SOMEHOW!!
                        tab.lastKnownTitle = lastKnownTitle;
                        tabs.Add(tab);
                        //Might be able to get rid of the following if condition if I make a viable comparison operator,etc for sorting/grouping
                        if (line.Contains("://")) //Note to self: TO DO? : replace with try catch then inform user some entries were not normal URLs?
                        {
                            urlsRead++;
                            if (!this.baseURLs.Contains(tab.baseWebsite))
                            {
                                baseURLsRead++;
                                this.baseURLs.Add(tab.baseWebsite);
                                if(PrintEnabled)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("--new BaseURL found!--");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            if(PrintEnabled)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("[BaseURL: "+tab.baseWebsite+"]");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(line);
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("[Last Known Title: {0}]", tab.lastKnownTitle);
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
                            if (PrintEnabled)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("|--UNPROCESSED LINE:--| {0} [Last Known Title: {1}]", line,lastKnownTitle);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    if(PrintEnabled)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Total Lines Read: {0} |||| Lines read successfully: {1} |||| Total unique BaseURLs: {2}", urlsRead + unprocessedLines, urlsRead, baseURLsRead);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    //tabsProcessed = true;
                }
            }
            tabsProcessed = true;
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
