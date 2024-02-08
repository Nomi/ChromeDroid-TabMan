using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using ChromeDroid_TabMan.Auxiliary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    public class AdbConnection
    {
        private static AdbConnection _instance = null; 
        private static readonly object _lock = new object(); //For thread safety. //Another thread safe way to do it would simply be: public static AdbConnection Instace => new AdbConnection(); (Link: https://medium.com/peaceful-programmer/thread-safe-singleton-design-pattern-in-net-c-utilizing-the-c-properties-efficiently-25221fcd3b92)

        public AdbClient client { get; }
        public DeviceData device { get; }
        private AdbConnection(string adbPath) 
        {
            var adbClientAndDeviceData = ImportUtils.ConnectAndGetAdbClientAndDevice(adbPath);
            client = adbClientAndDeviceData.client;
            device = adbClientAndDeviceData.device;
        }
        public static AdbConnection GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                    throw new Exception("ERROR: Can't get instance of AdbConnection without establishing the connection.");
                return _instance;
            }
        }
        public static AdbConnection ConnectAndOrGetInstance(string adbPath)
        {
            lock(_lock)
            {
                if (_instance == null)
                    _instance = new AdbConnection(adbPath);
                return _instance;
            }
        }
    }
}
