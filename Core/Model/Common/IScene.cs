using System;
using Puppet.Core.Network.Socket;

namespace Puppet.Core.Model
{
    public interface IScene
    {
        string SceneName { get; }
        EScene SceneType { get; }
        IScene NextScene { get; }
        IScene PrevScene { get; }
        void BeginScene();
        void EndScene();
        void ProcessEvents(string eventType, ISocketResponse onEventResponse);
    }
}
