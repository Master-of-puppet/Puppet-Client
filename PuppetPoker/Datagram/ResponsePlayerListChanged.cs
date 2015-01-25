using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponsePlayerListChanged : DataModel
    {
        public string command { get; set; }
        public PokerPlayerController player { get; set; }
        public string action { get; set; }

        public ResponsePlayerListChanged()
            : base()
        {
        }

        public PokerPlayerChangeAction GetActionState()
        {
            if (string.IsNullOrEmpty(action))
                return PokerPlayerChangeAction.none;

            object state = Enum.Parse(typeof(PokerPlayerChangeAction), action);
            if (state != null)
                return (PokerPlayerChangeAction)state;
            else
            {
                Logger.LogWarning("Không thể chuyển đổi từ action: '{0}' sang enum PokerActionPlayerChange", action);
                return PokerPlayerChangeAction.none;
            }
        }
    }
}
