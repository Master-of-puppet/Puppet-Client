using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using Puppet.Utils.Loggers;
using Puppet.Utils.Storage;
using Puppet.Utils.Threading;
using System;

namespace Puppet
{
    public interface IPuSettings
    {
        bool UseUnity { get; }
        bool IsDebug { get; set; }
        string ZoneName { get; set; }
        EPlatform Platform { get; set; }
        ServerEnvironment Environment { get; set; }
        IServerMode ServerModeHttp { get; set; }
        IServerMode ServerModeBundle { get; set; }
        IServerMode ServerModeSocket { get; set; }
        ISocket Socket { get; set; }
        Action ActionUpdate { get; set; }
        DataClientDetails ClientDetails { get; }
        string UniqueDeviceIdentification { get; }
        IStorage PlayerPref { get; }
        IThread Threading { get; }
        string PathCache { get; }
        IMainMono MainMono { get; }
        ENetworkDataType NetworkDataType { get; }
        

        void ChangeRealtimeServer(string server, int port);
        void ActionPrintLog(ELogType type, object message);
        void Init();
        void OnApplicationPause(bool pauseStatus);
        void OnApplicationQuit();
        void OnUpdate();
        void Reset();
    }
}
