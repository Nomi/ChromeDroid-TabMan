using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public struct BrowserDetailsStruct
    {
        public string BrowserName;
        public string PackageName;
        public string SocketNameFullOrPartial;
        public bool IsSocketNameFull;
        public bool DiscoveredOrRediscovered;
        public BrowserDetailsStruct(string browserName, string packageName, string socketNameFullOrPartial, bool isSocketNameFull, bool discoveredOrRediscovered)
        {
            BrowserName = browserName;
            PackageName = packageName;
            SocketNameFullOrPartial= socketNameFullOrPartial;
            IsSocketNameFull = isSocketNameFull;
        }
    }
    public class BrowserComboItem
    {

        public BrowserDetailsStruct BrowserDetails { get; set; }
        public string Name => ((BrowserDetails.IsSocketNameFull && BrowserDetails.SocketNameFullOrPartial!=BrowserDetails.BrowserName) ? (BrowserDetails.BrowserName + " (" + BrowserDetails.SocketNameFullOrPartial + ")" ): BrowserDetails.BrowserName);
        
        public BrowserComboItem(string name, string packageName, string socketNameFullOrPartial, bool isSocketNameFull, bool discoveredOrRediscovered)
        {
            BrowserDetails = new BrowserDetailsStruct(name, packageName,socketNameFullOrPartial,isSocketNameFull, discoveredOrRediscovered);
        }

    }
}
