using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public abstract class AbstractGameplay<T> : IGameplay where T : DataPlayerController
    {
        public abstract void EnterGameplay();
        public abstract void ExitGameplay();
        public abstract void ProcessEvents(string eventType, ISocketResponse onEventResponse);
        
        public abstract void PlayerEnter(T player);
        public abstract void PlayerExit(T player);
        public abstract void BeginGameRound();
        public abstract void EndGameRound();
        public abstract void ChangeState(string oldState, string newState);
    }
}
