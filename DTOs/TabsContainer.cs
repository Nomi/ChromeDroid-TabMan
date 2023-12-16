using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public class TabsContainer : ITabsContainer
    {
        public ICollection<TabInf> AllTabInfs { get; }
        public IDictionary<string, List<TabInf>> BaseUrlToTabInfCollectionMap { get; }

        public int CountOfBaseURLs => BaseUrlToTabInfCollectionMap.Keys.Count;

        public int Count => AllTabInfs.Count;

        public TabsContainer(ICollection<TabInf> tabInfs, IDictionary<string,List<TabInf>> baseUrlToTabInfCollectionMap)
        {
            AllTabInfs = tabInfs;
            BaseUrlToTabInfCollectionMap= baseUrlToTabInfCollectionMap;
        }
    }
}
