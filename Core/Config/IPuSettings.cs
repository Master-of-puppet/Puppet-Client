﻿using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;

namespace Puppet
{
    public interface IPuSettings
    {
        DataClientDetails ClientDetails { get; set; }
        string ZoneName { get; set; }
        EPlatform Platform { get; set; }
        ServerEnvironment Environment { get; set; }
        IServerMode ServerModeHttp { get; set; }
        IServerMode ServerModeBundle { get; set; }
        IServerMode ServerModeSocket { get; set; }
        ISocket Socket { get; set; }

        IStorage PlayerPref { get; }
        IThread Threading { get; }
        /// <summary>
        /// Path to save cache
        /// </summary>
        string PathCache { get; }
        
        void ActionChangeScene(string fromScene, string toScene);
        void ActionPrintLog(ELogType type, object message);
        void Init();
    }
}
