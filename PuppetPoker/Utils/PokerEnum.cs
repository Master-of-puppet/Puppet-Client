using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker
{
    public enum GameState
    {
        none = 0,
        waiting = 1,
        ready = 2,
        waitingForTurn = 3,
        inTurn = 4,
        outTurn = 5,
        finish = 8
    }
}
