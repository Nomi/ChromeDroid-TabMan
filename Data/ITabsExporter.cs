using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Data
{
    internal interface ITabsExporter
    {
        TabsList tabsList { get; }
        public string ExportTabs();
    }
}
