using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan
{
    class TabInf: IComparable<TabInf>
    {
        [JsonIgnore]
        public int tabPosition=-1;
        [JsonPropertyName("url")]
        public string url
        {
            get { return url; }
            set { url = value; baseWebsite = GetBasewebsite(); }
        }
        [JsonIgnore]
        public string title;
        [JsonIgnore]
        public string baseWebsite { get; private set; }
        [JsonPropertyName("title")]
        public string lastKnownTitle;

        public TabInf()
        {

        }
        //public TabInf(string url, string lkTitle)
        //{
        //    this.url = url;
        //    this.title = "title-to-be-implemented";
        //    this.lastKnownTitle = lkTitle;
        //    this.tabPosition = -1;
        ////    baseWebsite = GetBasewebsite();//Not needed after the set method i made for title
        //}
        public TabInf(string url, string title, int tabPos)
        {
            this.url = url;
            this.title = title;
            this.tabPosition = tabPos;
            baseWebsite = GetBasewebsite();
        }
        private string GetBasewebsite()
        {
            string baseURL = string.Empty;
            if (url.Contains("://"))
            {
                int indexAfterColonDoubleSlash = url.IndexOf("://", 0) + "://".Length;
                int indexNextSlash = url.IndexOf("/", indexAfterColonDoubleSlash);
                int ssLen = indexNextSlash - indexAfterColonDoubleSlash;
                baseURL = url.Substring(indexAfterColonDoubleSlash, ssLen);
            }
            return baseURL;
        }

        public int CompareTo(TabInf tab2)
        {
            return this.baseWebsite.CompareTo(tab2.baseWebsite);
        }
    }
}
