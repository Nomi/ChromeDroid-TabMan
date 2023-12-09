using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Principal;

namespace ChromeDroid_TabMan.Data
{
    internal class GroupedBasicHtmlListTabsExporter : ITabsExporter
    {
        public string OutputFile { get; }

        public GroupedBasicHtmlListTabsExporter(string outputFile = "")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').EndsWith(".html"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.ListDefaultFileName;
            OutputFile = outputFile;
        }

        public string Export(ITabsContainer tabsContainer)
        {
            string title = "Recovered tabs exported on " + DateTime.Now.ToString() + ".";

            using (FileStream fs = new FileStream(OutputFile, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<html>");
                    w.WriteLine("<head><title>"+title+"</title></head>");
                    w.WriteLine("<body>");
                    w.WriteLine("<H1><strong>----------------- TOTAL TABS RECOVERED= {0} -----------------</strong></H1>", tabsContainer.Count);

                    List<string> baseUrls = tabsContainer.BaseUrlToTabInfCollectionMap.Keys.ToList();
                    if (ConfigHelper.SortGroupsInGroupedHtmlAndNetscapeBookmarksAlphabetically)
                    {
                        baseUrls.Sort();
                    }
                    foreach (var baseurl in baseUrls)
                    {
                        w.Write("<H2><img width=\"20\" height=\"20\" src=\"https://{0}/favicon.ico\">", (baseurl as string));
                        w.Write(baseurl as string);
                        w.WriteLine("</H2>");
                        var sw = new StringWriter();
                        int count = 0;
                        foreach (var tab in tabsContainer.BaseUrlToTabInfCollectionMap[baseurl])
                        {
                            if ((tab as TabInf).BaseWebsite == (baseurl as string))
                            {
                                count++;
                                sw.WriteLine("<li><a href={0}>{1}</a></li>", (tab as TabInf).URL, (tab as TabInf).LastKnownTitle);
                            }
                        }
                        w.WriteLine("<b>Count: {0}.</b>", count);
                        w.WriteLine("<UL>");
                        w.Write(sw.ToString());
                        w.WriteLine("</UL>");
                    }
                    w.WriteLine("</body>");
                    w.WriteLine("</html>");
                    w.Flush();
                }
            }

            return OutputFile;
        }
    }
}
