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

    public enum PokerSide
    {
        Slot_1 = 0,
        Slot_2 = 1,
        Slot_3 = 2,
        Slot_4 = 3,
        Slot_5 = 4,
        Slot_6 = 5,
        Slot_7 = 6,
        Slot_8 = 7,
        Slot_9 = 8,
    }
}
