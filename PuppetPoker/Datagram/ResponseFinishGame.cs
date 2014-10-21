﻿using Puppet.Core.Model;
using Puppet.Poker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Poker.Datagram
{
    public class ResponseFinishGame : DataModel
    {
        public string command { get; set; }
        public ResponseResultSummary []pots { get; set; }
        public ResponseMoneyExchange []totalExchange { get; set; }

        public ResponseFinishGame()
            : base()
        {
        }
    }

    public class ResponseMoneyExchange : DataModel
    {
        public string userName { get; set; }
        public int moneyExchange { get; set; }
        public bool winner { get; set; }
         
        public ResponseMoneyExchange()
            : base()
        {
        }
    }

    public class ResponseResultSummary : DataModel
    {
        public ResponseMoneyExchange []players { get; set; }
        public int potId { get; set; }
        public ResponseResultSummary()
            : base()
        {
        }
    }
}