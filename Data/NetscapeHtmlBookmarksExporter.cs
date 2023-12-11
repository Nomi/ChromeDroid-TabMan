using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Data
{
    internal class NetscapeHtmlBookmarksExporter : ITabsExporter
    {
        public string OutputFile { get; }

        public NetscapeHtmlBookmarksExporter(string outputFile = "")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').ToLower().EndsWith(".html"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.BookmarksDefaultFileName;
            OutputFile= outputFile;
        }

        public string Export(ITabsContainer tabsContainer)
        {
            string title = "Recovered tabs exported on " + DateTime.Now.ToString() + ".";
            return ExportUsingBookmarksManagerNugetPackage(tabsContainer, title);
        }

        private string ExportUsingBookmarksManagerNugetPackage(ITabsContainer tabsContainer, string title) //does not support nested folders apparently.
        {
            var bookmarks = new BookmarksManager.BookmarkFolder(title);

            foreach (TabInf tab in tabsContainer.AllTabInfs)
            {
                var link = new BookmarksManager.BookmarkLink(tab.URL, tab.LastKnownTitle);
                bookmarks.Add(link);
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
