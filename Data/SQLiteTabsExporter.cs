using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Data
{
    internal class SQLiteTabsExporter : ITabsExporter
    {
        TabsDbContext _tabsDbContext;
        public string OutputFile { get; }

        public SQLiteTabsExporter(string outputFile="")
        {
            if (outputFile.Length == 0 || !outputFile.Trim('"').EndsWith(ConfigHelper.Database.DbFileExtensionWithDot))
                outputFile = ConfigHelper.FileNamesAndPaths.OutputPathDefaultExportDirectory + ConfigHelper.Database.DbFileName;
            OutputFile = outputFile;

            _tabsDbContext = new TabsDbContext(OutputFile);
        }

        public string Export(ITabsContainer tabsContainer)
        {
            System.IO.File.Delete(OutputFile); //Deletes if file already exists. //though due to having date in file name, it's impossible.
            _tabsDbContext.Database.EnsureCreated();
            _tabsDbContext.Tabs.AddRange(tabsContainer.AllTabInfs);
            _tabsDbContext.SaveChanges();

            return OutputFile;
        }
    }
}
