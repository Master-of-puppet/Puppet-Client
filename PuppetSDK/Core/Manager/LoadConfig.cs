using System;
using System.Collections.Generic;
using Puppet.Utils;
using Puppet.Core.Model;
using Puppet.Core.Network.Http;

namespace Puppet.Core.Manager
{
    internal class LoadConfig
    {
        internal LoadConfig()
        {
            HttpPool.GetAppConfig();
            HttpPool.CheckVersion();
        }
    }
}
