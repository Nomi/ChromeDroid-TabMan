using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Models
{
    public class TabInf : IComparable<TabInf>
    {
        [JsonIgnore]
        public int tabPosition { get; set; }
        [JsonPropertyName("url")]
        public string url { get; set; }
        [JsonIgnore]
        public string currentTitle { get; set; }
        [JsonIgnore]
        public string baseWebsite { get; set; }
        [JsonPropertyName("title")]
        public string lastKnownTitle { get; set; }

        public TabInf(string url, string lkTitle)
        {
            this.url = url;
            currentTitle = "title-to-be-implemented";
            lastKnownTitle = lkTitle;
            tabPosition = -1;
            baseWebsite = GetBasewebsite();
        }
        public TabInf(string url, string lkTitle, int tabPos)
        {
            this.url = url;
            this.lastKnownTitle = lkTitle;
            tabPosition = tabPos;
            baseWebsite = GetBasewebsite();
        }
        private string GetBasewebsite()
        {
            string baseURL = string.Empty;
            //The following approach should be faster than regex I think.
            if (url.Contains("://"))
            {
                int indexAfterColonDoubleSlash = url.IndexOf("://", 0) + "://".Length;
                int indexNextSlash = url.IndexOf("/", indexAfterColonDoubleSlash);
                if (indexNextSlash == -1)
                    indexNextSlash = url.Length;
                int ssLen = indexNextSlash - indexAfterColonDoubleSlash;
                if (ssLen>4)
                {
                    if (url[indexAfterColonDoubleSlash]=='w' && url[indexAfterColonDoubleSlash+1] == 'w' && url[indexAfterColonDoubleSlash+2]=='w' && url[indexAfterColonDoubleSlash+3]=='.')
                    {
                        indexAfterColonDoubleSlash += 4;
                        ssLen= indexNextSlash - indexAfterColonDoubleSlash;
                    }
                }
                baseURL = url.Substring(indexAfterColonDoubleSlash, ssLen);
            }
            return baseURL;
        }

        public int CompareTo(TabInf tab2)
        {
            return baseWebsite.CompareTo(tab2.baseWebsite);
        }
    }
}


///The following is how the file looked after some changes I made to it prior to reverting. Since I reverted and pulled the reverted version while writing this, this was never committed.
/*
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
        public string url { get; set; }
        //{
        //    get { return url; }
        //    set { SetBaseWebsite(value); url = value; }
        //}
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


        private void SetBaseWebsite(string _URL)
        {
            this.baseWebsite = string.Empty;            
            if (_URL.Contains("://"))
            {
                int indexAfterColonDoubleSlash = _URL.IndexOf("://", 0) + "://".Length;
                int indexNextSlash = _URL.IndexOf("/", indexAfterColonDoubleSlash);
                int ssLen = indexNextSlash - indexAfterColonDoubleSlash;
                this.baseWebsite = _URL.Substring(indexAfterColonDoubleSlash, ssLen);
            }
        }

    }
}

*/