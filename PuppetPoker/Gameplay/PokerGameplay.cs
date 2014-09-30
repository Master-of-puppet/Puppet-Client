using Puppet.Core.Model;
using Puppet.Core.Network.Socket;
using Puppet.Utils;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
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
            //"params":"Sfs2X.Entities.Data.SFSObject","cmd":"pluginMessage","sourceRoom":14
            if(eventType == SFSEvent.EXTENSION_RESPONSE)
            {
                if(onEventResponse.Params["cmd"].ToString() == "pluginMessage")
                {
                    SFSObject obj = (SFSObject)onEventResponse.Params["params"];
                    Logger.Log(obj.GetDump());
                }
            }

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
