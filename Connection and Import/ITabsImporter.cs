using ChromeDroid_TabMan.Data;
using ChromeDroid_TabMan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal interface ITabsImporter
    {
        public ITabsContainer Import();
    }
}
