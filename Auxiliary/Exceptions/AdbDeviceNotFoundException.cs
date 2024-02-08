using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Auxiliary.Exceptions
{
    public class AdbDeviceNotFoundException : Exception
    {
        public AdbDeviceNotFoundException() : base("ERROR: ADB device either is not connected or was not found.") { }
    }
}
