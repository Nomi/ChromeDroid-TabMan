using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Auxiliary.Exceptions
{
    public class PidNotParsedException : Exception
    {
        public PidNotParsedException(string message = "PID could not be found or parsed. One possibility is that the process may not be running.") : base(message)
        {

        }
    }
}
