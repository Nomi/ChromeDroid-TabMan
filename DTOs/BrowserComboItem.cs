using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public enum DiscoveryStateEnum
    {
        NotFound=0,
        NotSearchedFor,
        Verified,
        Discovered,
        RediscoveredAndFixed,
        RediscoveredAndFilledRestOfTheSocket
    };
    public struct BrowserDetailsStruct
    {
        public string BrowserName;
        public string PackageName;
        public string SocketNameFullOrPartial;
        public bool IsSocketNameFull;
        public DiscoveryStateEnum DiscoveryState;
        public BrowserDetailsStruct(string browserName, string packageName, string socketNameFullOrPartial, bool isSocketNameFull, DiscoveryStateEnum discoveryState)
        {
            BrowserName = browserName;
            PackageName = packageName;
            SocketNameFullOrPartial = socketNameFullOrPartial;
            IsSocketNameFull = isSocketNameFull;
            DiscoveryState = discoveryState;
        }
    }
    public class BrowserComboItem
    {

        public BrowserDetailsStruct BrowserDetails { get; set; }
        //public string Name => ((BrowserDetails.IsSocketNameFull && BrowserDetails.SocketNameFullOrPartial!=BrowserDetails.BrowserName) ? (BrowserDetails.BrowserName + " (" + BrowserDetails.SocketNameFullOrPartial + ")" ): BrowserDetails.BrowserName);
        public string Name
        {
            get
            {
                string prefix;
                string suffix = string.Empty; //string suffix = (BrowserDetails.PackageName == null || BrowserDetails.DiscoveryState==DiscoveryStateEnum.NotSearchedFor) ? string.Empty : " (" + BrowserDetails.PackageName + ")";
                switch (BrowserDetails.DiscoveryState)
                {
                    case DiscoveryStateEnum.NotSearchedFor:
                        prefix= string.Empty;
                        suffix=string.Empty;
                        break;
                    case DiscoveryStateEnum.NotFound:
                        prefix = "[✘] ";
                        break;
                    case DiscoveryStateEnum.Verified:
                        prefix = "[✔] ";
                        break;
                    case DiscoveryStateEnum.RediscoveredAndFilledRestOfTheSocket:
                        prefix = "[✔*] ";
                        break;
                    case DiscoveryStateEnum.RediscoveredAndFixed:
                        prefix = "[ⒻⒾⓍⒺⒹ] "; //"[**✔**] ";
                        break;
                    case DiscoveryStateEnum.Discovered:
                        prefix = "[🔍] ";
                        suffix = " (" + BrowserDetails.PackageName + ")";
                        break;
                    default:
                        throw new Exception("Unsupported DiscoveryState.");
                }
                return prefix + BrowserDetails.BrowserName + suffix;

            }
        }
        public BrowserComboItem(string name, string packageName, string socketNameFullOrPartial, bool isSocketNameFull, DiscoveryStateEnum discoveryState)
        {
            BrowserDetails = new BrowserDetailsStruct(name, packageName,socketNameFullOrPartial,isSocketNameFull, discoveryState);
        }

    }
}
