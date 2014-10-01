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
            game = new PokerGameplay();
            PuMain.GameLogic = game;
        }

        /// <summary>
        /// Call when Enter Plaza Game Poker
        /// </summary>
        public void EnterPoker()
        {

        }
    }
}
