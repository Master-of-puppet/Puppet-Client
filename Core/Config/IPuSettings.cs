using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;

namespace Puppet
{
    public interface IPuSettings
    {
        /// <summary>
        /// deltaTime value for per frame in Unity3D
        /// </summary>
        float DeltaTime { get; set; }
        /// <summary>
        /// Zone Name in SmartFox
        /// </summary>
        string ZoneName { get; set; }
        EPlatform Platform { get; set; }
        ServerEnvironment Environment { get; set; }
        IServerMode ServerModeHttp { get; set; }
        IServerMode ServerModeBundle { get; set; }
        IServerMode ServerModeSocket { get; set; }

        IStorage PlayerPref { get; }
        IThread Threading { get; }
        /// <summary>
        /// Path to save cache
        /// </summary>
        string PathCache { get; }
        
        void ActionChangeScene(string fromScene, string toScene);
        void ActionPrintLog(ELogType type, object message);
    }
}
