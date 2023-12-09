using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ChromeDroid_TabMan.Auxiliary;

namespace ChromeDroid_TabMan.Models
{
    public class TabInf : IComparable<TabInf>
    {
        [Key]
        [JsonIgnore]
        public int TabNum { get; set; } //might have something to do with position of tabs or the order in which they were opened.
        [JsonPropertyName("url")]
        public string URL { get; set; }
        //[JsonIgnore]
        //public string currentTitle { get; set; }
        [JsonPropertyName("title")]
        public string LastKnownTitle { get; set; }
        [JsonIgnore]
        public string BaseWebsite { get; set; }


        public TabInf() { } //exists for entity framework SQLite DB operations.
        //public TabInf(string url, string lkTitle)
        //{
        //    this.URL = url;
        //    //currentTitle = "title-to-be-implemented";
        //    LastKnownTitle = lkTitle;
        //    TabNum = -1;
        //    BaseWebsite = GetBaseWebsite();
        //}
        public TabInf(string url, string lkTitle, int tabNum)
        {
            this.URL = url;
            this.LastKnownTitle = lkTitle;
            if (LastKnownTitle.Length == 0)
                LastKnownTitle = url;
            TabNum = tabNum;
            BaseWebsite = GetBaseWebsite();
        }
        private string GetBaseWebsite()
        {
            string baseURL = ConfigHelper.UnidentifiedBaseUrlString;
            //The following approach should be faster than regex I think.
            if (URL.Contains("://"))
            {
                int indexAfterColonDoubleSlash = URL.IndexOf("://", 0) + "://".Length;
                int indexNextSlash = URL.IndexOf("/", indexAfterColonDoubleSlash);
                if (indexNextSlash == -1)
                    indexNextSlash = URL.Length;
                int ssLen = indexNextSlash - indexAfterColonDoubleSlash;
                if (ssLen>4)
                {
                    if (URL[indexAfterColonDoubleSlash]=='w' && URL[indexAfterColonDoubleSlash+1] == 'w' && URL[indexAfterColonDoubleSlash+2]=='w' && URL[indexAfterColonDoubleSlash+3]=='.')
                    {
                        indexAfterColonDoubleSlash += 4;
                        ssLen= indexNextSlash - indexAfterColonDoubleSlash;
                    }
                }
                baseURL = URL.Substring(indexAfterColonDoubleSlash, ssLen);
                if (!baseURL.Contains("."))
                    baseURL = ConfigHelper.UnidentifiedBaseUrlString;
            }
            return baseURL;
        }

        public int CompareTo(TabInf tab2)
        {
            return BaseWebsite.CompareTo(tab2.BaseWebsite);
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