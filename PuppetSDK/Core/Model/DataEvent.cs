using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppet.Core.Model
{
    public class DataResponseEvents : DataModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataEvent[] items { get; set; }

        public DataResponseEvents() : base() { }
    }

    public class DataEvent : DataModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string create_time { get; set; }
        public string image { get; set; }
        public string url { get; set; }
        public string display { get; set; }
        public string extra_url { get; set; }

        public DataEvent() : base() { }

        public EventType GetEventType ()
        {
            int type = 0;
            int.TryParse(display, out type);
            return (EventType)type;
        }
    }
}
