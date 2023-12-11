using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public class BrowserJsonVersionDTO
    {
        [JsonPropertyName("Android-Package")]
        public string AndroidPackageName {get;set;}

        [JsonPropertyName("Browser")]
        public string BrowserNameSlashVersion { get; set; }
        [JsonIgnore]
        public string OnlyNameBrowser => BrowserNameSlashVersion.Split('/')[0];
        [JsonIgnore]
        public string OnlyVersionBrowser => BrowserNameSlashVersion.Split('/')[2];


        /* //Example content of "/json/version" endpoint HTTP Get response:
         {
            "Android-Package": "com.android.chrome",
            "Browser": "Chrome/119.0.6045.194",
            "Protocol-Version": "1.3",
            "User-Agent": "...",                                                    //removed for privacy
            "V8-Version": "11.9.169.7",
            "WebKit-Version": "537.36 (@...)",                                      //partially removed for privacy
            "webSocketDebuggerUrl": "ws://localhost:9222/devtools/browser"
         }
        */
    }
}
