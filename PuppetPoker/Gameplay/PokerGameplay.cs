using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker
{
    public class PokerGameplay : AbstractGameplay<PokerPlayerController>
    {
        PokerPlayerController[] listPlayer = new PokerPlayerController[Constant.MAX_PLAYER];

        public override void EnterGameplay()
        {

        }

        public override void ExitGameplay()
        {

        }

        public override void ProcessEvents(string eventType, ISocketResponse onEventResponse)
        {

        }

        public override void PlayerEnter(PokerPlayerController player)
        {
        }

        public override void PlayerExit(PokerPlayerController player)
        {
        }

        public override void BeginGameRound()
        {
        }

        public override void EndGameRound()
        {
        }

        public override void ChangeState(string oldState, string newState)
        {
        }
    }
}
