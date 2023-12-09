using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChromeDroid_TabMan.Data
{
    internal class GroupedNetscapeHtmlBookmarksExporter : ITabsExporter
    {
        public string OutputFile { get; }

        public GroupedNetscapeHtmlBookmarksExporter(string outputFile="")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').EndsWith(".html"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.BookmarksDefaultFileName;
            OutputFile= outputFile;
        }
        public string Export(ITabsContainer tabsContainer)
        {
            string title = "Recovered tabs exported on " + DateTime.Now.ToString() + ".";

            return ExportUsingCustomSolution_WithoutDates(tabsContainer, title);
        }

        private string ExportUsingCustomSolution_WithoutDates(ITabsContainer tabsContainer, string title) //Supports nested folders.
        {
            using (FileStream fs = new FileStream(OutputFile, FileMode.Create))
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
                    w.Write("   <DT><H3>");
                    w.WriteLine(title + "</H3>");
                    w.WriteLine("   <DL><p>");

                    List<string> baseUrls = tabsContainer.BaseUrlToTabInfCollectionMap.Keys.ToList();
                    if (ConfigHelper.SortGroupsInGroupedHtmlAndNetscapeBookmarksAlphabetically)
                    {
                        baseUrls.Sort();
                    }
                    foreach (var baseurl in baseUrls)
                    {
                        string dateTimeUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        w.Write("       <DT><H3>");
                        w.WriteLine((baseurl as string).Replace(".", "_") + "</H3>");

                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in tabsContainer.BaseUrlToTabInfCollectionMap[baseurl])
                        {
                            if ((tab as TabInf).BaseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("          <DT><A HREF=\"{0}\">{1}</A>", (tab as TabInf).URL, (tab as TabInf).LastKnownTitle);
                            }
                        }
                        w.WriteLine("       <DL><p>");
                        w.Write(sw.ToString());
                        w.WriteLine("       </DL><p>");
                    }
                    w.WriteLine("   </DL><p>");
                    w.WriteLine("</DL><p>");
                    w.Flush();
                }
            }

            return OutputFile;
        }

        private string ExportUsingCustomSolution(ITabsContainer tabsContainer, string title) //Supports nested folders. //Probably could get rid of the dates part.
        {
            using (FileStream fs = new FileStream(OutputFile, FileMode.Create))
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

                    List<string> baseUrls = tabsContainer.BaseUrlToTabInfCollectionMap.Keys.ToList();
                    if (ConfigHelper.SortGroupsInGroupedHtmlAndNetscapeBookmarksAlphabetically)
                    {
                        baseUrls.Sort();
                    }
                    foreach (var baseurl in baseUrls)
                    {
                        string dateTimeUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        w.Write("       <DT><H3 ADD_DATE=\"" + dateTimeUnixTime + "\"LAST_MODIFIED=\"" + dateTimeUnixTime + "\">");
                        w.WriteLine((baseurl as string).Replace(".", "_") + "</H3>");

                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in tabsContainer.BaseUrlToTabInfCollectionMap[baseurl])
                        {
                            if ((tab as TabInf).BaseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("          <DT><A HREF=\"{0}\" ADD_DATE={2}>{1}</A>", (tab as TabInf).URL, (tab as TabInf).LastKnownTitle, dateTimeUnixTime);
                            }
                        }
                        w.WriteLine("       <DL><p>");
                        w.Write(sw.ToString());
                        w.WriteLine("       </DL><p>");
                    }
                    w.WriteLine("   </DL><p>");
                    w.WriteLine("</DL><p>");
                    w.Flush();
                }
            }

            return OutputFile;
        }

        private string ExportUsingBookmarksManagerNugetPackage(TabsContainer tabsContainer, string title) //does not support nested folders apparently.
        {
            var bookmarks = new BookmarksManager.BookmarkFolder(title);

            List<string> baseUrls = tabsContainer.BaseUrlToTabInfCollectionMap.Keys.ToList();
            if (ConfigHelper.SortGroupsInGroupedHtmlAndNetscapeBookmarksAlphabetically)
            {
                baseUrls.Sort();
            }

            foreach (var baseUrl in baseUrls)
            {
                var currFolder = new BookmarksManager.BookmarkFolder(baseUrl);
                foreach (var tabInf in tabsContainer.BaseUrlToTabInfCollectionMap[baseUrl])
                {
                    var link = new BookmarksManager.BookmarkLink(tabInf.URL, tabInf.LastKnownTitle);
                    currFolder.Add(link);
                }
                bookmarks.Add(currFolder);
            }


            var writer = new BookmarksManager.NetscapeBookmarksWriter(bookmarks);
            writer.OutputEncoding = Encoding.UTF8; //Encoding.GetEncoding(1257);


            using (var file = File.Open(OutputFile, FileMode.Create))
            {
                writer.Write(file);
            }

            return OutputFile;
        }
    }
}
