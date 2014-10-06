﻿using Puppet.Core.Model;
using System;
using System.Collections.Generic;

namespace Puppet.Poker.Models
{
    public class PokerPlayerController : DataPlayerController
    {
        public int currentBet { get; set; }
        public string gameState { get; set; }
        public string action { get; set; }

        public PokerPlayerController() 
            : base()
        {
        }

        public PokerSide GetSide()
        {
            return PokerMain.Instance.game.GetSide(this);
        }

        public PokerPlayerState GetPlayerState()
        {
            object state = Enum.Parse(typeof(PokerPlayerState), gameState);
            if (state != null)
                return (PokerPlayerState)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ gameState: '{0}' sang enum PokerPlayerState", gameState);
                return PokerPlayerState.none;
            }
        }
    }
}
