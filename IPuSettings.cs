using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;

namespace Puppet
{
    public enum EPlatform
    {
        Editor,
        iOS,
        Android,
        WebPlayer,
    }

    public interface IPuSettings
    {
        EPlatform Platform { get; }
        ServerEnvironment Environment { get; }
        IServerMode ServerModeHttp { get; }
        IServerMode ServerModeBundle { get; }
        IServerMode ServerModeSocket { get; }
        string PathCache { get; }

        IStorage PlayerPref { get; }
        IThread Threading { get; }

        void ActionChangeScene(string fromScene, string toScene);
        void ActionPrintLog(ELogType type, object message);
    }
}
