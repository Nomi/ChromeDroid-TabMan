using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.DTOs
{
    public class SocketInfo
    {
        public string SocketConnectionStr => this.ToString();
        public string ConnBase { get; } //something like "localabstract"
        public string Name { get; set; }
        public bool IsSocketNameComplete { get; set; }
        public bool IsSocketNameVerifiedCorrect { get; set; }

        public SocketInfo(string socketNameOnly, string connBaseOnly, bool isSocketNameCompleteAndCorrect, bool isSocketNameVerifiedCorrect=false)
        {
            //ConnBase = (connBase != null) ? connBase : ConfigHelper.ADB.BaseLocalForwardedURL;
            ConnBase = ConnBase;
            Name = socketNameOnly;
            IsSocketNameComplete = isSocketNameCompleteAndCorrect;
            IsSocketNameVerifiedCorrect = isSocketNameVerifiedCorrect;
        }

        public SocketInfo(string RemoteForward_Remote__FullString, bool isSocketNameCompleteAndCorrect, bool isSocketNameVerifiedCorrect) //, int AnyRandomIntToUseThisConstructor
        {
            string[] sock = RemoteForward_Remote__FullString.Split(":");
            ConnBase = sock[0];
            Name = sock[1];
            IsSocketNameComplete = isSocketNameCompleteAndCorrect;
            IsSocketNameVerifiedCorrect = isSocketNameVerifiedCorrect;
            //int num = AnyRandomIntToUseThisConstructor;
            //num = 0;
        }

        public override string ToString()
        {
            return ConnectTwoStringsWithSemicolonInBetween(ConnBase, Name);
        }


        private static string ConnectTwoStringsWithSemicolonInBetween(string str1, string str2)
        {
            return str1 + ":" + str2;
        }
    }
}
