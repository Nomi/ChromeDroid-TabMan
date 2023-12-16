using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    internal interface ITabsContainer
    {
        ICollection<TabInf> AllTabInfs { get; }
        int CountOfBaseURLs { get; }
        int Count { get; }
        IDictionary<string, List<TabInf>> BaseUrlToTabInfCollectionMap {get;}
    }
}
