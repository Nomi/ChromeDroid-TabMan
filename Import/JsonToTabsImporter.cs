using ChromeDroid_TabMan.Data;
using ChromeDroid_TabMan.DTOs;
using ChromeDroid_TabMan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal class JsonToTabsImporter : ITabsImporter
    {
        string JsonPath { get; set; }
        public JsonToTabsImporter(string jsonPath)
        {
            JsonPath = jsonPath;
        }
        public ITabsContainer Import()
        {
            var basicTabInfs = GetBasicTabInfsFromJson(); 

            List<TabInf> tabInfs = new();
            Dictionary<string, List<TabInf>> baseUrlToTabInfListMap = new();

            int basicTabInfsCount = basicTabInfs.Count;
            for(int i=0; i<basicTabInfsCount;i++ )
            {
                var basicTabInf = basicTabInfs[i];
                TabInf tabInf = new TabInf(basicTabInf.url, basicTabInf.lastKnownTitle, i + 1);

                tabInfs.Add(tabInf);
                baseUrlToTabInfListMap.TryAdd(tabInf.BaseWebsite, new());
                baseUrlToTabInfListMap[tabInf.BaseWebsite].Add(tabInf);
            }

            return new TabsContainer(tabInfs, baseUrlToTabInfListMap);
        }


        public List<BasicTabInf> GetBasicTabInfsFromJson()
        {
            string jsonText = File.ReadAllText(JsonPath);

            return System.Text.Json.JsonSerializer.Deserialize<List<BasicTabInf>>(jsonText); //JsonConvert.DeserializeObject<>(jsonText);
        }
    }
}
