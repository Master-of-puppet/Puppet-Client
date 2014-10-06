using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Poker
{
    public enum PokerGameState
    {
        none,
        waitingForPlayer,
        waitingDealing,
        dealing,
        playing,
        finalizing,
    }

    public enum PokerPlayerChangeAction
    {
        none,
        waitingPlayerAdded,
        waitingPlayerRemoved,
        playerAdded,
        playerRemoved,
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

    public enum PokerRequestPlay
    {
        /// <summary>
        /// Ngoi xuong
        /// </summary>
        SIT,
        /// <summary>
        /// Theo
        /// </summary>
        CALL,
        /// <summary>
        /// Xem bài
        /// </summary>
        CHECK,
        /// <summary>
        /// Tăng mức cược
        /// </summary>
        RAISE,
        /// <summary>
        /// Úp bài (bỏ cuộc)
        /// </summary>
        FOLD,
        /// <summary>
        /// Đặt cược bằng số tiền mình đang có
        /// </summary>
        ALL,
    }
}
