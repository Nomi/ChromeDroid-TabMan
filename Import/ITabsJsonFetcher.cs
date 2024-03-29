﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Connection_and_Import
{
    internal interface ITabsJsonFetcher
    {
#nullable enable
        IChromiumDevToolsConnector? _ChromiumDevToolsConnector { get; }
#nullable disable
        public string FetchTabsJson();
    }
}
