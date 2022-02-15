using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan
{
    public class BasicTabInf
    {
        [JsonPropertyName("url")]
        public string url;
        [JsonPropertyName("title")]
        public string lastKnownTitle;
    }
}
