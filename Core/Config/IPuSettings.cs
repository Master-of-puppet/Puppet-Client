using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;

namespace Puppet
{
    public interface IPuSettings
    {
        EPlatform Platform { get; set; }
        ServerEnvironment Environment { get; set; }
        IServerMode ServerModeHttp { get; set; }
        IServerMode ServerModeBundle { get; set; }
        IServerMode ServerModeSocket { get; set; }

        IStorage PlayerPref { get; }
        IThread Threading { get; }
        string PathCache { get; }

        void ActionChangeScene(string fromScene, string toScene);
        void ActionPrintLog(ELogType type, object message);
    }
}
