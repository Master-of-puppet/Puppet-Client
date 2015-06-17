using System;
using System.Collections.Generic;
using System.Linq;

namespace Puppet.Poker
{
    public class PokerMain : BaseSingleton<PokerMain>
    {
        internal PokerGameplay game;
    
        protected override void Init()
        {
            PuMain.Dispatcher.onPreChangeScene += Dispatcher_onPreChangeScene;
        }

        void Dispatcher_onPreChangeScene(EScene fromScene, EScene toScene)
        {
            if(toScene == EScene.Pocker_Gameplay)
            {
                game = new PokerGameplay();
                PuMain.GameLogic = game;
            }
        }

        public void StartListen() { }
    }
}
