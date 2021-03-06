﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataGame : RoomInfo
    {
        public string description { get; set; }
        public string icon { get; set; }
        public string displayName { get; set; }

        public DataGame()
            : base()
        {
        }

        public DataGame(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}
