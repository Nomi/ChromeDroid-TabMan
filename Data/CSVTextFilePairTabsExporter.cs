using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using ChromeDroid_TabMan.Auxiliary;

namespace ChromeDroid_TabMan.Data
{
    internal class CSVTextFilePairTabsExporter : ITabsExporter
    {
        public string OutputFile { get; }

        public CSVTextFilePairTabsExporter(string outputFile = "")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').ToLower().EndsWith(".csv"))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.FileNamesAndPaths.CSVDefaultFileName;
            OutputFile = outputFile;
        }

        public string Export(ITabsContainer tabsContainer)
        {
            using (StreamWriter outfile = new StreamWriter(OutputFile,false))
            {
                outfile.Write("tab_num,title,url,base_url");
                foreach(TabInf tab in tabsContainer.AllTabInfs)
                {
                    outfile.Write("\n");
                    outfile.Write($"\"{tab.TabNum}\",\"{tab.LastKnownTitle}\",\"{tab.URL}\",\"{tab.BaseWebsite}\"");
                }
                outfile.Flush();
            }
            return OutputFile;
        }
    }
}
