//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public class BasicTabInf
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
        //[JsonProperty("title")]
        [JsonPropertyName("title")]
        public string lastKnownTitle { get; set; }
    }
}
