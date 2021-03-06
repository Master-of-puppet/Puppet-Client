﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataLobby : RoomInfo
    {
        public string description { get; set; }
        public string icon { get; set; }
        public string displayName { get; set; }
        public int index { get; set; }
        public DataPlayerController [] users { get; set; }
        public LobbyConfig gameDetails { get; set; }

        public DataLobby() 
            : base()
        {
        }

        public DataLobby(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
    	{				
   	 	}

        public class LobbyConfig : AbstractData
        {
			public double betting { get; set;}		
            public int numPlayers { get; set; }
        }
    }
}
