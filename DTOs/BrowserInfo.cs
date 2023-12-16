using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public enum DiscoveryStateEnum
    {
        NotFound = 0,
        NotSearchedFor,
        Verified,
        Discovered,
        RediscoveredAndFixed,
        RediscoveredAndFilledRestOfTheSocket
    };
    public class BrowserInfo
    {
        public string BrowserName { get; set; }
        public string PackageName { get; set; }
        public SocketInfo Socket { get; set; }

        private DiscoveryStateEnum _discoveryState;
        public DiscoveryStateEnum DiscoveryState
        {
            get { return _discoveryState; }
            set
            {
                switch (value)
                {

                    case DiscoveryStateEnum.Verified:
                    case DiscoveryStateEnum.RediscoveredAndFilledRestOfTheSocket:
                    case DiscoveryStateEnum.RediscoveredAndFixed:
                    case DiscoveryStateEnum.Discovered:
                        Socket.IsSocketNameVerifiedCorrect = true;
                        Socket.IsSocketNameComplete = true;
                        _discoveryState= value;
                        break;


                    case DiscoveryStateEnum.NotFound:
                    case DiscoveryStateEnum.NotSearchedFor:
                        Socket.IsSocketNameVerifiedCorrect = false;
                        _discoveryState = value;
                        break;


                    default:
                        throw new Exception("Unsupported DiscoveryState.");
                }
            }
        }
        public BrowserInfo(string browserName, string packageName, string socketNameIncludingConnBase, bool isSocketNameFull ,DiscoveryStateEnum discoveryState)
        {
            BrowserName = browserName;
            PackageName = packageName;
            
            Socket = new SocketInfo(socketNameIncludingConnBase, isSocketNameFull, false);
            DiscoveryState = discoveryState;
        }

        public BrowserInfo(string browserName, string packageName, string socketName, string connBase, bool isSocketNameFull, DiscoveryStateEnum discoveryState)
        {
            BrowserName = browserName;
            PackageName = packageName;

            Socket = new SocketInfo(socketName, connBase, isSocketNameFull, false);
            DiscoveryState = discoveryState;
        }

        public BrowserInfo(string browserName, string packageName, SocketInfo socket, DiscoveryStateEnum discoveryState)
        {
            BrowserName = browserName;
            PackageName = packageName;

            Socket = socket;
            DiscoveryState = discoveryState;
        }
    }
}
