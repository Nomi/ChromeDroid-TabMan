using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Data
{
    internal interface ITabsExporter
    {
        public string OutputFile { get; }
        public string Export(ITabsContainer tabsContainer);
    }
}
